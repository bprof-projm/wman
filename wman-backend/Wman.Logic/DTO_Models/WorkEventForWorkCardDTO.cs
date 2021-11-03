using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wman.Data.DB_Models;

namespace Wman.Logic.DTO_Models
{
    public class WorkEventForWorkCardDTO
    {
        public string JobDescription { get; set; }
        public DateTime EstimatedStartDate { get; set; }
        public DateTime EstimatedFinishDate { get; set; }      
        public List<UserDTO> AssignedUsers { get; set; }
        public List<ListLabelsDTO> Labels { get; set; }
        public AddressHUNDTO Address { get; set; }
        public DateTime WorkStartDate { get; set; }
        public DateTime WorkFinishDate { get; set; }
        public TimeSpan WorkTime { get; set; }
        public Status Status { get; set; }
    }
}
