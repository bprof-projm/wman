using MockQueryable.Moq;
using Moq;
using System.Collections.Generic;
using System.Linq;
using Wman.Data.DB_Models;
using Wman.Repository.Interfaces;

namespace Wman.Test.Builders.LogicBuilders
{
    public class LabelLogicBuilder
    {
        public static Mock<ILabelRepo> GetLabelRepo(List<Label> labelList)
        {
            var labelRepo = new Mock<ILabelRepo>();
            var mock = labelList.AsQueryable().BuildMock();

            labelRepo.Setup(x => x.GetAll()).Returns(mock.Object);
            labelRepo.Setup(x => x.GetOne(It.IsAny<int>())).ReturnsAsync(labelList[0]);

            return labelRepo;
        }

        public static List<Label> GetLabels()
        {
            List<Label> labelList = new();

            labelList.Add( new Label{
                Id = 0,
                Color = "blue",
                Content = "something"
            });

            return labelList;
        }
    }
}
