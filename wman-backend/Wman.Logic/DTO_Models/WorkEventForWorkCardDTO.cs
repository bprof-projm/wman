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
        public int Id { get; set; }
        public string JobDescription { get; set; }
        public DateTime EstimatedStartDate { get; set; }
        public DateTime EstimatedFinishDate { get; set; }      
        public List<WorkerDTO> AssignedUsers { get; set; }
        public List<ListLabelsDTO> Labels { get; set; }
        public AddressHUNDTO Address { get; set; }
        public DateTime WorkStartDate { get; set; }
        public DateTime WorkFinishDate { get; set; }
        public List<ProofOfWorkDTO> ProofOfWorkPic { get; set; }
        public Status Status { get; set; }
    }
}
