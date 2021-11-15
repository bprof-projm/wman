using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wman.Logic.DTO_Models
{
    public class DnDEventDTO
    {
        [Required]
        [DataType(DataType.DateTime)]
        public DateTime EstimatedStartDate { get; set; }
        [Required]
        [DataType(DataType.DateTime)]
        public DateTime EstimatedFinishDate { get; set; }
    }
}
