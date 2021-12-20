using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Wman.Data.DB_Models;
using Wman.Logic.DTO_Models;
using Wman.Logic.Helpers;
using Wman.Logic.Interfaces;
using Wman.Repository.Interfaces;

namespace Wman.Logic.Classes
{
    public class UserLogic : IUserLogic
    {
        UserManager<WmanUser> userManager;
        IMapper mapper;
        IWorkEventRepo eventRepo;
        //EventRepo eventRepo;
        public UserLogic(UserManager<WmanUser> userManager, IMapper mapper, IWorkEventRepo eventRepo)
        {
            this.userManager = userManager;
            this.mapper = mapper;
            this.eventRepo = eventRepo;
        }
        public async Task<IEnumerable<WorkloadDTO>> GetWorkLoads(IEnumerable<string> usernames)
        {
            var output = new List<WorkloadDTO>();
            var allUsers = userManager.Users
                .Include(y => y.WorkEvents)
                .Include(z => z.ProfilePicture);
            WmanUser selectedUser;
            var profileUrl = string.Empty;
            foreach (var username in usernames)
            {
                selectedUser = allUsers.Where(x => x.UserName == username).SingleOrDefault();
                if (selectedUser == null)
                {
                    throw new NotFoundException(WmanError.UserNotFound);
                }
                if (await userManager.IsInRoleAsync(selectedUser, "Worker") == false)
                {
                    throw new NotMemberOfRoleException(WmanError.NotAWorker);
                }
                if (selectedUser.ProfilePicture != null)
                {
                    profileUrl = selectedUser.ProfilePicture.Url;
                }
                else
                {
                    profileUrl = string.Empty;
                }
                output.Add(new WorkloadDTO
                {
                    userID = selectedUser.Id,
                    Username = username,
                    Percent = Convert.ToInt32(CalculateLoad(selectedUser)),
                    ProfilePicUrl = profileUrl
                });
            }

            return output;
        }

        public async Task<IEnumerable<WorkloadDTO>> GetWorkLoads()
        {
            var output = new List<WorkloadDTO>();
            var allUsers = userManager.Users
                .Include(y => y.WorkEvents)
                .Include(z => z.ProfilePicture);
            var profileUrl = string.Empty;
            foreach (var user in allUsers)
            {
                if (await userManager.IsInRoleAsync(user, "Worker"))
                {
                    if (user.ProfilePicture != null)
                    {
                        profileUrl = user.ProfilePicture.Url;
                    }
                    else
                    {
                        profileUrl = string.Empty;
                    }
                    output.Add(new WorkloadDTO
                    {
                        userID = user.Id,
                        Username = user.UserName,
                        Percent = Convert.ToInt32(CalculateLoad(user)),
                        ProfilePicUrl = profileUrl
                    });
                }
                
            }

            return output;
        }

        public async Task<IEnumerable<AssignedEventDTO>> WorkEventsOfUser(string username) //Kept this as a legacy method, because it might be already used on FE with this older DTO. But this essentially does the same as this.WorkEventsOfLoggedInUser(), just a differently formatted output. Should probably be deleted, together with the endpoint referencing this. 
        {

            var events = await this.GetEventsOfUser(username);
            var mapped = mapper.Map<IEnumerable<AssignedEventDTO>>(events);

            return mapped;
        }

        public async Task<IEnumerable<WorkEventForWorkCardDTO>> WorkEventsOfLoggedInUser(string username)
        {
            var events = await this.GetEventsOfUser(username);
            var output = mapper.Map<IEnumerable<WorkEventForWorkCardDTO>>(events);
            return output;
        }

