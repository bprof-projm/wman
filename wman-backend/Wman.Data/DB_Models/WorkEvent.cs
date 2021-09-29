using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wman.Data.DB_Models
{
    class WorkEvent
    {
        [Key]
        public int Id { get; set; }

        public string JobDescription { get; set; }
    }
}
