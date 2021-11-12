﻿using System;
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
        public static string EmailExists { get { return "There is already a user registered with the provided email address"; } }
        public static string RoleNotFound { get { return "The specified role does not exist"; } }
        public static string EventNotFound { get { return "The specified event does not exist"; } }
        public static string InvalidInputRange { get { return "The provided input parameter is not in the valid range"; } }
        public static string HasNoRole { get { return "The user '{0}' does not have the role '{1}'"; } }
        public static string UserIsBusy { get { return "The user is busy in the event's timeframe"; } }
        public static string EventDateInvalid { get { return "The event date is invalid!"; } } //Not in same day/starts after it finishes

    }
}
