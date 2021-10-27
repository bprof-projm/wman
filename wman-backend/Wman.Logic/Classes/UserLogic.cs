using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wman.Data.DB_Models;
using Wman.Logic.Interfaces;

namespace Wman.Logic.Classes
{
    public class UserLogic : IUserLogic
    {
        UserManager<WmanUser> userManager;

        public UserLogic(UserManager<WmanUser> userManager)
        {
            this.userManager = userManager;
        }
    }
}
