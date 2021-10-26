using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wman.Logic.DTO_Models
{
    public class AssignedEventDTO
    {
        public int Id { get; set; }
        public string JobDescription { get; set; }
        public DateTime EstimatedStartDate { get; set; }
        public DateTime EstimatedFinishDate { get; set; }
        public AddressHUNDTO Address { get; set; }
    }
}
