using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wman.Logic.Helpers
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string message) : base(message) { }
    }
    public class EmailNotFoundException : Exception
    {
        public EmailNotFoundException(string message) : base(message) { }
    }

    public class IncorrectPasswordException : Exception
    {
        public IncorrectPasswordException(string message) : base(message) { }
    }
    public class HasNoRoleException : Exception
    {
        public HasNoRoleException(string message) : base(message) { }
        public HasNoRoleException(string mymessage, string username, string rolename) : base(mymessage)
        {
            mymessage = string.Format(mymessage, username, rolename);
        }
    }
    public class EmptyResultException : Exception
    {
        public EmptyResultException(string message) : base(message) { }
    }

}
