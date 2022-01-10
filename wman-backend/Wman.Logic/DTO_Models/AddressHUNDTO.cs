using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wman.Logic.DTO_Models
{
    public class AddressHUNDTO
    {
        [Required]
        [StringLength(20)]
        [RegularExpression(@"^[\d \p{L} \s]+$")]
        [DataType(DataType.Text)]
        public string City { get; set; }
        [Required]
        [StringLength(30)]
        [RegularExpression(@"^[\d \p{L} \s]+$")]
        [DataType(DataType.Text)]
        public string Street { get; set; }
        [RegularExpression(@"^\d{4}$")]
        public int ZIPCode { get; set; }
        [Required]
        [StringLength(20)]
        [DataType(DataType.Text)]
        public string BuildingNumber { get; set; }
        [StringLength(20)]
        [DataType(DataType.Text)]
        public string FloorDoor { get; set; }
    }
}
