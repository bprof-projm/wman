using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wman.Data.DB_Models;

namespace Wman.Data.DB_Connection_Tables
{
    public class WorkEventLabel
    {
        public int WorkEventId { get; set; }
        public WorkEvent WorkEvent { get; set; }
        public int LabelId { get; set; }
        public Label Label { get; set; }
    }
}
