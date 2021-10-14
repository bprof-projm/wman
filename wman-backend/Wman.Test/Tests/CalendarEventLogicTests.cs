using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Wman.Data.DB_Models;
using Wman.Logic.Classes;
using Wman.Repository.Interfaces;
using Wman.Test.Builders;

namespace Wman.Test.Tests
{
    public class CalendarEventLogicTests
    {
        private Mock<IWorkEventRepo> eventRepo;

        private List<WorkEvent> eventList;

        [SetUp]
        public void SetUp()
        {
            this.eventList = CalendarEventLogicBuilder.GetWorkEvents();

            this.eventRepo = CalendarEventLogicBuilder.GetEventRepo(eventList);
        }

        [Test]
        public async Task GetCurrentDayEvents_ReturnedEventsNotNull_GetAllCalledOnce()
        {
            //Arrange
            CalendarEventLogic calendarLogic = new CalendarEventLogic(eventRepo.Object);

            //Act
            var result = await calendarLogic.GetCurrentDayEvents();

            //Assert
            Assert.That(result != null);
            Assert.AreEqual(1, result.Count);
            this.eventRepo.Verify(x => x.GetAll(), Times.Once);
        }

        [Test]
        public async Task GetCurrentWeekEvents_ReturnedEventsNotNull_GetAllCalledOnce()
        {
            //Arrange
            CalendarEventLogic calendarLogic = new CalendarEventLogic(eventRepo.Object);

            //Act
            var result = await calendarLogic.GetCurrentWeekEvents();

            //Assert
            Assert.That(result != null);
            Assert.True(result.Count >= 1 && result.Count <= 2); //Could be 1 or 2 because one the test cases has -1 days on it and if we were to test it on Monday it would be different than on Friday
            this.eventRepo.Verify(x => x.GetAll(), Times.Once);
        }

        [Test]
        public async Task GetDayEvents_ReturnsOneEvent_GetAllCalledOnce()
        {
            //Arrange
            CalendarEventLogic calendarLogic = new CalendarEventLogic(eventRepo.Object);
            DateTime dateTime = new DateTime(2021, 10, 10);

            //Act
            var resultInt = await calendarLogic.GetDayEvents(dateTime.DayOfYear);
            var resultDateTime = await calendarLogic.GetDayEvents(dateTime);


            //Assert
            Assert.That(resultInt is not null && resultDateTime is not null);
            Assert.True(resultInt.Count == 1 && resultDateTime.Count == 1); 
            this.eventRepo.Verify(x => x.GetAll(), Times.Exactly(2));
        }
    }
}
