using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wman.Data.DB_Models;
using Wman.Logic.DTO_Models;
using Wman.Logic.Interfaces;
using Wman.Repository.Interfaces;

namespace Wman.Logic.Classes
{
    public class CalendarEventLogic : ICalendarEventLogic
    {
        private IWorkEventRepo workEventRepo;
        public CalendarEventLogic(IWorkEventRepo workEventRepo)
        {
            this.workEventRepo = workEventRepo;
        }
        public async Task<List<CalendarWorkEventDTO>> GetCurrentDayEvents()
        {
            var events = await (from x in workEventRepo.GetAll()
                         where x.EstimatedStartDate.DayOfYear == DateTime.UtcNow.DayOfYear
                         select x).ToListAsync();
            var eventDTOs = Converter.CalendarWorkEventConverter(events);
            return eventDTOs.ToList();
        }

        public async Task<List<CalendarWorkEventDTO>> GetCurrentWeekEvents()
        {
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
                          where x.EstimatedStartDate.DayOfYear >= firstDayOfTheWeek.DayOfYear && x.EstimatedStartDate.DayOfYear <=lastDayOfTheWeek.DayOfYear
                          select x).ToListAsync();
            var eventDTOs = Converter.CalendarWorkEventConverter(events);
            return eventDTOs.ToList();
        }

        public async Task<List<CalendarWorkEventDTO>> GetDayEvents(int day)
        {
            if (day > 0 && day < 367)
            {
                var events = await (from x in workEventRepo.GetAll()
                                    where x.EstimatedStartDate.DayOfYear == day
                                    select x).ToListAsync();
                var eventDTOs = Converter.CalendarWorkEventConverter(events);
                return eventDTOs.ToList();
            }
            else
            {
                throw new ArgumentException("invalid parameter (it has to be over 0 and under 365)");
            }
            
        }

        public List<CalendarWorkEventDTO> GetWeekEvents(int week)
        {
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
                    return week == CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(time, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);

                });

                var eventDTOs = Converter.CalendarWorkEventConverter(find);
                return eventDTOs.ToList();
            }
            else
            {
                throw new ArgumentException("invalid parameter (it has to be over 0 and under 54)");
            }
            
        }
        public async Task<List<CalendarWorkEventDTO>> GetDayEvents(DateTime day)
        {

            var events = await (from x in workEventRepo.GetAll()
                          where x.EstimatedStartDate.DayOfYear == day.DayOfYear
                          select x).ToListAsync();
            var eventDTOs = Converter.CalendarWorkEventConverter(events);
            return eventDTOs.ToList();
        }

        public async Task<List<CalendarWorkEventDTO>> GetWeekEvents(DateTime firstDayOfTheWeek, DateTime lastDayOfTheWeek)
        {
            var events =await (from x in workEventRepo.GetAll()
                          where x.EstimatedStartDate.DayOfYear >= firstDayOfTheWeek.DayOfYear && x.EstimatedStartDate.DayOfYear <= lastDayOfTheWeek.DayOfYear
                          select x).ToListAsync();
            var eventDTOs = Converter.CalendarWorkEventConverter(events);
            return eventDTOs.ToList();
        }
    }
}
