using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wman.Logic.DTO_Models
{
    public class LoginDTO
    {
        [DataType(DataType.Text)]
        [MaxLength(254)]
        public string LoginName{ get; set; }
        [Required(AllowEmptyStrings =false)]
        [DataType(DataType.Password)]
        [StringLength(20, MinimumLength = 6)]

        public string Password { get; set; }
    }
}
