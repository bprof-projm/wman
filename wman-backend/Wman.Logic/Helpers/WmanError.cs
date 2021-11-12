using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wman.Logic.Helpers
{
    public class WmanError
    {
        public static string UserNotFound { get { return "The selected user could not be found"; } }
        public static string IncorrectPassword { get { return "The provided password is incorrect"; } }
        public static string EmailExists { get { return "There is already a user registered with the provided email address!"; } }
        public static string RoleNotFound { get { return "The specified role does not exist!"; } }
        public static string EventNotFound { get { return "The specified event does not exist!"; } }
    }
}
