﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wman.Logic.Interfaces;

namespace Wman.Logic.Classes
{
    public class AdminLogic : IAdminLogic
    {
        public void test(IAuthLogic logic)
        {
            var test = logic.GetAllUsers().Result.First();
            ;
        }
    }
}
