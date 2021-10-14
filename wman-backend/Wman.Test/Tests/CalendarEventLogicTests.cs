using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using Wman.Data.DB_Models;
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
            this.eventList = EventLogicBuilder.GetWorkEvents();

            this.eventRepo = EventLogicBuilder.GetEventRepo(eventList);
        }
    }
}
