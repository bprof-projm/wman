using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Wman.Data.DB_Models
{
    public class WmanUser : IdentityUser<int>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Pictures ProfilePicture { get; set; }
        [NotMapped]
        public ICollection<WmanUserRole> UserRoles { get; set; }
        [JsonIgnore]
        public virtual ICollection<WorkEvent> WorkEvents { get; set; }

        public WmanUser()
        {
            WorkEvents = new List<WorkEvent>();
        }
    }
}
