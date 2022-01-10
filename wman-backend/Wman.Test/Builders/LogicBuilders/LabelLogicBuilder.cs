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
                Color = "#0000FF",
                Content = "something"
            });

            labelList.Add(new Label
            {
                Id = 1,
                Color = "#FF0000",
                Content = "dunno"
            });

            labelList.Add(new Label
            {
                Id = 2,
                Color = "#FFFF00",
                Content = "somethingelse"
            });

            labelList.Add(new Label
            {
                Id = 3,
                Color = "#00FF00",
                Content = "fesh"
            });

            labelList.Add(new Label
            {
                Id = 4,
                Color = "#FFA500",
                Content = "bruv"
            });

            return labelList;
        }
    }
}
