using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wman.Data.DB_Models;

namespace Wman.Logic.Interfaces
{
    public interface ICalendarEventLogic
    {
        Task<List<WorkEvent>> GetCurrentWeekEvents();
        Task<List<WorkEvent>> GetCurrentDayEvents();
        List<WorkEvent> GetWeekEvents(int week);
        Task<List<WorkEvent>> GetDayEvents(int day);
        Task<List<WorkEvent>> GetDayEvents(DateTime day);
        Task<List<WorkEvent>> GetWeekEvents(DateTime firstDayOfTheWeek, DateTime lastDayOfTheWeek);
        
    }
}
