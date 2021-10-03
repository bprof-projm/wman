using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
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
        private IConfiguration Configuration;

        public AuthLogic(UserManager<WmanUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.Configuration = configuration;
        }
        public async Task<IQueryable<WmanUser>> GetAllUsers()
        {
            return userManager.Users;
        }

        public async Task<WmanUser> GetOneUser(string username)
        {
            return await userManager.Users.Where(x => x.UserName == username).SingleOrDefaultAsync();
        }

        public async Task<IdentityResult> UpdateUser(string oldUsername, userDTO newUser)
        {

            var result = new IdentityResult();
            var user = userManager.Users.Where(x => x.UserName == oldUsername).SingleOrDefault();
            if (user == null)
            {
                var myerror = new IdentityError() { Code = "UserNotFound", Description = "User not found!" };
                return IdentityResult.Failed(myerror);
            }
            user.UserName = newUser.Username; //Not using converter class/automapper on purpose
            user.Email = newUser.Email;
            user.FirstName = newUser.Firstname;
            user.LastName = newUser.Lastname;
            user.Picture = newUser.Picture;
            user.PasswordHash = userManager.PasswordHasher.HashPassword(user, newUser.Password);

            result = await userManager.UpdateAsync(user);
            return result;
        }


        public async Task<IdentityResult> DeleteUser(string uname)
        {
            var myerror = new IdentityError() { Code = "UserNotFound", Description = "User not found!" };
            var result = new IdentityResult();
            var user = userManager.Users.Where(x => x.UserName == uname).SingleOrDefault();
            if (user == null)
            {
                return IdentityResult.Failed(myerror);
            }
            result = await userManager.DeleteAsync(user);

            return result;

        }

        public async Task<IdentityResult> CreateUser(userDTO model)
        {
            var result = new IdentityResult();
            var user = new WmanUser();
            //Reinvented the wheel, it does this by itself :(

            //user = userManager.Users.Where(x => x.UserName == model.Username).SingleOrDefault();
            //if (user != null)
            //{
            //    var myerror = new IdentityError() { Code = "UsernameExists", Description = "Username already exists!" };
            //    return IdentityResult.Failed(myerror);
            //}
            user = userManager.Users.Where(x => x.Email == model.Email).SingleOrDefault();
            if (user != null)
            {
                var myerror = new IdentityError() { Code = "EmailExists", Description = "An accunt with this email address already exists!" };
                return IdentityResult.Failed(myerror);
            }
            user = new WmanUser
            {
                Email = model.Email,
                UserName = model.Username,
                Picture = model.Picture,
                FirstName = model.Firstname,
                LastName = model.Lastname,
                SecurityStamp = Guid.NewGuid().ToString()
            };
            result = await userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user, "Debug");
                return result;
            }

            return result;
        }

        public async Task<TokenModel> LoginUser(LoginDTO model)
        {
            var user = new WmanUser();
            if (model.Username != null)
            {
                user = await userManager.FindByNameAsync(model.Username);
            }
            else if (model.Email != null)
            {
                user = await userManager.FindByNameAsync(model.Email);
            }
            else
            {
                throw new ArgumentException("No username/email was provided!");
            }
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
                  Encoding.UTF8.GetBytes(Configuration.GetValue<string>("SigningKey")));

                var token = new JwtSecurityToken(
                  issuer: "http://www.security.org",
                  audience: "http://www.security.org",
                  claims: claims,
                  expires: DateTime.Now.AddMinutes(60),
                  signingCredentials: new SigningCredentials(signinKey, SecurityAlgorithms.HmacSha256)
                );
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
            if ( await userManager.IsInRoleAsync(user, role))
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
            return await userManager.GetRolesAsync(user);
        }

        public async Task<bool> AssignRolesToUser(WmanUser user, List<string> roles)
        {
            WmanUser selectedUser;
            selectedUser = await GetOneUser(user.UserName);
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
                var user = this.GetOneUser(userName).Result;
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
