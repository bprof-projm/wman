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
        Task<UserDTO> GetOneUser(string username);

        Task<IEnumerable<UserDTO>> GetAllUsers();

        Task<IdentityResult> CreateWorker(RegisterDTO login);

        Task<IdentityResult> DeleteUser(string uname);

        Task<IdentityResult> UpdateUser(string oldUsername, string pwd, UserDTO newUser);

        Task<TokenModel> LoginUser(LoginDTO login);

        Task<IEnumerable<string>> GetAllRolesOfUser(string username);

        Task SetRoleOfUser(string username, string role);

        Task<List<UserDTO>> GetAllUsersOfRole(string roleId);

    }
}
