﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wman.Data.DB_Models;
using Wman.Logic.DTO_Models;

namespace Wman.Logic.Interfaces
{
    public interface IAuthLogic
    {

        Task<WmanUser> GetOneUser(string username);


        Task<IQueryable<WmanUser>> GetAllUsers();


        Task<IdentityResult> CreateUser(Login login);


        Task<IdentityResult> DeleteUser(string uname);


        Task<IdentityResult> UpdateUser(string oldUsername, WmanUser newUser);

        Task<TokenModel> LoginUser(Login login);

        Task<bool> HasRole(WmanUser user, string role);


        Task<IEnumerable<string>> GetAllRolesOfUser(WmanUser user);


        Task<bool> AssignRolesToUser(WmanUser user, List<string> roles);


        Task<List<WmanUser>> GetAllUsersOfRole(string roleId);


        Task<bool> HasRoleByName(string userName, string role);
    }
}
