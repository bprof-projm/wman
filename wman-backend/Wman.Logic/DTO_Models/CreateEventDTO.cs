
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Wman.Data.DB_Models;

namespace Wman.Logic.DTO_Models
{
    
    public class CreateEventDTO
    {
        [Required]
        [StringLength(200)]
        [DataType(DataType.Text)]
        public string JobDescription { get; set; }
        [Required]
        [DataType(DataType.DateTime)]
        public DateTime EstimatedStartDate { get; set; }
        [Required]
        [DataType(DataType.DateTime)]
        public DateTime EstimatedFinishDate { get; set; }
        [Required]
        public AddressHUNDTO Address { get; set; }
        [Required]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Status Status { get; set; }
    }
}
