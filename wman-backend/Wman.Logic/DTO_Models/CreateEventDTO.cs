using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wman.Data.DB_Models;

namespace Wman.Logic.DTO_Models
{
    public class CreateEventDTO
    {
        public string JobDescription { get; set; }
        public DateTime EstimatedStartDate { get; set; }
        public DateTime EstimatedFinishDate { get; set; }
        public AddressHUNDTO Address { get; set; }
        public string Status { get; set; }
    }
}
