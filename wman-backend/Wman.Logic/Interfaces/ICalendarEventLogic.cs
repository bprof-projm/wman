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
        Task<List<WorkEventForWorkCardDTO>> GetCurrentWeekEvents();
        Task<List<WorkEventForWorkCardDTO>> GetCurrentDayEvents();
        List<WorkEventForWorkCardDTO> GetWeekEvents(int week);
        Task<List<WorkEventForWorkCardDTO>> GetDayEvents(int day);
        Task<List<WorkEventForWorkCardDTO>> GetDayEvents(DateTime day);
        Task<List<WorkEventForWorkCardDTO>> GetWeekEvents(DateTime firstDayOfTheWeek, DateTime lastDayOfTheWeek);
        
    }
}
