using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Wman.Data.DB_Models;
using Wman.Logic.Classes;
using Wman.Repository.Interfaces;
using Wman.Test.Builders;
using Wman.Test.Builders.LogicBuilders;

namespace Wman.Test.Tests
{
    public class UserLogicTests
    {
        private Mock<UserManager<WmanUser>> userManager;
        private IMapper mapper;

        private List<WmanUser> users;

        private Mock<IWorkEventRepo> eventRepo;
        private List<WorkEvent> eventList;

        [SetUp]
        public void SetUp()
        {
            this.users = UserManagerBuilder.GetWmanUsers();
            this.eventList = EventLogicBuilder.GetWorkEvents();
            UserLogicBuilder.AssignWorkEvents(this.users, this.eventList);

            this.eventRepo = EventLogicBuilder.GetEventRepo(this.eventList);
            this.userManager = UserManagerBuilder.GetUserManager(this.users);

            this.mapper = MapperBuilder.GetMapper();
        }

        [Test]
        public async Task GetWorkLoads_ReturnsRepoProperly_SuccessfulOperation()
        {
            //Arrange
            UserLogic userLogic = new(this.userManager.Object, this.mapper, this.eventRepo.Object);

            //Act
            var call = await userLogic.GetWorkLoads();

            //Assert
            Assert.That(call.ElementAt(2).Username == users[2].UserName);
            this.userManager.Verify(x => x.Users, Times.Once);
            this.userManager.Verify(x => x.IsInRoleAsync(It.IsAny<WmanUser>(), It.IsAny<string>()), Times.Exactly(users.Count()));
        }

        [Test]
        public async Task GetWorkLoads_WithParameters_ReturnsRepoProperly_SuccessfulOperation()
        {
            //Arrange
            UserLogic userLogic = new(this.userManager.Object, this.mapper, this.eventRepo.Object);
            List<string> usernames = new() { users[0].UserName, users[1].UserName };

            //Act
            var call = await userLogic.GetWorkLoads(usernames);

            //Assert
            Assert.That(call.ElementAt(1).Username == usernames[1]);
            this.userManager.Verify(x => x.Users, Times.Once);
            this.userManager.Verify(x => x.IsInRoleAsync(It.IsAny<WmanUser>(), It.IsAny<string>()), Times.Exactly(usernames.Count()));
        }

        [Test]
        public async Task GetEventDetailsForWorker_ReturnsWorkEventProperly()
        {
            //Arrange
            UserLogic userLogic = new(this.userManager.Object, this.mapper, this.eventRepo.Object);
            string usernameToTest = users[0].UserName;
            int idOfWorkEvent = eventList[0].Id;

            //Act
            var call = await userLogic.GetEventDetailsForWorker(usernameToTest, idOfWorkEvent);

            //Assert
            Assert.That(call.Id == users[0].WorkEvents.ElementAt(0).Id);
            this.eventRepo.Verify(x => x.GetOne(It.IsAny<int>()), Times.Once);
        }

        /*
         * DEPRECATED METHODS
        [Test]
        public async Task WorkEventsOfUser_ReturnsWorkEventProperly()
        {
            //Arrange
            UserLogic userLogic = new(this.userManager.Object, this.mapper, this.eventRepo.Object);
            string usernameToTest = users[0].UserName;

            //Act
            var call = await userLogic.WorkEventsOfUser(usernameToTest);

            //Assert
            Assert.That(call.ElementAt(0).Id == users[0].WorkEvents.ElementAt(0).Id);
            this.userManager.Verify(x => x.Users, Times.Once);
        }*/
    }
}
