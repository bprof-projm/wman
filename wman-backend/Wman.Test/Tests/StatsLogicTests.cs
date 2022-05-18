using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Identity;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;
using Wman.Data.DB_Models;
using Wman.Logic.Classes;
using Wman.Logic.Services;
using Wman.Repository.Interfaces;
using Wman.Test.Builders;
using Wman.Test.Builders.LogicBuilders;
using System;
using System.IO;

namespace Wman.Test.Tests
{
    public class StatsLogicTests
    {
        private Mock<IWorkEventRepo> eventRepo;
        private List<WorkEvent> eventList;

        private Mock<UserManager<WmanUser>> userManager;
        private List<WmanUser> users;

        private Mock<IFileRepo> fileRepo;

        private Mock<IEmailService> emailService;
        private IConfiguration config;

        [SetUp]
        public void SetUp()
        {
            this.eventList = StatsLogicBuilder.GetWorkEventsWithFinishedStatus();
            this.users = UserManagerBuilder.GetWmanUsers();

            UserLogicBuilder.AssignWorkEvents(this.users, this.eventList);

            this.eventRepo = EventLogicBuilder.GetEventRepo(this.eventList);

            this.userManager = UserManagerBuilder.GetUserManager(this.users);

            this.emailService = EventLogicBuilder.GetEmailService();
            this.config = AspConfigurationBuilder.GetConfiguration();

            this.fileRepo = StatsLogicBuilder.GetFileRepo();
        }

        [Test]
        public async Task GetStatsBasicTest()
        {
            //Arrange
            StatsLogic statsLogic = new(eventRepo.Object, fileRepo.Object, config, emailService.Object, userManager.Object);
            DateTime input = DateTime.Now;

            //Act
            var call = await statsLogic.GetManagerStats(input);

            //Assert
            Assert.IsNotNull(call);
            Assert.That(this.eventList.Count == call.Count);
            //GetStats
            this.eventRepo.Verify(x => x.GetAll(), Times.Once);
            //makexls
            this.fileRepo.Verify(x => x.Create(It.IsAny<string>(), It.IsAny<Stream>()), Times.Once);
            //SendEmails
            this.fileRepo.Verify(x => x.GetDetails(It.IsAny<string>()), Times.Never);
            this.userManager.Verify(x => x.GetUsersInRoleAsync("Manager"), Times.Once);
            this.emailService.Verify(x => x.SendXls(It.IsAny<WmanUser>(), It.IsAny<string>()));
        }
    }
}
