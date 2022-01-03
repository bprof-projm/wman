using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using Wman.Data.DB_Models;
using Wman.Logic.Classes;
using Wman.Repository.Interfaces;
using Wman.Test.Builders;
using Wman.Test.Builders.LogicBuilders;

namespace Wman.Test.Tests
{
    public class CalendarEventLogicTests
    {
        private Mock<IWorkEventRepo> eventRepo;
        private List<WorkEvent> eventList;

        private IMapper mapper;

        private Mock<UserManager<WmanUser>> userManager;
        private List<WmanUser> users;

        [SetUp]
        public void SetUp()
        {
            this.eventList = EventLogicBuilder.GetWorkEvents();
            this.users = UserManagerBuilder.GetWmanUsers();
            UserLogicBuilder.AssignWorkEvents(this.users, this.eventList);

            this.mapper = MapperBuilder.GetMapper();

            this.eventRepo = EventLogicBuilder.GetEventRepo(this.eventList);
            this.userManager = UserManagerBuilder.GetUserManager(this.users);
        }

        [Test]
        [TestCase(-1)]
        [TestCase(54)]
        [TestCase(140)]
        [TestCase(null)]
        public async Task GetWeekEvents_IntParameter_InvalidParametersGiven_ExceptionExpected(int testInput)
        {
            //Arrange
            CalendarEventLogic calendarLogic = new(this.eventRepo.Object, this.mapper, this.userManager.Object);

            //Act
            async Task testDelegate() => await calendarLogic.GetWeekEvents(testInput, users[0].UserName);

            //Assert
            Assert.ThrowsAsync<ArgumentException>(testDelegate);

            this.eventRepo.Verify(x => x.GetAll(), Times.Never);
            this.userManager.Verify(x => x.Users, Times.Once);
            this.userManager.Verify(x => x.IsInRoleAsync(It.IsAny<WmanUser>(), It.IsAny<string>()), Times.Never);
        }

        [Test]
        [TestCase(-200)]
        [TestCase(-1)]
        [TestCase(368)]
        [TestCase(null)]
        public async Task GetDayEvents_IntParameter_InvalidParametersGiven_ExceptionExpected(int testInput)
        {
            //Arrange
            CalendarEventLogic calendarLogic = new(this.eventRepo.Object, this.mapper, this.userManager.Object);

            //Act
            async Task testDelegate() => await calendarLogic.GetDayEvents(testInput, users[0].UserName);

            //Assert
            Assert.ThrowsAsync<ArgumentException>(testDelegate);

            this.eventRepo.Verify(x => x.GetAll(), Times.Never);
            this.userManager.Verify(x => x.Users, Times.Once);
            this.userManager.Verify(x => x.IsInRoleAsync(It.IsAny<WmanUser>(), It.IsAny<string>()), Times.Never);
        }

        [Test]
        public async Task GetCurrentDayEvents_ReturnedEventsNotNull_GetAllCalledOnce()
        {
            //Arrange
            CalendarEventLogic calendarLogic = new(this.eventRepo.Object, this.mapper, this.userManager.Object);

            //Act
            var result = await calendarLogic.GetCurrentDayEvents(users[0].UserName);

            //Assert
            Assert.That(result != null);
            Assert.AreEqual(1, result.Count);

            this.eventRepo.Verify(x => x.GetAll(), Times.Once);
            this.userManager.Verify(x => x.Users, Times.Once);
            this.userManager.Verify(x => x.IsInRoleAsync(It.IsAny<WmanUser>(), It.IsAny<string>()), Times.Once);
        }

        [Test]
        public async Task GetCurrentWeekEvents_ReturnedEventsNotNull_GetAllCalledOnce()
        {
            //Arrange
            CalendarEventLogic calendarLogic = new(this.eventRepo.Object, this.mapper, this.userManager.Object);

            //Act
            var result = await calendarLogic.GetCurrentWeekEvents(users[0].UserName);

            //Assert
            Assert.That(result != null);
            Assert.True(result.Count >= 1 && result.Count <= 2); //Could be 1 or 2 because one of the test cases has -1 days on it and if we were to test it on Monday it would be different than on Friday
            
            this.eventRepo.Verify(x => x.GetAll(), Times.Once);
            this.userManager.Verify(x => x.Users, Times.Once);
            this.userManager.Verify(x => x.IsInRoleAsync(It.IsAny<WmanUser>(), It.IsAny<string>()), Times.Once);
        }

        [Test]
        public async Task GetDayEvents_ReturnsOneEvent_GetAllCalledOnce()
        {
            //Arrange
            CalendarEventLogic calendarLogic = new(this.eventRepo.Object, this.mapper, this.userManager.Object);
            DateTime dateTime = new(2021, 10, 10);

            //Act
            var resultInt = await calendarLogic.GetDayEvents(dateTime.DayOfYear, users[0].UserName);
            var resultDateTime = await calendarLogic.GetDayEvents(dateTime, users[0].UserName);

            //Assert
            Assert.That(resultInt is not null && resultDateTime is not null);
            Assert.True(resultInt.Count == 1 && resultDateTime.Count == 1);

            this.eventRepo.Verify(x => x.GetAll(), Times.Exactly(2));
            this.userManager.Verify(x => x.Users, Times.Exactly(2));
            this.userManager.Verify(x => x.IsInRoleAsync(It.IsAny<WmanUser>(), It.IsAny<string>()), Times.Exactly(2));
        }

        [Test]
        public async Task GetWeekEvents_ReturnsOneEvent_GetAllCalledOnce()
        {
            //Arrange
            CalendarEventLogic calendarLogic = new(this.eventRepo.Object, this.mapper, this.userManager.Object);

            DateTime firstDayOfWeek = new(2021, 10, 4); // first day of the week
            DateTime lastDayOfTheWeek = new(2021, 10, 10); // last day of the week

            int week = CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(lastDayOfTheWeek, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);

            //Act
            var resultInt = await calendarLogic.GetWeekEvents(week, users[0].UserName);
            var resultDateTime = await calendarLogic.GetWeekEvents(firstDayOfWeek, lastDayOfTheWeek, users[0].UserName);

            //Assert
            Assert.That(!(resultInt is null) && !(resultDateTime is null));
            Assert.True(resultInt.Count == 1 && resultDateTime.Count == 1);

            this.eventRepo.Verify(x => x.GetAll(), Times.Exactly(2));
            this.userManager.Verify(x => x.Users, Times.Exactly(2));
            this.userManager.Verify(x => x.IsInRoleAsync(It.IsAny<WmanUser>(),It.IsAny<string>()), Times.Exactly(2));
        }
    }
}
