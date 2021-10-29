using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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
    public class UserLogic : IUserLogic
    {
        UserManager<WmanUser> userManager;

        public UserLogic(UserManager<WmanUser> userManager)
        {
            this.userManager = userManager;
        }
        public async Task<IEnumerable<WorkloadDTO>> getWorkLoads(IEnumerable<string> usernames)
        {
            var output = new List<WorkloadDTO>();
            var allUsers = userManager.Users
                .Include(y => y.WorkEvents)
                .AsNoTracking();
            WmanUser selectedUser;
            foreach (var username in usernames)
            {
                selectedUser = allUsers.Where(x => x.UserName == username).SingleOrDefault();
                if (selectedUser == null)
                {
                    throw new ArgumentException(String.Format("User: {0} doesn't exists!", username));
                }
                output.Add(new WorkloadDTO
                {
                    Username = username,
                    Percent = Convert.ToInt32(calculateLoad(selectedUser)),
                    ProfilePic = selectedUser.ProfilePicture
                });
            }

            return output;
        }

        public async Task<IEnumerable<WorkloadDTO>> getWorkLoads()
        {
            var output = new List<WorkloadDTO>();
            var allUsers = userManager.Users
                .Include(y => y.WorkEvents)
                .AsNoTracking();
            foreach (var user in allUsers)
            {
                if (await userManager.IsInRoleAsync(user, "Worker"))
                {
                    //TODO: Move output.add inside this when role management is working.
                }
                output.Add(new WorkloadDTO
                {
                    Username = user.UserName,
                    Percent = Convert.ToInt32(calculateLoad(user)),
                    ProfilePic = user.ProfilePicture
                });
            }

            return output;
        }

        private double calculateLoad(WmanUser user)
        {
            var beforeToday = WorksBeforeToday(user.WorkEvents);
            var fromToday = RemainingWorks(user.WorkEvents);

            TimeSpan tsBeforeToday = TimeSpan.Zero;
            TimeSpan tsFromToday = TimeSpan.Zero;

            foreach (var item in beforeToday)
            {
                tsBeforeToday += (item.WorkFinishDate - item.WorkStartDate);
            }
            foreach (var item in fromToday)
            {
                tsFromToday += (item.EstimatedFinishDate - item.EstimatedStartDate);
            }

            TimeSpan tsSUM = tsBeforeToday + tsFromToday;
            ;
            return (tsSUM.TotalHours / 168) * 100;
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
