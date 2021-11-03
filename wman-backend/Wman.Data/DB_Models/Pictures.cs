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
    public enum PicturesType
    {
        ProfilePic,
        ProofOfWorkPic
    }
    public class Pictures
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string CloudPhotoID { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public PicturesType PicturesType { get; set; }
        public int WManUserID { get; set; }
        public int WorkEventID { get; set; }
        [JsonIgnore]
        public virtual ICollection<WorkEvent> WorkEvents { get; set; }
        public Pictures()
        {
            this.WorkEvents = new List<WorkEvent>();
        }
    }
}
