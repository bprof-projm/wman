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
        UserManager<WmanUser> userManager;
        IEventLogic eventLogic;


        public ManagerLogic(UserManager<WmanUser> userManager, IEventLogic eventLogic)
        {
            this.userManager = userManager;
            this.eventLogic = eventLogic;
        }

        public async Task<ICollection<ManagerXlsModel>> getStats()
        {
            var allCompleted = await eventLogic.GetAllCompleted();
            var output = new List<ManagerXlsModel>();

            foreach (var job in allCompleted)
            {
                foreach (var person in job.AssignedUsers)
                {
                    output.Add(new ManagerXlsModel
                    {
                        JobDesc = job.JobDescription,
                        JobLocation = this.address2string(job.Address),
                        JobStart = job.WorkStartDate,
                        JobEnd = job.WorkFinishDate,
                        WorkerName = person.LastName + " " + person.FirstName

                    });
                }
            }
            return output;
        }
        private string address2string(AddressHUN item) //TODO: Maybe override tostring, check if it breaks something
        {
           string output = item.ZIPCode + " " +
                   item.City + ", " +
                   item.Street + " " +
                   item.BuildingNumber;
            if (!String.IsNullOrWhiteSpace(item.BuildingNumber))
            {
                output += " (" + item.BuildingNumber + ")";
            }
            return output;
        }
    }
}
