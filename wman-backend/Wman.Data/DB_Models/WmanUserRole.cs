using Microsoft.AspNetCore.Identity;

namespace Wman.Data.DB_Models
{
    public class WmanUserRole : IdentityUserRole<int>
    {
        public WmanUser User { get; set; }
        public WmanRole Role { get; set; }
    }
}