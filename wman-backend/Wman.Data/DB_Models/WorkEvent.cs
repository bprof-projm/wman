using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
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
    public enum Status
    {
        awaiting, started, finished
    }
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
        public ICollection<WmanUserWorkEvent> AssignedUsers { get; set; }
        [NotMapped]
        [JsonIgnore]
        public ICollection<WorkEventLabel> Labels { get; set; }
        [NotMapped]
        [JsonIgnore]
        public ICollection<WorkEventPicture> ProofOfWorkPic { get; set; }
        public int AddressId { get; set; }
        [NotMapped]
        public virtual AddressHUN Address { get; set; }
        public DateTime WorkStartDate { get; set; }
        public DateTime WorkFinishDate { get; set; }
        public TimeSpan WorkTime { get; set; }
        //[JsonConverter(typeof(JsonStringEnumConverter))]
        public Status Status { get; set; }
    }
}
