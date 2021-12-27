using MockQueryable.Moq;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using Wman.Data.DB_Models;
using Wman.Repository.Interfaces;

namespace Wman.Test.Builders.LogicBuilders
{
    public class EventLogicBuilder
    {
        public static Mock<IAddressRepo> GetAddressRepo(List<AddressHUN> addressList)
        {
            var addressRepo = new Mock<IAddressRepo>();
            var mock = addressList.AsQueryable().BuildMock();

            addressRepo.Setup(x => x.GetAll()).Returns(mock.Object);
            addressRepo.Setup(x => x.GetOne(It.IsAny<int>())).ReturnsAsync(addressList[0]);

            return addressRepo;
        }

        public static Mock<IWorkEventRepo> GetEventRepo(List<WorkEvent> eventList)
        {
            var eventRepo = new Mock<IWorkEventRepo>();
            var mock = eventList.AsQueryable().BuildMock();

            eventRepo.Setup(x => x.GetAll()).Returns(mock.Object);
            eventRepo.Setup(x => x.GetOne(It.IsAny<int>())).ReturnsAsync(eventList[0]);
            eventRepo.Setup(x => x.GetOneWithTracking(It.IsAny<int>())).ReturnsAsync(eventList[0]);

            return eventRepo;
        }

        public static List<AddressHUN> GetAddresses()
        {
            List<AddressHUN> addressList = new();

            addressList.Add(new AddressHUN
            {
                Id = 1,
                City = "Anyadba",
                Street = "Banya utca",
                ZIPCode = "2320",
                BuildingNumber = "45",
                Floordoor = "0"
            });

            addressList.Add(new AddressHUN
            {
                Id = 2,
                City = "Feshestetthely",
                Street = "asd utca",
                ZIPCode = "2201",
                BuildingNumber = "23",
                Floordoor = "0"
            });

            addressList.Add(new AddressHUN
            {
                Id = 3,
                City = "Feshestetthely",
                Street = "asd utca",
                ZIPCode = "2201",
                BuildingNumber = "40",
                Floordoor = "0"
            });

            return addressList;
        }

        public static List<WorkEvent> GetWorkEvents()
        {
            List<WorkEvent> eventList = new();

            eventList.Add(new WorkEvent
            {
                Id=0,
                JobDescription = "PizzaDobálóKretén",
                EstimatedStartDate = DateTime.UtcNow,
                EstimatedFinishDate = DateTime.UtcNow.AddMinutes(20),
                WorkStartDate = new DateTime(2021, 10, 16),
                WorkFinishDate = new DateTime(2021, 10, 16),
                Status = Status.started
            });

            eventList.Add(new WorkEvent
            {
                Id = 1,
                JobDescription = "NoggerFagyi",
                EstimatedStartDate = DateTime.UtcNow.AddDays(-1),
                EstimatedFinishDate = DateTime.UtcNow.AddDays(-1).AddMinutes(40),
                WorkStartDate = new DateTime(2021, 10, 12),
                WorkFinishDate = new DateTime(2021, 10, 14),
                Status = Status.started
            });

            eventList.Add(new WorkEvent
            {
                Id = 2,
                JobDescription = "Allahmashhallah",
                EstimatedStartDate = DateTime.UtcNow.AddDays(7),
                EstimatedFinishDate = DateTime.UtcNow.AddDays(7).AddMinutes(40),
                WorkStartDate = new DateTime(2021, 10, 15),
                WorkFinishDate = new DateTime(2021, 10, 18),
                Status = Status.awaiting
            });

            eventList.Add(new WorkEvent
            {
                Id = 3,
                JobDescription = "Boombliallahkutarvashmir",
                EstimatedStartDate = new DateTime(2021, 10, 10),
                EstimatedFinishDate = new DateTime(2021, 10, 12),
                WorkStartDate = new DateTime(2021, 10, 10),
                WorkFinishDate = new DateTime(2021, 10, 12),
                Status = Status.finished
            });

            return eventList;
        }
    }
}