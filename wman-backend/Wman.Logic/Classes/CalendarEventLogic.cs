using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wman.Data.DB_Models;
using Wman.Logic.DTO_Models;
using Wman.Logic.Helpers;
using Wman.Logic.Interfaces;
using Wman.Repository.Interfaces;

namespace Wman.Logic.Classes
{
    public class CalendarEventLogic : ICalendarEventLogic
    {
        private IWorkEventRepo workEventRepo;
        private IMapper mapper;
        UserManager<WmanUser> userManager;

        public CalendarEventLogic(IWorkEventRepo workEventRepo, IMapper mapper, UserManager<WmanUser> userManager)
        {
            this.workEventRepo = workEventRepo;
            this.mapper = mapper;
            this.userManager = userManager;
        }

        public async Task<List<WorkEventForWorkCardDTO>> GetCurrentDayEvents(string username)
        {
            var user = await userManager.Users
                .Where(x => x.UserName == username)
                .SingleOrDefaultAsync();
            if (user == null)
            {
                throw new NotFoundException(WmanError.UserNotFound);
            }
            List<WorkEvent> events = await (from x in workEventRepo.GetAll()
                                            where x.EstimatedStartDate.DayOfYear == DateTime.Now.DayOfYear && x.EstimatedStartDate.Year == DateTime.Today.Year
                                            select x).ToListAsync();

            if (await userManager.IsInRoleAsync(user, "worker"))
            {
                events = events.Where(x => x.AssignedUsers.Any(x => x.UserName == username)).ToList();
            }

            return mapper.Map<List<WorkEventForWorkCardDTO>>(events);
        }

        public async Task<List<WorkEventForWorkCardDTO>> GetCurrentWeekEvents(string username)
        {
            var user = await userManager.Users
                .Where(x => x.UserName == username)
                .SingleOrDefaultAsync();
            if (user == null)
            {
                throw new NotFoundException(WmanError.UserNotFound);
            }
            DateTime firstDayOfTheWeek = new DateTime();
            DateTime lastDayOfTheWeek = new DateTime();

            DateTime time = DateTime.Today;
            DayOfWeek day = CultureInfo.InvariantCulture.Calendar.GetDayOfWeek(time);

            switch (day)
            {
                case DayOfWeek.Sunday:
                    lastDayOfTheWeek = time;
                    firstDayOfTheWeek = time - TimeSpan.FromDays(6);
                    break;
                case DayOfWeek.Monday:
                    firstDayOfTheWeek = time;
                    lastDayOfTheWeek = time + TimeSpan.FromDays(6);
                    break;
                case DayOfWeek.Tuesday:
                    firstDayOfTheWeek = time - TimeSpan.FromDays(1);
                    lastDayOfTheWeek = time + TimeSpan.FromDays(5);
                    break;
                case DayOfWeek.Wednesday:
                    firstDayOfTheWeek = time - TimeSpan.FromDays(2);
                    lastDayOfTheWeek = time + TimeSpan.FromDays(4);
                    break;
                case DayOfWeek.Thursday:
                    firstDayOfTheWeek = time - TimeSpan.FromDays(3);
                    lastDayOfTheWeek = time + TimeSpan.FromDays(3);
                    break;
                case DayOfWeek.Friday:
                    firstDayOfTheWeek = time - TimeSpan.FromDays(4);
                    lastDayOfTheWeek = time + TimeSpan.FromDays(2);
                    break;
                case DayOfWeek.Saturday:
                    firstDayOfTheWeek = time - TimeSpan.FromDays(5);
                    lastDayOfTheWeek = time + TimeSpan.FromDays(1);
                    break;
                default:
                    break;
            }


            var events = await(from x in workEventRepo.GetAll()
                          where x.EstimatedStartDate.DayOfYear >= firstDayOfTheWeek.DayOfYear && x.EstimatedStartDate.DayOfYear <=lastDayOfTheWeek.DayOfYear && x.EstimatedStartDate.Year == DateTime.Today.Year
                               select x).ToListAsync();
            if (await userManager.IsInRoleAsync(user, "worker"))
            {
                events = events.Where(x => x.AssignedUsers.Any(x => x.UserName == username)).ToList();
            }
            return mapper.Map<List<WorkEventForWorkCardDTO>>(events);

        }

