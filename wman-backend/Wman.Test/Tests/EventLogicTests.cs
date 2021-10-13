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
            eventList = EventLogicBuilder.GetWorkEvents();
            addressList = EventLogicBuilder.GetAddresses();
            mapper = EventLogicBuilder.GetMapper();

        }
    }
}
