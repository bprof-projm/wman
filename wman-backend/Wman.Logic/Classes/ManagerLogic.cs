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
            var allCompleted = await eventLogic.getAllCompleted();
            var output = new List<ManagerXlsModel>();

            foreach (var item in allCompleted)
            {
                output.Add(new ManagerXlsModel //TODO: Still wip
                {
                    JobDesc = item.JobDescription,
                    JobLocation = this.address2string(item.Address),
                    JobStart = item.WorkStartDate,
                    JobEnd = item.WorkFinishDate,
                    WorkerName = "debug"
                    
                });
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
