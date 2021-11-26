using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Wman.Data.DB_Models;
using Wman.Logic.Classes;
using Wman.Logic.DTO_Models;
using Wman.Logic.Helpers;
using Wman.Repository.Interfaces;
using Wman.Test.Builders;
using Wman.Test.Builders.LogicBuilders;

namespace Wman.Test.Tests
{
    public class EventLogicTests
    {
        private Mock<IWorkEventRepo> eventRepo;
        private Mock<IAddressRepo> addressRepo;

        private IMapper mapper;

        private List<WorkEvent> eventList;
        private List<AddressHUN> addressList;

        private Mock<UserManager<WmanUser>> userManager;
        private List<WmanUser> users;

        [SetUp]
        public void SetUp()
        {
            this.users = UserManagerBuilder.GetWmanUsers();
            this.userManager = UserManagerBuilder.GetUserManager(users);

            this.mapper = MapperBuilder.GetMapper();

            this.eventList = EventLogicBuilder.GetWorkEvents();
            this.addressList = EventLogicBuilder.GetAddresses();

            this.eventRepo = EventLogicBuilder.GetEventRepo(eventList);
            this.addressRepo = EventLogicBuilder.GetAddressRepo(addressList);
        }

        [Test]
        [TestCase("2021-10-10", "2021-10-10")]
        [TestCase("2021-10-10", null)]
        [TestCase(null, "2021-10-10")]
        [TestCase(null, null)]
        public async Task DnDEvent_WrongInput_ThrowsException_FailedOperation(DateTime startDate, DateTime finishDate)
        {
            //Arrange
            EventLogic eventLogic = new EventLogic(this.eventRepo.Object, this.mapper, this.addressRepo.Object, this.userManager.Object);
            DnDEventDTO eventDTO = new DnDEventDTO()
            {
                EstimatedStartDate = startDate,
                EstimatedFinishDate = finishDate
            };
            int desiredId = eventList[1].Id;

            //Act
            AsyncTestDelegate testDelegate = async () => await eventLogic.DnDEvent(desiredId, eventDTO);

            //Assert
            Assert.ThrowsAsync<InvalidOperationException>(testDelegate);

            this.eventRepo.Verify(x => x.GetOneWithTracking(It.IsAny<int>()), Times.Never);
        }

        [Test]
        public async Task DnDEvent_AssignUser_NoConflictsDuringDnDEvent_SuccessfulOperation()
        {
            //Arrange
            EventLogic eventLogic = new EventLogic(this.eventRepo.Object, this.mapper, this.addressRepo.Object, this.userManager.Object);
            DnDEventDTO eventDTO = new DnDEventDTO()
            {
                EstimatedStartDate = eventList[0].EstimatedStartDate,
                EstimatedFinishDate = eventList[0].EstimatedFinishDate
            };
            int desiredId = eventList[1].Id;

            //Act
            var assignCall = eventLogic.AssignUser(desiredId, users[0].UserName);
            var mainCall = eventLogic.DnDEvent(desiredId, eventDTO);

            //Assert
            Assert.That(assignCall.IsCompleted && mainCall.IsCompleted);

            this.eventRepo.Verify(x => x.GetOneWithTracking(It.IsAny<int>()), Times.Exactly(2));
            this.userManager.Verify(x => x.Users, Times.Once);
            this.userManager.Verify(x => x.IsInRoleAsync(It.IsAny<WmanUser>(), It.IsAny<string>()), Times.Once);
            this.eventRepo.Verify(x => x.GetAll(), Times.Once);
            this.eventRepo.Verify(x => x.Update(It.IsAny<int>(), It.IsAny<WorkEvent>()), Times.Once);
        }

        [Test]
        public async Task MassAssignUser_AssignMultipleUsers_Successfull()
        {
            //Arrange
            EventLogic eventLogic = new EventLogic(this.eventRepo.Object, this.mapper, this.addressRepo.Object, this.userManager.Object);
            List<string> userNames = new List<string>() { users[0].UserName, users[1].UserName };

            //Act
            var call = eventLogic.MassAssignUser(eventList[0].Id, userNames);

            //Assert
            Assert.That(call.IsCompleted);

            this.eventRepo.Verify(x => x.GetOneWithTracking(It.IsAny<int>()), Times.Once);
            this.userManager.Verify(x => x.Users, Times.Exactly(userNames.Count));
            this.userManager.Verify(x => x.IsInRoleAsync(It.IsAny<WmanUser>(), It.IsAny<string>()), Times.Exactly(userNames.Count));
            this.eventRepo.Verify(x => x.Update(It.IsAny<int>(), It.IsAny<WorkEvent>()), Times.Exactly(userNames.Count));
        }

        [Test]
        [TestCase(4,"NonexistentUser")]
        [TestCase(0,"12ö934öüakíyxkl")]
        [TestCase(0,null)]
        [TestCase(null,null)]
        public async Task AssignUser_AssignBadValues_ExceptionCaught(int idInput, string userInput)
        {
            //Arrange
            EventLogic eventLogic = new EventLogic(this.eventRepo.Object, this.mapper, this.addressRepo.Object, this.userManager.Object);

            //Act
            AsyncTestDelegate testDelegate = async () => await eventLogic.AssignUser(idInput, userInput);

            //Assert
            Assert.ThrowsAsync<NotFoundException>(testDelegate);

            this.eventRepo.Verify(x => x.Update(It.IsAny<int>(), It.IsAny<WorkEvent>()), Times.Never);
            this.userManager.Verify(x => x.IsInRoleAsync(It.IsAny<WmanUser>(), It.IsAny<string>()), Times.Never);
        }

