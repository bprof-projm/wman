using AutoMapper;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using Wman.Data.DB_Models;
using Wman.Repository.Interfaces;
using Wman.Test.Builders;

namespace Wman.Test.Tests
{
    public class EventLogicTests
    {
        private Mock<IWorkEventRepo> eventRepo;
        private Mock<IAddressRepo> addressRepo;
        private Mock<IMapper> mapper;

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
    }
}
