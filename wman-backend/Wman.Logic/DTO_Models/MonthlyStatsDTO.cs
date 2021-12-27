using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wman.Logic.DTO_Models
{
    public class MonthlyStatsDTO
    {
        public int UserID { get; set; }
        public string Username { get; set; }
        public string ProfilePicUrl { get; set; }


        public WorkloadWithHoursDTO January { get; set; }
        public WorkloadWithHoursDTO February { get; set; }
        public WorkloadWithHoursDTO March { get; set; }
        public WorkloadWithHoursDTO April { get; set; }
        public WorkloadWithHoursDTO May { get; set; }
        public WorkloadWithHoursDTO June { get; set; }
        public WorkloadWithHoursDTO July { get; set; }
        public WorkloadWithHoursDTO August { get; set; }
        public WorkloadWithHoursDTO September { get; set; }
        public WorkloadWithHoursDTO October { get; set; }
        public WorkloadWithHoursDTO November { get; set; }
        public WorkloadWithHoursDTO December { get; set; }
    }
    public class WorkloadWithHoursDTO
    {
        public int Hours { get; set; }
        public int WorkloadPercent { get; set; }
    }
}
