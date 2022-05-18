﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wman.Logic.DTO_Models
{
    public class StatsXlsModel
    {
        public string WorkerName { get; set; }
        public string JobDesc { get; set; }
        public string JobLocation { get; set; }
        public DateTime JobStart { get; set; }
        public DateTime JobEnd { get; set; }
        public int WorkHours { get; set; }
        public string PicUrl { get; set; }

    }
}
