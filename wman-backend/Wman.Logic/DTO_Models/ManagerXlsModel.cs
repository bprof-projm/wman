using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wman.Logic.DTO_Models
{
    public class ManagerXlsModel
    {
        public string WorkerName { get; set; }
        public string JobDesc { get; set; }
        public AddressHUNDTO JobLocation { get; set; }
        public DateTime JobStart { get; set; }
        public DateTime JobEnd { get; set; }

    }
}
