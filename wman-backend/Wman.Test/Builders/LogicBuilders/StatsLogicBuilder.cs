using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Wman.Data.DB_Models;
using Wman.Repository.Interfaces;

namespace Wman.Test.Builders.LogicBuilders
{
    public class StatsLogicBuilder
    {
        public static Mock<IFileRepo> GetFileRepo()
        {
            var fileRepo = new Mock<IFileRepo>();

            fileRepo.Setup(x => x.Create(It.IsAny<string>(), It.IsAny<Stream>())).Returns(Task.CompletedTask);
            fileRepo.Setup(x => x.GetDetails(It.IsAny<string>())).Returns(TaskDirectoryInfoHelper());
            fileRepo.Setup(x => x.DeleteOldFiles(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime>())).Returns(Task.CompletedTask);
            return fileRepo;
        }

        public static List<WorkEvent> GetWorkEventsWithFinishedStatus()
        {
            var eventList = EventLogicBuilder.GetWorkEvents();

            foreach (var item in eventList)
            {
                item.Status = Status.finished;
                item.WorkFinishDate = DateTime.Now.AddHours(-1);
            }

            return eventList;
        }

        private static Task<DirectoryInfo> TaskDirectoryInfoHelper()
        {
            var akarmi = new DirectoryInfo("Test_data");
            return Task.FromResult(akarmi);
        }
    }
}
