using AutoMapper;
using MockQueryable.Moq;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wman.Data.DB_Models;
using Wman.Logic.DTO_Models;
using Wman.Logic.Helpers;
using Wman.Repository.Interfaces;

namespace Wman.Test.Builders
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

            return eventRepo;
        }

        public static IMapper GetMapper()
        {
            var mapperconf = new MapperConfiguration(x => { x.AddProfile(new AutoMapperProfiles()); });

            IMapper mapper = mapperconf.CreateMapper();
            
            return mapper;
        }
        public static List<AddressHUN> GetAddresses()
        {
            List<AddressHUN> eventList = new List<AddressHUN>();

            eventList.Add(new AddressHUN
            {
                Id = 1,
                City = "Anyadba",
                Street = "Banya utca",
                ZIPCode = "2320",
                BuildingNumber = "45",
                Floordoor = "0"
            });

            eventList.Add(new AddressHUN
            {
                Id = 2,
                City = "Feshestetthely",
                Street = "asd utca",
                ZIPCode = "2201",
                BuildingNumber = "23",
                Floordoor = "0"
            });

            eventList.Add(new AddressHUN
            {
                Id = 3,
                City = "Feshestetthely",
                Street = "asd utca",
                ZIPCode = "2201",
                BuildingNumber = "40",
                Floordoor = "0"
            });

            return eventList;
        }

        public static List<WorkEvent> GetWorkEvents()
        {
            List<WorkEvent> eventList = new List<WorkEvent>();

            eventList.Add(new WorkEvent
            {
                JobDescription = "PizzaDobálóKretén",
                EstimatedStartDate = new DateTime(2021, 10, 16),
                EstimatedFinishDate = new DateTime(2021, 10, 16),
                AddressId = 1,
                WorkStartDate = new DateTime(2021, 10, 16),
                WorkFinishDate= new DateTime(2021, 10, 16),
                Status = Status.started
            });

            eventList.Add(new WorkEvent
            {
                JobDescription = "NoggerFagyi",
                EstimatedStartDate = new DateTime(2021, 10, 12),
                EstimatedFinishDate = new DateTime(2021, 10, 14),
                AddressId = 2,
                WorkStartDate = new DateTime(2021, 10, 12),
                WorkFinishDate = new DateTime(2021, 10, 14),
                Status = Status.started
            });


            eventList.Add(new WorkEvent
            {
                JobDescription = "Allahmashhallah",
                EstimatedStartDate = new DateTime(2021, 10, 15),
                EstimatedFinishDate = new DateTime(2021, 10, 18),
                AddressId = 3,
                WorkStartDate = new DateTime(2021, 10, 15),
                WorkFinishDate = new DateTime(2021, 10, 18),
                Status = Status.awaiting
            });

            eventList.Add(new WorkEvent
            {
                JobDescription = "Boombliallahkutarvashmir",
                EstimatedStartDate = new DateTime(2021, 10, 10),
                EstimatedFinishDate = new DateTime(2021, 10, 12),
                AddressId = 3,
                WorkStartDate = new DateTime(2021, 10, 10),
                WorkFinishDate = new DateTime(2021, 10, 12),
                Status = Status.finished
            });

            return eventList;
        }
    }
}
