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
        Task<ICollection<StatsXlsModel>> GetStats(DateTime month);
        Task SendEmails(string username);
        void registerRecurringJob(string x);
    }
}
