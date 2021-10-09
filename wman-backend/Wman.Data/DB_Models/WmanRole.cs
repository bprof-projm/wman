using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wman.Data.DB_Models
{
    public class WmanRole : IdentityRole<int>
    {
        public ICollection<WmanUserRole> UserRoles { get; set; }
    }
}
