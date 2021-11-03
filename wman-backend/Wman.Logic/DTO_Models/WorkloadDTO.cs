using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wman.Data.DB_Models;

namespace Wman.Logic.DTO_Models
{
   public class WorkloadDTO
    {
        public string Username { get; set; }
        public string ProfilePicUrl { get; set; }
        public int Percent { get; set; }
    }
}
