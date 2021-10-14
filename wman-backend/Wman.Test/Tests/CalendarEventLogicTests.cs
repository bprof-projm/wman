using Moq;
using NUnit.Framework;
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
            this.eventRepo.Verify(x => x.GetAll(), Times.Once);
            Assert.True(result.Count > 0);
        }
    }
}
