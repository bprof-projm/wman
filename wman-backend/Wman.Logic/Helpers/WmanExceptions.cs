using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wman.Logic.Helpers
{
    public class UserNotFoundException : Exception
    {
        public UserNotFoundException(string message) : base(message) { }
    }
    public class EmailNotFoundException : Exception
    {
        public EmailNotFoundException(string message) : base(message) { }
    }

    public class IncorrectPasswordException : Exception
    {
        public IncorrectPasswordException(string message) : base(message) { }
    }
    public class NotAllowedException : Exception
    {
        public NotAllowedException(string message) : base(message) { }
    }
    public class RoleNotFoundException : Exception
    {
        public RoleNotFoundException(string message) : base(message) { }
    }

    public class EventNotFoundException : Exception
    {
        public EventNotFoundException(string message) : base(message) { }
    }
}
