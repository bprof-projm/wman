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

        WmanUser GetOneUser(int userId, string email);


        IQueryable<WmanUser> GetAllUsers();


        Task<string> CreateUser(Login login);


        Task<string> DeleteUser(string uname);


        Task<string> UpdateUser(string oldUsername, WmanUser newUser);

        Task<TokenModel> LoginUser(Login login);

        bool HasRole(WmanUser user, string role);


        IEnumerable<string> GetAllRolesOfUser(WmanUser user);


        bool AssignRolesToUser(WmanUser user, List<string> roles);


        Task<List<WmanUser>> GetAllUsersOfRole(string roleId);


        Task<bool> HasRoleByName(string userName, string role);
    }
}
