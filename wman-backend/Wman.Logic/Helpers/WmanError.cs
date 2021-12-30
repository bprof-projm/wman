using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wman.Logic.Helpers
{
    public class WmanError
    {
        public static string UserNotFound = "The selected user could not be found";
        public static string IncorrectPassword = "The provided password is incorrect";
        public static string EmailExists = "There is already a user registered with the provided email address";
        public static string RoleNotFound = "The specified role does not exist";
        public static string EventNotFound = "The specified event does not exist";
        public static string InvalidInputRange = "The provided input parameter is not in the valid range";
        public static string NotAWorker = "Specified user is not member of the role 'Worker'";
        public static string UserIsBusy = "The user is busy in the event's timeframe";
        public static string EventDateInvalid = "The event date is invalid!"; //Not in same day/starts after it finishes
        public static string LabelExists = "This Label already exists!";
        public static string NoLabels = "There are no labels added!";
        public static string LabelNotFound = "Label not found!";
        public static string WrongColor = "The color should be in hexadecimal format";
        public static string PhotoNotFound = "Photo not found";
        public static string NotHisBusiness = "The currently logged in user is not assigned to the requested event";
        public static string AdminExists = "At least one admin user already exists!";
        public static string NotAWorkforce = "Specified user is not a workforce!";
        public static string CantCreateAdmin = "Admins cannot be created/edited using this api!";
        public static string StatusFinished = "This event have already been finished";
        public static string StatusPowMissing = "This event can not be finished until you provide the proof of work photo(s)";
    }
}
