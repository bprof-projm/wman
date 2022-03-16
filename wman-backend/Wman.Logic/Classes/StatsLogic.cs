using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wman.Logic.DTO_Models;
using Wman.Logic.Interfaces;

namespace Wman.Logic.Classes
{
    public class StatsLogic : IStatsLogic
    {
        IEventLogic eventLogic;

        public StatsLogic(IEventLogic eventLogic)
        {
            this.eventLogic = eventLogic;
        }

        public async Task<ICollection<StatsXlsModel>> GetStats(DateTime input)
        {
            var allCompleted = await eventLogic.GetAllCompleted();
            var thisMonth = allCompleted.Where(x => x.WorkFinishDate.Year == input.Year && x.WorkFinishDate.Month == input.Month);
            var output = new List<StatsXlsModel>();

            foreach (var job in thisMonth)
            {
                foreach (var person in job.AssignedUsers)
                {
                    output.Add(new StatsXlsModel
                    {
                        JobDesc = job.JobDescription,
                        JobLocation = job.Address.ToString(),
                        JobStart = job.WorkStartDate,
                        JobEnd = job.WorkFinishDate,
                        WorkerName = person.LastName + " " + person.FirstName

                    });
                }
            }
            return output;
        }
    }
}
