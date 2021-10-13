using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

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
        [JsonIgnore]
        public virtual ICollection<WmanUser> AssignedUsers { get; set; }
        
        [JsonIgnore]
        public ICollection<Label> Labels { get; set; }
        
        [JsonIgnore]
        public ICollection<Pictures> ProofOfWorkPic { get; set; }
        public int AddressId { get; set; }
        public virtual AddressHUN Address { get; set; }
        public DateTime WorkStartDate { get; set; }
        public DateTime WorkFinishDate { get; set; }
        public TimeSpan WorkTime { get; set; }
        //[JsonConverter(typeof(JsonStringEnumConverter))]
        public Status Status { get; set; }
        public WorkEvent()
        {
            AssignedUsers = new List<WmanUser>();
            this.Address = new AddressHUN();
        }
    }
}