        [Test]
        public async Task AssignUser_AssignToExistingUser_Successful()
        {
            //Arrange
            EventLogic eventLogic = new EventLogic(this.eventRepo.Object, this.mapper, this.addressRepo.Object, this.userManager.Object);

            //Act
            var call = eventLogic.AssignUser(eventList[0].Id, users[0].UserName);

            //Assert
            Assert.That(call.IsCompleted);

            this.eventRepo.Verify(x => x.GetOneWithTracking(It.IsAny<int>()), Times.Once);
            this.userManager.Verify(x => x.Users, Times.Once);
            this.userManager.Verify(x => x.IsInRoleAsync(It.IsAny<WmanUser>(), It.IsAny<string>()), Times.Once);
            this.eventRepo.Verify(x => x.Update(It.IsAny<int>(), It.IsAny<WorkEvent>()), Times.Once);
        }

        [Test]
        public async Task GetAllAssignedUsers_AssignExistingUser_ReturnsSuccessfully()
        {
            //Arrange
            EventLogic eventLogic = new EventLogic(this.eventRepo.Object, this.mapper, this.addressRepo.Object, this.userManager.Object);

            //Act
            await eventLogic.AssignUser(eventList[0].Id, users[0].UserName);
            var call = await eventLogic.GetAllAssignedUsers(eventList[0].Id);

            //Assert
            Assert.AreEqual(1, call.Count());

            this.eventRepo.Verify(x => x.GetOneWithTracking(It.IsAny<int>()), Times.Once);
            this.userManager.Verify(x => x.Users, Times.Once);
            this.userManager.Verify(x => x.IsInRoleAsync(It.IsAny<WmanUser>(), It.IsAny<string>()), Times.Once);
            this.eventRepo.Verify(x => x.Update(It.IsAny<int>(), It.IsAny<WorkEvent>()), Times.Once);
            this.eventRepo.Verify(x => x.GetOne(It.IsAny<int>()), Times.Once);
        }

        [Test]
        public async Task UpdateEvent_UpdateExisitingEvent_SuccessfulOperation()
        {
            //Arrange
            EventLogic eventLogic = new EventLogic(this.eventRepo.Object, this.mapper, this.addressRepo.Object, this.userManager.Object);

            UpdateEventDTO workEvent = new UpdateEventDTO
            {
                Id= eventList[0].Id,
                JobDescription = "LucsokMokos",
                EstimatedStartDate = DateTime.UtcNow,
                EstimatedFinishDate = DateTime.UtcNow.AddMinutes(20),
                Status = Status.started
            };

            //Act
            var result = eventLogic.UpdateEvent(workEvent);

            //Assert
            this.eventRepo.Verify(x => x.GetOneWithTracking(It.IsAny<int>()), Times.Once);
            this.eventRepo.Verify(x => x.SaveDatabase(), Times.Once);
        }

        [Test]
        public async Task DeleteEvent_SuccessfullyDeletesElement()
        {
            //Arrange
            EventLogic eventLogic = new EventLogic(this.eventRepo.Object, this.mapper, this.addressRepo.Object, this.userManager.Object);

            //Act
            var result = eventLogic.DeleteEvent(eventList[0].Id);

            //Assert
            this.eventRepo.Verify(x => x.Delete(It.IsAny<int>()), Times.Once);
        }

        [Test]
        public async Task GetAllEvents_ReturnsRepoCorrectly()
        {
            //Arrange
            EventLogic eventLogic = new EventLogic(this.eventRepo.Object, this.mapper, this.addressRepo.Object, this.userManager.Object);

            //Act
            var result = await eventLogic.GetAllEvents();

            //Assert
            Assert.That(result.Count() == eventList.Count());
            this.eventRepo.Verify(x => x.GetAll(), Times.Once);
        }

        [Test]
        public async Task GetEvent_ReturnsWorkEvent_CompareToEventInList_Successful()
        {
            //Arrange
            EventLogic eventLogic = new EventLogic(this.eventRepo.Object, this.mapper, this.addressRepo.Object, this.userManager.Object);

            //Act
            var result = await eventLogic.GetEvent(eventList[0].Id);

            //Assert
            Assert.That(result.Id == eventList[0].Id);
            this.eventRepo.Verify(x => x.GetOne(It.IsAny<int>()), Times.Once);
        }

        [Test]
        public async Task CreateEvent_SuccessfulCreation()
        {
            //Arrange
            EventLogic eventLogic = new EventLogic(this.eventRepo.Object, this.mapper, this.addressRepo.Object, this.userManager.Object);

            var helper = new AddressHUNDTO()
            {
                City = addressList[0].City,
                BuildingNumber = addressList[0].BuildingNumber,
                FloorDoor = addressList[0].Floordoor,
                Street = addressList[0].Street,
                ZIPCode = int.Parse(addressList[0].ZIPCode),
            };

            CreateEventDTO eventDTO = new CreateEventDTO()
            {
                JobDescription = "ValmiTestXDLOLNI",
                EstimatedStartDate = new DateTime(2021,10,12,8,10,15),
                EstimatedFinishDate = new DateTime(2021,10,12,9,0,0),
                Address = helper,
                Status = Status.awaiting
            };

            //Act
            await eventLogic.CreateEvent(eventDTO);

            //Assert
            this.eventRepo.Verify(x => x.Add(It.IsAny<WorkEvent>()), Times.Once);
        }
    }
}
