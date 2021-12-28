using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Wman.Data.DB_Models;
using Wman.Logic.Classes;
using Wman.Repository.Interfaces;
using Wman.Test.Builders;
using Wman.Test.Builders.LogicBuilders;

namespace Wman.Test.Tests
{
    public class AllInWorkEventTests
    {
        Mock<UserManager<WmanUser>> userManager;
        Mock<IWorkEventRepo> eventRepo;
        Mock<ILabelRepo> labelRepo;
        
        private IMapper mapper;

        private List<WmanUser> users;
        private List<Label> labelList;
        private List<WorkEvent> eventList;

        [SetUp]
        public void Setup()
        {
            this.users = UserManagerBuilder.GetWmanUsers();
            this.eventList = EventLogicBuilder.GetWorkEvents();
            this.labelList = LabelLogicBuilder.GetLabels();

            this.mapper = MapperBuilder.GetMapper();

            this.userManager = UserManagerBuilder.GetUserManager(this.users);
            this.eventRepo = EventLogicBuilder.GetEventRepo(this.eventList);
            this.labelRepo = LabelLogicBuilder.GetLabelRepo(this.labelList);
        }

        [Test]
        public async Task ForWorkCard_ReturnsWorkEventProperly_SuccessfulOperation()
        {
            //Arrange
            AllInWorkEventLogic workLogic = new(this.userManager.Object,
                this.eventRepo.Object, this.labelRepo.Object, this.mapper);
            int helperId = eventList[0].Id;

            //Act
            var call = await workLogic.ForWorkCard(helperId);

            //Assert
            Assert.That(call.Id == helperId);
            this.eventRepo.Verify(x => x.GetOne(It.IsAny<int>()), Times.Once);
        }

        [Test]
        public async Task Available()
        {
            //Arrange
            AllInWorkEventLogic workLogic = new(this.userManager.Object,
                this.eventRepo.Object, this.labelRepo.Object, this.mapper);
            DateTime fromDate = DateTime.UtcNow.AddDays(-2);
            DateTime toDate = DateTime.UtcNow.AddDays(1);

            //Act
            var call = await workLogic.Available(fromDate, toDate);

            //Assert
            this.userManager.Verify(x => x.Users, Times.Exactly(2));
            this.eventRepo.Verify(x => x.GetAll(), Times.Once);
            this.userManager.Verify(x => x.GetRolesAsync(It.IsAny<WmanUser>()), Times.Exactly(3));//there are 3 users in the test repo
        }
    }
}
