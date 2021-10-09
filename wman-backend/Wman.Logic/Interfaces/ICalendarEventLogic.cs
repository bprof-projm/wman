using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wman.Data.DB_Models;
using Wman.Logic.DTO_Models;

namespace Wman.Logic.Interfaces
{
    public interface ICalendarEventLogic
    {
        Task<List<CalendarWorkEventDTO>> GetCurrentWeekEvents();
        Task<List<CalendarWorkEventDTO>> GetCurrentDayEvents();
        List<CalendarWorkEventDTO> GetWeekEvents(int week);
        Task<List<CalendarWorkEventDTO>> GetDayEvents(int day);
        Task<List<CalendarWorkEventDTO>> GetDayEvents(DateTime day);
        Task<List<CalendarWorkEventDTO>> GetWeekEvents(DateTime firstDayOfTheWeek, DateTime lastDayOfTheWeek);
        
    }
}
