using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wman.Logic.Helpers
{
    public class WmanError
    {
        public static string GenericError { get { return nameof(GenericError); } }
        public static string UserNotFound { get { return nameof(UserNotFound); } }
        public static string IncorrectPassword { get { return nameof(IncorrectPassword); } }
    }
}
