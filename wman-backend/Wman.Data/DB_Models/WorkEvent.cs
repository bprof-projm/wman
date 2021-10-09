using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Wman.Data.DB_Connection_Tables;

namespace Wman.Data.DB_Models
{
    public class WorkEvent
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string JobDescription { get; set; }
        public DateTime EstimatedStartDate { get; set; }
        public DateTime EstimatedFinishDate { get; set; }
        
        [NotMapped]
        [JsonIgnore]
        public ICollection<WorkEventPicture> ProofOfWorkPic { get; set; }
    }
}
