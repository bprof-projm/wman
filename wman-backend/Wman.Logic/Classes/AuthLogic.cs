using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Wman.Data.DB_Models;
using Wman.Logic.DTO_Models;
using Wman.Logic.Interfaces;

namespace Wman.Logic.Classes
{
    public class AuthLogic : IAuthLogic
    {

        UserManager<WmanUser> userManager;
        RoleManager<IdentityRole> roleManager;


        public AuthLogic(UserManager<WmanUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
        }
        public async Task<IQueryable<WmanUser>> GetAllUsers()
        {
            return userManager.Users;
        }

        public async Task<WmanUser> GetOneUser(int id, string email)
        {
            if (id != -1)
            {
                return userManager.Users.Where(x => x.Id == id).SingleOrDefault();
            }
            else if (email != null)
            {
                return userManager.Users.Where(x => x.Email == email).SingleOrDefault();
            }
            else
            {
                throw new ArgumentException();
            }
        }

        public async Task<string> UpdateUser(string oldUsername, WmanUser newUser)
        {
            await userManager.UpdateAsync(newUser);
            return "Success";
        }


        public async Task<string> DeleteUser(string uname)
        {
            try
            {
                var user = userManager.Users.Where(x => x.UserName == uname).Single();
                await userManager.DeleteAsync(user);
                return "Success";
            }
            catch (Exception)
            {
                return "Fail";
            }

        }

        public async Task<string> CreateUser(Login model)
        {
            var user = new WmanUser
            {
                Email = model.Email,
                UserName = model.Username,
                SecurityStamp = Guid.NewGuid().ToString()
            };
            var result = await userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user, "Debug");
                return "OK";
            }
            return "NOT OK";
        }

        public async Task<TokenModel> LoginUser(Login model)
        {
            var user = await userManager.FindByNameAsync(model.Email);
            if (user != null && await userManager.CheckPasswordAsync(user, model.Password))
            {


                var claims = new List<Claim>
                {
                  new Claim(JwtRegisteredClaimNames.Sub, model.Email),
                  new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                  new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()) //TODO: .tostring might break something, test.
                };


                var roles = await userManager.GetRolesAsync(user);
                ;
                claims.AddRange(roles.Select(role => new Claim(ClaimsIdentity.DefaultRoleClaimType, role)));


                var signinKey = new SymmetricSecurityKey(
                  Encoding.UTF8.GetBytes("abc 123 970608 yxcvbnm"));

                var token = new JwtSecurityToken(
                  issuer: "http://www.security.org",
                  audience: "http://www.security.org",
                  claims: claims,
                  expires: DateTime.Now.AddMinutes(60),
                  signingCredentials: new SigningCredentials(signinKey, SecurityAlgorithms.HmacSha256)
                );
                bool adminState = false;
                if (roles.Contains("Admin"))
                {
                    adminState = true;
                }

                bool editorState = false;
                if (roles.Contains("Editor"))
                {
                    editorState = true;
                }
                return new TokenModel
                {
                    Token = new JwtSecurityTokenHandler().WriteToken(token),
                    ExpirationDate = token.ValidTo
                };
            }
            throw new ArgumentException("Login failed");
        }

        public async Task<bool> HasRole(WmanUser user, string role)
        {
            if (userManager.IsInRoleAsync(user, role).Result)
            {
                return true;
            }
            return false;
        }

        public async Task<bool> HasRoleByName(string userName, string role)
        {
            var user = await this.userManager.FindByNameAsync(userName);
            if (userManager.IsInRoleAsync(user, role).Result/* || userManager.IsInRoleAsync(user, "Admin").Result*/)
            {
                return true;
            }
            return false;
        }
        public async Task<IEnumerable<string>> GetAllRolesOfUser(WmanUser user)
        {
            return userManager.GetRolesAsync(user).Result.ToList();
        }

        public async Task<bool> AssignRolesToUser(WmanUser user, List<string> roles)
        {
            WmanUser selectedUser;
            selectedUser = GetOneUser(user.Id, null).Result;
            userManager.AddToRolesAsync(selectedUser, roles).Wait();
            return true;
        }

        public async Task<bool> CreateRole(string name)
        {
            var query = await this.roleManager.FindByNameAsync(name);
            if (query != null)
            {
                return false;
            }
            roleManager.CreateAsync(new IdentityRole { Id = Guid.NewGuid().ToString(), Name = name, NormalizedName = name.ToUpper() }).Wait();
            return true;
        }

        public async Task<string> RemoveUserFromRole(string userName, string requiredRole)
        {
            try
            {
                var user = await this.userManager.FindByNameAsync(userName);
                await this.userManager.RemoveFromRoleAsync(user, requiredRole);
                return "Success";
            }
            catch (Exception)
            {
                return "Fail";
            }
        }

        public async Task<bool> SwitchRoleOfUser(string userName, string newRole)
        {
            try
            {
                var user = this.GetOneUser(-1, userName).Result;
                foreach (var role in this.GetAllRolesOfUser(user).Result)
                {
                    await this.RemoveUserFromRole(user.UserName, role);
                }
                await this.userManager.AddToRoleAsync(user, newRole);
                return true;
            }
            catch (Exception)
            {
                return false;
            }


        }
        public async Task<List<WmanUser>> GetAllUsersOfRole(string roleId)
        {
            var users = await this.userManager.GetUsersInRoleAsync(roleId);
            return users.ToList();
        }
    }
}
