using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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
        IPhotoLogic photoLogic;
        IMapper mapper;
        public AdminLogic(UserManager<WmanUser> userManager, IPhotoLogic photoLogic, IMapper mapper)
        {
            this.userManager = userManager;
            this.photoLogic = photoLogic;
            this.mapper = mapper;
        }
        public async Task<IdentityResult> UpdateWorkforce(string username, WorkerModifyDTO model)
        {
            var result = new IdentityResult();
            var user = userManager.Users.Where(x => x.UserName == username).SingleOrDefault();
            if (user == null)
            {
                throw new NotFoundException(WmanError.UserNotFound);
            }
            if (await userManager.IsInRoleAsync(user, "Admin") || await userManager.IsInRoleAsync(user, "SystemAdmin"))
            {
                throw new InvalidOperationException(WmanError.NotAWorkforce);
            }
            

            user.Email = model.Email;
            user.FirstName = model.Firstname;
            user.LastName = model.Lastname;
            user.PhoneNumber = model.PhoneNumber;
            if (!string.IsNullOrWhiteSpace(model.Role))
            {
                if (model.Role.ToLower() == "admin")
                {
                    throw new InvalidOperationException(WmanError.CantCreateAdmin);
                }
                if (model.Role.ToLower() != "worker" && model.Role.ToLower() != "manager")
                {
                    throw new InvalidOperationException(WmanError.RoleNotFound);
                }
                var prevRole = userManager.GetRolesAsync(user).Result.FirstOrDefault();
                
                if (prevRole.ToLower() != model.Role.ToLower())
                {
                    await userManager.RemoveFromRoleAsync(user, prevRole);
                    await userManager.AddToRoleAsync(user, model.Role);
                }
            }
           
            //user.PasswordHash = userManager.PasswordHasher.HashPassword(user, model.Password);
            if (model.Photo != null)
            {
                await photoLogic.UpdateProfilePhoto(username, model.Photo);
            }
            result = await userManager.UpdateAsync(user);
            await this.CheckResult(result);
            return result;
        }


        public async Task<IdentityResult> DeleteWorkforce(string uname)
        {
            var result = new IdentityResult();
            var user = userManager.Users.Where(x => x.UserName == uname).SingleOrDefault();
            if (user == null)
            {
                throw new NotFoundException(WmanError.UserNotFound);
            }
            if (await userManager.IsInRoleAsync(user, "Admin") || await userManager.IsInRoleAsync(user, "SystemAdmin"))
            {
                throw new InvalidOperationException(WmanError.NotAWorkforce);
            }
            if (user.ProfilePicture != null)
            {
                await photoLogic.RemoveProfilePhoto(user.UserName);
            }
            result = await userManager.DeleteAsync(user);
            await this.CheckResult(result);

            return result;
        }

        public async Task<IdentityResult> CreateWorkforce(RegisterDTO model)
        {
            if (model.Role.ToLower().Contains("admin"))
            {
                throw new InvalidOperationException(WmanError.CantCreateAdmin);
            }
                if (model.Role.ToLower() != "worker" && model.Role.ToLower() != "manager")
                {
                    throw new InvalidOperationException(WmanError.RoleNotFound);
                }
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
                PhoneNumber = model.PhoneNumber,
                SecurityStamp = Guid.NewGuid().ToString()
            };
            result = await userManager.CreateAsync(user, model.Password);
            if (await CheckResult(result))
            {
                await userManager.AddToRoleAsync(user, model.Role);
            }
            if (model.Photo != null)
            {
               await photoLogic.AddProfilePhoto(user.UserName, model.Photo);
            }
            return result;
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
