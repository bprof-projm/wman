﻿using AutoMapper;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Wman.Data.DB_Models;
using Wman.Logic.Classes;
using Wman.Logic.DTO_Models;
using Wman.Repository.Interfaces;
using Wman.Test.Builders;

namespace Wman.Test.Tests
{
    public class EventLogicTests
    {
        private Mock<IWorkEventRepo> eventRepo;
        private Mock<IAddressRepo> addressRepo;

        private IMapper mapper;

        private List<WorkEvent> eventList;
        private List<AddressHUN> addressList;

        [SetUp]
        public void SetUp()
        {
            this.eventList = EventLogicBuilder.GetWorkEvents();
            this.addressList = EventLogicBuilder.GetAddresses();
            this.mapper = EventLogicBuilder.GetMapper();

            this.eventRepo = EventLogicBuilder.GetEventRepo(eventList);
            this.addressRepo = EventLogicBuilder.GetAddressRepo(addressList);
        }

        [Test]
        public async Task UpdateEvent_UpdateExisitingEvent_SuccessfulOperation()
        {
            //Arrange
            EventLogic eventLogic = new EventLogic(eventRepo.Object, mapper, addressRepo.Object);

            WorkEvent workEvent = new WorkEvent
            {
                JobDescription = "MocskosLucsok",
                EstimatedStartDate = new DateTime(2021, 10, 16),
                EstimatedFinishDate = new DateTime(2021, 10, 16),
                AddressId = 1,
                WorkStartDate = new DateTime(2021, 10, 16),
                WorkFinishDate = new DateTime(2021, 10, 16),
                Status = Status.started
            };

            //Act
            var result = eventLogic.UpdateEvent(eventList[0].Id, workEvent);

            //Assert
            this.eventRepo.Verify(x => x.Update(It.IsAny<int>(), It.IsAny<WorkEvent>()), Times.Once);
        }

        [Test]
        public async Task DeleteEvent_SuccessfullyDeletesElement()
        {
            //Arrange
            EventLogic eventLogic = new EventLogic(eventRepo.Object, mapper, addressRepo.Object);

            //Act
            var result = eventLogic.DeleteEvent(eventList[0].Id);

            //Assert
            this.eventRepo.Verify(x => x.Delete(It.IsAny<int>()), Times.Once);
        }

        [Test]
        public async Task GetAllEvents_ReturnsRepoCorrectly()
        {
            //Arrange
            EventLogic eventLogic = new EventLogic(eventRepo.Object, mapper, addressRepo.Object);

            //Act
            var result = eventLogic.GetAllEvents();

            //Assert
            Assert.That(result.Count() == eventList.Count());
            this.eventRepo.Verify(x => x.GetAll(), Times.Once);
        }

        [Test]
        public async Task GetEvent_ReturnsWorkEvent_CompareToEventInList_Successful()
        {
            //Arrange
            EventLogic eventLogic = new EventLogic(eventRepo.Object, mapper, addressRepo.Object);

            //Act
            var result = await eventLogic.GetEvent(eventList[0].Id);

            //Assert
            Assert.That(result.Id == eventList[0].Id);
            this.eventRepo.Verify(x => x.GetOne(It.IsAny<int>()), Times.Once);
        }

        [Test]
        public async Task CreateEvent_SuccessfulCreation()
        {
            //Arrange
            EventLogic eventLogic = new EventLogic(eventRepo.Object, mapper, addressRepo.Object);

            var helper = new AddressHUNDTO()
            {
                City = addressList[0].City,
                BuildingNumber = addressList[0].BuildingNumber,
                FloorDoor = addressList[0].Floordoor,
                Street = addressList[0].Street,
                ZIPCode = addressList[0].ZIPCode,
            };

            CreateEventDTO eventDTO = new CreateEventDTO()
            {
                JobDescription = "ValmiTestXDLOLNI",
                EstimatedStartDate = new DateTime(2021,10,12,8,10,15),
                EstimatedFinishDate = new DateTime(2021,10,12,9,0,0),
                Address = helper,
                Status = Status.awaiting
            };

            //Act
            await eventLogic.CreateEvent(eventDTO);

            //Assert
            this.eventRepo.Verify(x => x.Add(It.IsAny<WorkEvent>()), Times.Once);
        }
    }
}