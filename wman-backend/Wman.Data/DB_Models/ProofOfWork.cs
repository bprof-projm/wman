using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wman.Data.DB_Models
{
    public class ProofOfWork
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string CloudPhotoID { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public int WorkEventID { get; set; }
        [NotMapped]
        public virtual WorkEvent WorkEvents { get; set; }
    }
}
