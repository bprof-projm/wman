using Microsoft.AspNetCore.Identity;
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

        Task<IdentityResult> CreateUser(RegisterDTO login);

        Task<IdentityResult> DeleteUser(string uname);

        Task<IdentityResult> UpdateUser(string oldUsername, string pwd, UserDTO newUser);

        Task<TokenModel> LoginUser(LoginDTO login);

        Task<IEnumerable<string>> GetAllRolesOfUser(string username);

        Task AssignRoleToUser(string username, string role);

        Task<List<WmanUser>> GetAllUsersOfRole(string roleId);

    }
}
