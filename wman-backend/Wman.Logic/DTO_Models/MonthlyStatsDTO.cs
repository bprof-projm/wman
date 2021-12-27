using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wman.Logic.DTO_Models
{
    public class MonthlyStatsDTO
    {
        public MonthlyStatsDTO()
        {
            this.MonthlyStats = new Dictionary<string, WorkloadWithHoursDTO>();
        }
        public int UserID { get; set; }
        public string Username { get; set; }
        public string ProfilePicUrl { get; set; }


        public Dictionary<string, WorkloadWithHoursDTO> MonthlyStats { get; set; }
    }
    public class WorkloadWithHoursDTO
    {
        public int Hours { get; set; }
        public int WorkloadPercent { get; set; }
    }
}