        public async Task<List<WorkEventForWorkCardDTO>> GetDayEvents(int day, string username)
        {
            var user = await userManager.Users
                .Where(x => x.UserName == username)
                .SingleOrDefaultAsync();
            if (user == null)
            {
                throw new NotFoundException(WmanError.UserNotFound);
            }
            if (day > 0 && day < 367)
            {
                var events = await (from x in workEventRepo.GetAll()
                                    where x.EstimatedStartDate.DayOfYear == day && x.EstimatedStartDate.Year == DateTime.Today.Year
                                    select x).ToListAsync();
                if (await userManager.IsInRoleAsync(user, "worker"))
                {
                    events = events.Where(x => x.AssignedUsers.Any(x => x.UserName == username)).ToList();
                }
                return mapper.Map<List<WorkEventForWorkCardDTO>>(events);
            }
            else
            {
                throw new ArgumentException(WmanError.InvalidInputRange);
            }
            
        }

        public async Task<List<WorkEventForWorkCardDTO>> GetWeekEvents(int week, string username)
        {
            var user = await userManager.Users
                .Where(x => x.UserName == username)
                .SingleOrDefaultAsync();
            if (user == null)
            {
                throw new NotFoundException(WmanError.UserNotFound);
            }
            if (week > 0 && week < 54)
            {
                var find = workEventRepo.GetAll().ToList().Where(x =>
                {
                    DateTime time = x.EstimatedStartDate;
                    DayOfWeek day = CultureInfo.InvariantCulture.Calendar.GetDayOfWeek(time);
                    if (day >= DayOfWeek.Monday && day <= DayOfWeek.Wednesday)
                    {
                        time = time.AddDays(3);
                    }
                    return week == CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(time, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday) && time.Year == DateTime.Today.Year;

                });
                if (await userManager.IsInRoleAsync(user, "worker"))
                {
                    find = find.Where(x => x.AssignedUsers.Any(x => x.UserName == username)).ToList();
                }
                return mapper.Map<List<WorkEventForWorkCardDTO>>(find);
            }
            else
            {
                throw new ArgumentException(WmanError.InvalidInputRange);
            }
            
        }
        public async Task<List<WorkEventForWorkCardDTO>> GetDayEvents(DateTime day, string username)
        {
            var user = await userManager.Users
                .Where(x => x.UserName == username)
                .SingleOrDefaultAsync();
            if (user == null)
            {
                throw new NotFoundException(WmanError.UserNotFound);
            }
            var events = await (from x in workEventRepo.GetAll()
                          where x.EstimatedStartDate.DayOfYear == day.DayOfYear && x.EstimatedStartDate.Year == DateTime.Today.Year
                                select x).ToListAsync();
            if (await userManager.IsInRoleAsync(user, "worker"))
            {
                events = events.Where(x => x.AssignedUsers.Any(x => x.UserName == username)).ToList();
            }
            return mapper.Map<List<WorkEventForWorkCardDTO>>(events);
        }

        public async Task<List<WorkEventForWorkCardDTO>> GetWeekEvents(DateTime firstDayOfTheWeek, DateTime lastDayOfTheWeek, string username)
        {
            var user = await userManager.Users
                .Where(x => x.UserName == username)
                .SingleOrDefaultAsync();
            if (user == null)
            {
                throw new NotFoundException(WmanError.UserNotFound);
            }
            var events =await (from x in workEventRepo.GetAll()
                          where x.EstimatedStartDate.DayOfYear >= firstDayOfTheWeek.DayOfYear && x.EstimatedStartDate.DayOfYear <= lastDayOfTheWeek.DayOfYear && x.EstimatedStartDate.Year == DateTime.Today.Year
                               select x).ToListAsync();
            if (await userManager.IsInRoleAsync(user, "worker"))
            {
                events = events.Where(x => x.AssignedUsers.Any(x => x.UserName == username)).ToList();
            }
            return mapper.Map<List<WorkEventForWorkCardDTO>>(events);
        }

        
    }
}
