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
            output.Password = user.PasswordHash;
            output.Username = user.UserName;
            output.Picture = user.Picture;

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
    }
}
