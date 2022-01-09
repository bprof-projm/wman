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
        Task<List<WorkEventForWorkCardDTO>> GetCurrentWeekEvents(string username);
        Task<List<WorkEventForWorkCardDTO>> GetCurrentDayEvents(string username);
        Task<List<WorkEventForWorkCardDTO>> GetWeekEvents(int year, int week, string username);
        Task<List<WorkEventForWorkCardDTO>> GetDayEvents(int day, string username);
        Task<List<WorkEventForWorkCardDTO>> GetDayEvents(DateTime day, string username);
        Task<List<WorkEventForWorkCardDTO>> GetWeekEvents(DateTime firstDayOfTheWeek, DateTime lastDayOfTheWeek, string username);

    }
}
