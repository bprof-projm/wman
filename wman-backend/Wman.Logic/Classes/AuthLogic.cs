using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Wman.Data.DB_Models;
using Wman.Logic.DTO_Models;
using Wman.Logic.Helpers;
using Wman.Logic.Interfaces;
using Wman.Repository.Interfaces;

namespace Wman.Logic.Classes
{
    public class AuthLogic : IAuthLogic
    {
        UserManager<WmanUser> userManager;
        RoleManager<WmanRole> roleManager;
        IMapper mapper;
        private IConfiguration Configuration;
        public AuthLogic(UserManager<WmanUser> userManager, RoleManager<WmanRole> roleManager, IConfiguration configuration, IMapper mapper)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.Configuration = configuration;
            this.mapper = mapper;
        }
        public async Task<IEnumerable<UserDTO>> GetAllUsers()
        {
            return mapper.Map<IEnumerable<UserDTO>>(await userManager.Users
                .Include(x =>x.ProfilePicture)
                .AsNoTracking()
                .ToListAsync());
        }

        public async Task<UserDTO> GetOneUser(string username)
        {
            var output = await userManager.Users.Where(x => x.UserName == username)
                .Include(x => x.ProfilePicture)
                .AsNoTracking()
                .SingleOrDefaultAsync();
            if (output == null)
            {
                throw new NotFoundException(WmanError.UserNotFound);
            }
            return mapper.Map<UserDTO>(output);
        }

        public async Task<IdentityResult> UpdateUser(string oldUsername, string pwd, UserDTO newUser)
        {

            var result = new IdentityResult();
            var user = userManager.Users.Where(x => x.UserName == oldUsername).SingleOrDefault();
            if (user == null)
            {
                throw new NotFoundException(WmanError.UserNotFound);
            }
            user.UserName = newUser.Username; //Not using converter class/automapper on purpose
            user.Email = newUser.Email;
            user.FirstName = newUser.Firstname;
            user.LastName = newUser.Lastname;
            user.ProfilePicture = newUser.Picture;
            user.PasswordHash = userManager.PasswordHasher.HashPassword(user, pwd);

            result = await userManager.UpdateAsync(user);
            await this.CheckResult(result);
            return result;
        }


        public async Task<IdentityResult> DeleteUser(string uname)
        {
            var result = new IdentityResult();
            var user = userManager.Users.Where(x => x.UserName == uname).SingleOrDefault();
            if (user == null)
            {
                throw new NotFoundException(WmanError.UserNotFound);
            }
            result = await userManager.DeleteAsync(user);
            await this.CheckResult(result);
            return result;
            

        }

        public async Task<IdentityResult> CreateWorker(RegisterDTO model)
        {
            var result = new IdentityResult();
            var user = new WmanUser();
            user = userManager.Users.Where(x => x.Email == model.Email).SingleOrDefault();
            if (user != null)
            {
                throw new InvalidOperationException(WmanError.EmailExists);
            }
            user = new WmanUser
            {
                Email = model.Email,
                UserName = model.Username,
                FirstName = model.Firstname,
                LastName = model.Lastname,
                SecurityStamp = Guid.NewGuid().ToString()
            };
            result = await userManager.CreateAsync(user, model.Password);
            if (await CheckResult(result))
            {
                await userManager.AddToRoleAsync(user, "Worker");
            }

            return result;
        }

        public async Task<TokenModel> LoginUser(LoginDTO model)
        {
            var user = new WmanUser();
            if (model.LoginName.Contains('@'))
            {
                user = await userManager.Users.Where(x => x.Email == model.LoginName).SingleOrDefaultAsync();
            }
            else if (model.LoginName != null)
            {
                user = await userManager.FindByNameAsync(model.LoginName);
            }
            if (user == null)
            {
                throw new NotFoundException(WmanError.UserNotFound);
            }
            else if (await userManager.CheckPasswordAsync(user, model.Password))
            {
                var claims = new List<Claim>
                {
                  new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                  new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                  new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
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
            throw new IncorrectPasswordException(WmanError.IncorrectPassword);
        }
        public async Task SetRoleOfUser(string username, string roleName)
        {
            WmanUser selectedUser = await userManager.FindByNameAsync(username);

            if (selectedUser == null)
            {
                throw new NotFoundException(WmanError.UserNotFound);
            }
            await this.RemovePrevRoles(selectedUser);
            await userManager.AddToRoleAsync(selectedUser, roleName);
        }

        public async Task<List<UserDTO>> GetAllUsersOfRole(string roleName)
        {
            var users = await this.userManager.GetUsersInRoleAsync(roleName);
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                throw new NotFoundException(WmanError.RoleNotFound);
            }
            return mapper.Map<List<UserDTO>>(users);
        }
        public async Task<IEnumerable<string>> GetAllRolesOfUser(string username)
        {
            var user = await this.userManager.FindByNameAsync(username);
            if (user == null)
            {
                throw new NotFoundException(WmanError.UserNotFound);
            }
            return await userManager.GetRolesAsync(user);
        }

        private async Task RemovePrevRoles(WmanUser user)
        {
            var roles = await userManager.GetRolesAsync(user);
            foreach (var item in roles)
            {
                await userManager.RemoveFromRolesAsync(user, roles);
            }
        }
        private async Task<bool> CheckResult(IdentityResult result)
        {
            if (result.Succeeded)
            {
                return true;
            }

            var output = "";
            foreach (var item in result.Errors)
            {
                output += item.Description + "\n";
            }
            throw new InvalidOperationException(output);
        }
    }
}
