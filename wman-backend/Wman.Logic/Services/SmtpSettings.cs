using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wman.Logic.Services
{
    public class SmtpSettings
    {
        public string SmtpHost { get; set; }
        public int SmtpPort { get; set; }
        public string SmtpSecret { get; set; }
        public string SmtpAddress { get; set; }
    }
}
