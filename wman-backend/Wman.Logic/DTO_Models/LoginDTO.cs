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
        [StringLength(30, MinimumLength = 3)]
        public string Username { get; set; }

        [EmailAddress]
        [MaxLength(254)]
        public string Email { get; set; }
        [Required(AllowEmptyStrings =false)]
        [DataType(DataType.Password)]
        [StringLength(20, MinimumLength = 6)]

        public string Password { get; set; }
    }
}
