using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wman.Data.DB_Models;
using Wman.Logic.DTO_Models;

namespace Wman.Logic.Interfaces
{
    public interface IStatsLogic
    {
        Task<ICollection<StatsXlsModel>> GetManagerStats(DateTime month);
        Task<ICollection<ICollection<StatsXlsModel>>> GetWorkerStats(DateTime input);
        Task SendManagerEmails(string username);
        void registerRecurringManagerJob(string x);
        void registerRecurringWorkerJob(string x);
    }
}
