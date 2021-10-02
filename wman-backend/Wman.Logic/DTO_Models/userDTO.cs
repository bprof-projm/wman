using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wman.Logic.DTO_Models
{
    public class userDTO
    {
        [DataType(DataType.Text)]
        [Required(AllowEmptyStrings = false)]
        [StringLength(30, MinimumLength = 3)]

        public string Username { get; set; }

        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        [MaxLength(150)]
        [Required(AllowEmptyStrings = false)]

        public string Email { get; set; }

        [DataType(DataType.Password)]
        [StringLength(20, MinimumLength = 5)]
        [Required(AllowEmptyStrings = false)]
        public string Password { get; set; }

        [DataType(DataType.Text)]

        [StringLength(30)]
        public string Firstname { get; set; }

        [DataType(DataType.Text)]
        [StringLength(30)]
        public string Lastname { get; set; }

        //[DataType(DataType.ImageUrl)]
        [StringLength(100)]
        public string Picture { get; set; }


    }
}
