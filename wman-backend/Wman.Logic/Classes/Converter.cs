using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wman.Data.DB_Models;
using Wman.Logic.DTO_Models;
using Wman.Logic.Interfaces;

namespace Wman.Logic.Classes
{
    public static class Converter
        //Ugly. Might use Automapper in the future, but we had performance (and other) concerns about it on our initial meeting. 
    {
        public static WmanUser Convert(UserDTO user) 
        {
            throw new NotImplementedException();
        }

        public static UserDTO Convert(WmanUser user)
        {
            if (user == null)
            {
                return null;
            }
            var output = new UserDTO();
            output.Email = user.Email;
            output.Firstname = user.FirstName;
            output.Lastname = user.LastName;
            //output.Password = user.PasswordHash;
            output.Username = user.UserName;
            output.Picture = user.ProfilePicture;

            return output;

        }
        public static CalendarWorkEventDTO Convert(WorkEvent input)
        {
            if (input == null)
            {
                return null;
            }
            var output = new CalendarWorkEventDTO();
            output.Id = input.Id;
            output.JobDescription = input.JobDescription;
            output.EstimatedStartDate = input.EstimatedStartDate;
            output.EstimatedFinishDate = input.EstimatedFinishDate;
            

            return output;

        }

        public static IEnumerable<UserDTO> MassConvert(IEnumerable<WmanUser> input)
        {
            var output = new List<UserDTO>();
            foreach (var item in input)
            {
                output.Add(Convert(item));
            }

            return output;
        }
        public static IEnumerable<CalendarWorkEventDTO> CalendarWorkEventConverter(IEnumerable<WorkEvent> input)
        {
            var output = new List<CalendarWorkEventDTO>();
            foreach (var item in input)
            {
                output.Add(Convert(item));
            }

            return output;
        }
    }
}
