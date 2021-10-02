﻿using System;
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
        public static WmanUser Convert(userDTO user) 
        {
            throw new NotImplementedException();
        }

        public static userDTO Convert(WmanUser user)
        {
            var output = new userDTO();
            output.Email = user.Email;
            output.Firstname = user.FirstName;
            output.Lastname = user.LastName;
            output.Password = user.PasswordHash;
            output.Username = user.UserName;
            output.Picture = user.Picture;

            return output;

        }

        public static IEnumerable<userDTO> MassConvert(IEnumerable<WmanUser> input)
        {
            var output = new List<userDTO>();
            foreach (var item in input)
            {
                output.Add(Convert(item));
            }

            return output;
        }
    }
}
