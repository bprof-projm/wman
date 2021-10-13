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
    public class Label
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Color { get; set; }
        public string Content { get; set; }
        [NotMapped]
        [JsonIgnore]
        public virtual ICollection<WorkEventLabel> WorkEvents { get; set; }
    }
}
