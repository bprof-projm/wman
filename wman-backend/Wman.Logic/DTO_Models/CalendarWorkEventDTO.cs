using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wman.Data.DB_Models;

namespace Wman.Logic.DTO_Models
{
    public class CalendarWorkEventDTO
    {
        
        public int Id { get; set; }
        [DataType(DataType.Text)]
        public string JobDescription { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime EstimatedStartDate { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime EstimatedFinishDate { get; set; }
        public AddressHUNDTO Address { get; set; }
    }
}
