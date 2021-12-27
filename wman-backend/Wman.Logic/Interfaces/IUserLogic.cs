using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Wman.Logic.DTO_Models;

namespace Wman.Logic.Interfaces
{
    public interface IUserLogic
    {
        public Task<IEnumerable<WorkloadDTO>> GetWorkLoads(IEnumerable<string> usernames);
        public Task<IEnumerable<WorkloadDTO>> GetWorkLoads();

        Task<IEnumerable<AssignedEventDTO>> WorkEventsOfUser(string username);
        Task<IEnumerable<WorkEventForWorkCardDTO>> WorkEventsOfLoggedInUser(string username);
        Task<WorkEventForWorkCardDTO> GetEventDetailsForWorker(string username, int id);

        Task<IEnumerable<WorkEventForWorkCardDTO>> WorkEventsOfUserToday(string username);
        Task<IEnumerable<WorkEventForWorkCardDTO>> WorkEventsOfUserThisWeek(string username);
        Task<IEnumerable<WorkEventForWorkCardDTO>> WorkEventsOfUserSpecific(string username, DateTime start, DateTime finish);

        Task<MonthlyStatsDTO> GetMonthlyStats(string username, DateTime year);
    }
}
