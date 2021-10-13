using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wman.Data.DB_Connection_Tables;

namespace Wman.Data.DB_Models
{
    public class WmanUser : IdentityUser<int>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Pictures ProfilePicture { get; set; }
        [NotMapped]
        public ICollection<WmanUserRole> UserRoles { get; set; }
        [NotMapped]
        public virtual ICollection<WmanUserWorkEvent> WorkEvents { get; set; }

    }
}
