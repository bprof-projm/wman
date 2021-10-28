using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wman.Data.DB_Models;
using Wman.Logic.Interfaces;

namespace Wman.Logic.Classes
{
    public class UserLogic : IUserLogic
    {
        UserManager<WmanUser> userManager;

        public UserLogic(UserManager<WmanUser> userManager)
        {
            this.userManager = userManager;
        }

        private int calculate(WmanUser user)
        {
            var beforeToday = WorksBeforeToday(user.WorkEvents);
            var fromToday = RemainingWorks(user.WorkEvents);
            //List<TimeSpan> tsBeforeToday = new List<TimeSpan>();
            //List<TimeSpan> tsFromToday = new List<TimeSpan>();

            TimeSpan tsBeforeToday = TimeSpan.Zero;
            TimeSpan tsFromToday = TimeSpan.Zero;
            
            foreach (var item in beforeToday)
            {
                tsFromToday += (item.WorkFinishDate - item.WorkStartDate);
            }
            foreach (var item in fromToday)
            {
                tsFromToday += (item.EstimatedFinishDate - item.EstimatedStartDate);
            }

            TimeSpan tsSUM = tsBeforeToday + tsFromToday;
            return tsSUM.Hours / 168;
        }
        private IEnumerable<WorkEvent> WorksBeforeToday(IEnumerable<WorkEvent> works)
        {
            return works.Where(x => x.WorkStartDate.DayOfYear < DateTime.Now.DayOfYear && x.WorkStartDate.Year == DateTime.Now.Year);
        }

        private IEnumerable<WorkEvent> RemainingWorks(IEnumerable<WorkEvent> works)
        {
            return works.Where(x => x.EstimatedStartDate.DayOfYear <= DateTime.Now.DayOfYear && x.EstimatedStartDate.Year == DateTime.Now.Year);
        }

    }
}
