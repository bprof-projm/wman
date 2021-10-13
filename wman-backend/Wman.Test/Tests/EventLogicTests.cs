using AutoMapper;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using Wman.Data.DB_Models;
using Wman.Repository.Interfaces;

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

        }
    }
}
