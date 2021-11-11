using AutoMapper;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Wman.Data.DB_Models;
using Wman.Logic.Classes;
using Wman.Repository.Interfaces;
using Wman.Test.Builders;
using Wman.Test.Builders.LogicBuilders;

namespace Wman.Test.Tests
{
    public class LabelLogicTests
    {
        private Mock<ILabelRepo> labelRepo;
        private Mock<IWorkEventRepo> eventRepo;
        private IMapper mapper;

        private List<Label> labelList;
        private List<WorkEvent> eventList;

        [SetUp]
        public void SetUp()
        {
            this.mapper = MapperBuilder.GetMapper();

            this.labelList = LabelLogicBuilder.GetLabels();
            this.labelRepo = LabelLogicBuilder.GetLabelRepo(this.labelList);

            this.eventList = EventLogicBuilder.GetWorkEvents();
            this.eventRepo = EventLogicBuilder.GetEventRepo(this.eventList);
        }

        [Test]
        public async Task GetAllLabels_ReturnsAllProperly_SuccessfulOperation()
        {
            //Arrange
            LabelLogic labelLogic = new LabelLogic(this.labelRepo.Object, this.mapper, this.eventRepo.Object);

            //Act
            var call = labelLogic.GetAllLabels();

            //Assert
            Assert.That(call.Count() == labelList.Count());
            this.labelRepo.Verify(x => x.GetAll(), Times.Once);
        }
    }
}
