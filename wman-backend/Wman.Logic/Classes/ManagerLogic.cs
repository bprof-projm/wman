using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wman.Data.DB_Models;
using Wman.Logic.DTO_Models;
using Wman.Logic.Interfaces;

namespace Wman.Logic.Classes
{
    public class ManagerLogic : IManagerLogic
    {
        IEventLogic eventLogic;

        public ManagerLogic(IEventLogic eventLogic)
        {
            this.eventLogic = eventLogic;
        }

        public async Task<ICollection<ManagerXlsModel>> GetStats(DateTime input)
        {
            var allCompleted = await eventLogic.GetAllCompleted();
            var thisMonth = allCompleted.Where(x => x.WorkFinishDate.Year == input.Year && x.WorkFinishDate.Month == input.Month);
            var output = new List<ManagerXlsModel>();

            foreach (var job in thisMonth)
            {
                foreach (var person in job.AssignedUsers)
                {
                    output.Add(new ManagerXlsModel
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
