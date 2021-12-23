using AutoMapper;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wman.Data.DB_Models;
using Wman.Logic.DTO_Models;
using Wman.Logic.Helpers;
using Wman.Logic.Interfaces;

namespace Wman.Logic.Classes
{
    public class AdminLogic : IAdminLogic
    {
        UserManager<WmanUser> userManager;
        RoleManager<IdentityRole<int>> roleManager;
        IMapper mapper;
        public AdminLogic(UserManager<WmanUser> userManager, RoleManager<IdentityRole<int>> roleManager, IMapper mapper)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.mapper = mapper;
        }
        public async Task<IdentityResult> UpdateWorker(string username, WorkerModifyDTO model)
        {
            var result = new IdentityResult();
            var user = userManager.Users.Where(x => x.UserName == username).SingleOrDefault();
            if (user == null)
            {
                throw new NotFoundException(WmanError.UserNotFound);
            }
            if (await userManager.IsInRoleAsync(user, "Worker") == false)
            {
                throw new InvalidOperationException(WmanError.NotAWorker);
            }
            user.Email = model.Email;
            user.FirstName = model.Firstname;
            user.LastName = model.Lastname;
            //user.ProfilePicture = newUser.Picture;
            //user.PasswordHash = userManager.PasswordHasher.HashPassword(user, model.Password);

            result = await userManager.UpdateAsync(user);
            await this.CheckResult(result);
            return result;
        }


        public async Task<IdentityResult> DeleteWorker(string uname)
        {
            var result = new IdentityResult();
            var user = userManager.Users.Where(x => x.UserName == uname).SingleOrDefault();
            if (user == null)
            {
                throw new NotFoundException(WmanError.UserNotFound);
            }
            if (await userManager.IsInRoleAsync(user, "Worker") == false)
            {
                throw new InvalidOperationException(WmanError.NotAWorker);
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

        public void test(IAuthLogic logic)
        {
            var test = logic.GetAllUsers().Result.First();
            ;
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
