using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wman.Logic.DTO_Models
{
    public class CreateLabelDTO
    {
        [RegularExpression("^#(?:[0-9a-fA-F]{3}){1,2}$")]
        public string Color { get; set; }
        [StringLength(10)]
        public string Content { get; set; }
    }
}
