using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wman.Logic.DTO_Models
{
    public class TokenModel
    {
        [Required]
        [DataType(DataType.Text)]
        public string Token { get; set; }

        [DataType(DataType.Date)]
        public DateTime ExpirationDate { get; set; }
    }
}
