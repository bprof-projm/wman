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
using Wman.Repository.Classes;

namespace Wman.Test.Tests
{
    public class StatsLogicTests
    {
        private Mock<IWorkEventRepo> eventRepo;
        private List<WorkEvent> eventList;
        
        private Mock<UserManager<WmanUser>> userManager;
        private List<WmanUser> users;

        private FileRepo fileRepo;
 
        private Mock<IEmailService> emailService;
        private IConfiguration config;

        [SetUp]
        public void SetUp()
        {
            this.eventList = EventLogicBuilder.GetWorkEvents();
            this.eventRepo = EventLogicBuilder.GetEventRepo(this.eventList);

            this.users = UserManagerBuilder.GetWmanUsers();
            this.userManager = UserManagerBuilder.GetUserManager(users);

            this.emailService = EventLogicBuilder.GetEmailService();
            this.config = AspConfigurationBuilder.GetConfiguration();
        }

        [Test]
        public async Task GetStatsBasicTest()
        {
            //Arrange
            StatsLogic statsLogic = new(eventRepo.Object, fileRepo, config, emailService.Object, userManager.Object);
            DateTime input = DateTime.Now;

            //Act
            var call = await statsLogic.GetStats(input);

            //Assert
        }
    }
}
