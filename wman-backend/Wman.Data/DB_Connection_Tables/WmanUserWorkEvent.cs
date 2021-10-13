using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wman.Data.DB_Models;

namespace Wman.Data.DB_Connection_Tables
{
    public class WmanUserWorkEvent
    {
        public int WmanUserId { get; set; }
        public WmanUser WmanUser { get; set; }
        public int WorkEventId { get; set; }
        public WorkEvent WorkEvent { get; set; }
    }
}
