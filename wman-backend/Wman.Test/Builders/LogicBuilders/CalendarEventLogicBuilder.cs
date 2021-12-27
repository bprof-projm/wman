using MockQueryable.Moq;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using Wman.Data.DB_Models;
using Wman.Repository.Interfaces;

namespace Wman.Test.Builders.LogicBuilders
{
    //This builder class implements similar functions to EventLogicBuilder,
    //but the required test cases are a bit different than the one needed for EventLogicTests, so this was a necessity
    public class CalendarEventLogicBuilder
    {
        public static Mock<IWorkEventRepo> GetEventRepo(List<WorkEvent> eventList)
        {
            var eventRepo = new Mock<IWorkEventRepo>();
            var mock = eventList.AsQueryable().BuildMock();

            eventRepo.Setup(x => x.GetAll()).Returns(mock.Object);
            eventRepo.Setup(x => x.GetOne(It.IsAny<int>())).ReturnsAsync(eventList[0]);

            return eventRepo;
        }

        public static List<WorkEvent> GetWorkEvents()
        {
            List<WorkEvent> eventList = new List<WorkEvent>();

            eventList.Add(new WorkEvent
            {
                JobDescription = "PizzaDobálóKretén",
                EstimatedStartDate = DateTime.UtcNow,
                EstimatedFinishDate = DateTime.UtcNow.AddMinutes(20),
                WorkStartDate = new DateTime(2021, 10, 16),
                WorkFinishDate = new DateTime(2021, 10, 16),
                Status = Status.started
            });

            eventList.Add(new WorkEvent
            {
                JobDescription = "NoggerFagyi",
                EstimatedStartDate = DateTime.UtcNow.AddDays(-1),
                EstimatedFinishDate = DateTime.UtcNow.AddDays(-1).AddMinutes(40),
                WorkStartDate = new DateTime(2021, 10, 12),
                WorkFinishDate = new DateTime(2021, 10, 14),
                Status = Status.started
            });

            eventList.Add(new WorkEvent
            {
                JobDescription = "Allahmashhallah",
                EstimatedStartDate = DateTime.UtcNow.AddDays(7),
                EstimatedFinishDate = DateTime.UtcNow.AddDays(7).AddMinutes(40),
                WorkStartDate = new DateTime(2021, 10, 15),
                WorkFinishDate = new DateTime(2021, 10, 18),
                Status = Status.awaiting
            });

            eventList.Add(new WorkEvent
            {
                JobDescription = "Boombliallahkutarvashmir",
                EstimatedStartDate = DateTime.UtcNow.AddDays(20),
                EstimatedFinishDate = DateTime.UtcNow.AddDays(20).AddMinutes(40),
                WorkStartDate = new DateTime(2021, 10, 10),
                WorkFinishDate = new DateTime(2021, 10, 12),
                Status = Status.finished
            });

            eventList.Add(new WorkEvent
            {
                JobDescription = "Boombliallahkutarvashmir",
                EstimatedStartDate = new DateTime(2021, 10, 10),
                EstimatedFinishDate = new DateTime(2021, 10, 10),
                WorkStartDate = new DateTime(2021, 10, 10),
                WorkFinishDate = new DateTime(2021, 10, 12),
                Status = Status.finished
            });

            return eventList;
        }
    }
}