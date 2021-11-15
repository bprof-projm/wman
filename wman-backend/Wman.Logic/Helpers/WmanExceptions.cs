using System;

namespace Wman.Logic.Helpers
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string message) : base(message) { }
    }

    public class IncorrectPasswordException : Exception
    {
        public IncorrectPasswordException(string message) : base(message) { }
    }
    public class NotMemberOfRoleException : Exception
    {
        public NotMemberOfRoleException(string message) : base(message) { }
    }
}
