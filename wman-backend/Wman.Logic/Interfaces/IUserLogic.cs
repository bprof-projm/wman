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
        Task<WorkEventForWorkCardDTO> GetEventDetailsForWorker(string username, int id);
        Task<MonthlyStatsDTO> GetMonthlyStats(string username, DateTime year);
    }
}