        public async Task<IEnumerable<WorkEventForWorkCardDTO>> WorkEventsOfUserToday(string username)
        {
            var allEventsAvail = await this.GetEventsOfUser(username);
            var selected = allEventsAvail.Where(x => x.EstimatedStartDate.DayOfYear == DateTime.Now.DayOfYear && x.EstimatedStartDate.Year == DateTime.Now.Year);

            var output = mapper.Map<IEnumerable<WorkEventForWorkCardDTO>>(selected);
            return output;
        }
        public async Task<IEnumerable<WorkEventForWorkCardDTO>> WorkEventsOfUserThisWeek(string username)
        {
            var allEventsAvail = await this.GetEventsOfUser(username);
            DayOfWeek day = CultureInfo.InvariantCulture.Calendar.GetDayOfWeek(DateTime.Now);
            DateTime StartDate = DateTime.MinValue;
            DateTime EndDate = DateTime.MinValue;
            StartDate = this.GetWeekStartDate(DateTime.Now);
        
            EndDate = StartDate.AddDays(6);
            var selected = allEventsAvail.Where(x => x.EstimatedStartDate.DayOfYear >= StartDate.DayOfYear && x.EstimatedStartDate.DayOfYear <= EndDate.DayOfYear && x.EstimatedStartDate.Year == StartDate.Year);

            var output = mapper.Map<IEnumerable<WorkEventForWorkCardDTO>>(selected);
            return output;
        }
        public async Task<IEnumerable<WorkEventForWorkCardDTO>> WorkEventsOfUserSpecific(string username, DateTime start, DateTime finish)
        {
            var allEventsAvail = await this.GetEventsOfUser(username);
            var selected = allEventsAvail.Where(x => x.EstimatedStartDate >= start && x.EstimatedStartDate <= finish);

            var output = mapper.Map<IEnumerable<WorkEventForWorkCardDTO>>(selected);
            return output;
        }
        public async Task<WorkEventForWorkCardDTO> GetEventDetailsForWorker(string username, int id)
        {
            var job = await eventRepo.GetOne(id);
            if (job == null)
            {
                throw new NotFoundException(WmanError.EventNotFound);
            }
            if (job.AssignedUsers.Where(x => x.UserName == username).SingleOrDefault() != null)
            {
                return mapper.Map<WorkEventForWorkCardDTO>(job);
            }
            throw new InvalidOperationException(WmanError.NotHisBusiness);
        }
        private async Task<IEnumerable<WorkEvent>> GetEventsOfUser(string username)
        {
            var selectedUser = await this.GetUser(username);
           var events = eventRepo.GetAll().Where(x => x.AssignedUsers.Contains(selectedUser));
            return events;
        }
        private async Task<WmanUser> GetUser(string username)
        {
            var selectedUser = await userManager.Users
                .Where(x => x.UserName == username)
                .SingleOrDefaultAsync();
            if (selectedUser == null)
            {
                throw new NotFoundException(WmanError.UserNotFound);
            }
            return selectedUser;
        }

        private double CalculateLoad(WmanUser user)
        {
            var selectedDate = DateTime.Now;
            var beforeToday = WorksBeforeToday(user.WorkEvents, selectedDate);
            var fromToday = RemainingWorks(user.WorkEvents, selectedDate);

            TimeSpan tsBeforeToday = TimeSpan.Zero;
            TimeSpan tsFromToday = TimeSpan.Zero;

            foreach (var item in beforeToday)
            {
                if (item.WorkFinishDate != DateTime.MinValue && item.WorkStartDate != DateTime.MinValue)
                {
                    tsBeforeToday += (item.WorkFinishDate - item.WorkStartDate);
                }
                else
                {
                    tsFromToday += (item.EstimatedFinishDate - item.EstimatedStartDate); //Work finish/start date is not valid, but it should already be. Assuming still in progress, and adding it to the remaining work pool
                }
            }
            foreach (var item in fromToday)
            {
                if (item.EstimatedFinishDate != DateTime.MinValue && item.EstimatedStartDate != DateTime.MinValue)
                {
                    tsFromToday += (item.EstimatedFinishDate - item.EstimatedStartDate);
                }
            }

            TimeSpan tsSUM = tsBeforeToday + tsFromToday;
            var hoursInMonth = DateTime.DaysInMonth(selectedDate.Year, selectedDate.Month) * 8;
            return (tsSUM.TotalHours / hoursInMonth) * 100;
        }
        private IEnumerable<WorkEvent> WorksBeforeToday(IEnumerable<WorkEvent> works, DateTime selectedMonth)
        {
            return works.Where(x => x.WorkStartDate.Day < selectedMonth.Day && x.WorkStartDate.Year == selectedMonth.Year && x.WorkStartDate.Month == selectedMonth.Month);
        }

        private IEnumerable<WorkEvent> RemainingWorks(IEnumerable<WorkEvent> works, DateTime selectedMonth)
        {
            return works.Where(x => x.EstimatedStartDate.Day >= selectedMonth.Day && x.EstimatedStartDate.Year == selectedMonth.Year && x.EstimatedStartDate.Month == selectedMonth.Month && x.WorkStartDate == DateTime.MinValue);
        }
        private DateTime GetWeekStartDate(DateTime input)
        {
            DateTime StartDate = DateTime.MinValue;
            DayOfWeek day = CultureInfo.InvariantCulture.Calendar.GetDayOfWeek(input);
            switch (day)
            {
                case DayOfWeek.Sunday:
                    StartDate = DateTime.Now.AddDays(-6);
                    break;
                case DayOfWeek.Monday:
                    StartDate = DateTime.Now;
                    break;
                case DayOfWeek.Tuesday:
                    StartDate = DateTime.Now.AddDays(-1);
                    break;
                case DayOfWeek.Wednesday:
                    StartDate = DateTime.Now.AddDays(-2);
                    break;
                case DayOfWeek.Thursday:
                    StartDate = DateTime.Now.AddDays(-3);
                    break;
                case DayOfWeek.Friday:
                    StartDate = DateTime.Now.AddDays(-4);
                    break;
                case DayOfWeek.Saturday:
                    StartDate = DateTime.Now.AddDays(-5);
                    break;
            }
            return StartDate;
        }
    }
}
