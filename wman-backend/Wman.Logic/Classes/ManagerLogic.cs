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
    public class ManagerLogic : IManagerLogic
    {
        UserManager<WmanUser> userManager;

        public ManagerLogic(UserManager<WmanUser> userManager)
        {
            this.userManager = userManager;
        }

        
    }
}
