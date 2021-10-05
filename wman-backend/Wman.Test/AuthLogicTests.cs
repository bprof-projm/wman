using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Wman.Data.DB_Models;
using Wman.Logic.Classes;
using Wman.Logic.DTO_Models;

namespace Wman.Test
{
    public class AuthLogicTests
    {
        
        private Mock<UserManager<WmanUser>> userManager;
        private Mock<RoleManager<IdentityRole>> roleManager;
        private Mock<IConfiguration> config;

        private List<WmanUser> users;

        [SetUp]
        public void Setup()
        {
            users = new List<WmanUser>();

            users.Add(new WmanUser { Id = 0, UserName="LmaoRandom", Email = "sanyesz@gmail.com",
                FirstName = "Sanyi", LastName = "Hurutos", Picture="asd", SecurityStamp = Guid.NewGuid().ToString() });
            users.Add(new WmanUser { Id = 1, UserName="TigoleBitties", Email = "foksok@gmail.com",
                FirstName = "Hamis", LastName = "Süni", Picture = "asd", SecurityStamp = Guid.NewGuid().ToString() });
            users.Add(new WmanUser { Id = 2, UserName="ArnoldBalValla",Email = "zsoltas@gmail.com",
                FirstName = "Medve", LastName = "Obudai", Picture = "asd", SecurityStamp = Guid.NewGuid().ToString() });

            userManager = GetUserManager(users);
            roleManager = GetMockRoleManager();
            config = new Mock<IConfiguration>();

            this.userManager.Setup(y => y.Users).Returns(users.AsQueryable());
        }

        [Test]
        public async Task CreateUser_SucceededCreation()
        {
            AuthLogic authLogic = new(this.userManager.Object, this.roleManager.Object, this.config.Object);

            UserDTO user = new UserDTO() { Username = "fogvaratartottGyik", Email = "maszkosfutocsiga@gmail.com",
                Password = "miAR3dV2Sf0s", Firstname = "Kronikus", Lastname = "VeszettMacska", Picture = "Keka" };

            var result = await authLogic.CreateUser(user);

            Assert.True(result.Succeeded);
            Assert.That(users.Count == 4);
        }

        [Test]
        public async Task DeleteUser_SucceededDeletion_OnRecentlyCreatedUserAndByName()
        {
            AuthLogic authLogic = new(this.userManager.Object, this.roleManager.Object, this.config.Object);

            UserDTO user = new UserDTO()
            {
                Username = "fogvaratartottGyik",
                Email = "maszkosfutocsiga@gmail.com",
                Password = "miAR3dV2Sf0s",
                Firstname = "Kronikus",
                Lastname = "VeszettMacska",
                Picture = "Keka"
            };

            await authLogic.CreateUser(user);

            //we have 4 users in the repo right now
            var result = await authLogic.DeleteUser(user.Username);
            var result2 = await authLogic.DeleteUser("ArnoldBalValla");

            //Should be 2 users in the repo after 2 deletions
            Assert.True(result.Succeeded);
            Assert.AreEqual(2, users.Count);
        }

        public static Mock<UserManager<WmanUser>> GetUserManager(List<WmanUser> ls)
        {
            var store = new Mock<IUserStore<WmanUser>>();
            var mgr = new Mock<UserManager<WmanUser>>(store.Object, null, null, null, null, null, null, null, null);

            mgr.Setup(x => x.DeleteAsync(It.IsAny<WmanUser>())).ReturnsAsync(IdentityResult.Success)
                .Callback<WmanUser>((x) => ls.Remove(x));
            mgr.Setup(x => x.CreateAsync(It.IsAny<WmanUser>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success)
                .Callback<WmanUser, string>((x, y) => ls.Add(x));
            mgr.Setup(x => x.UpdateAsync(It.IsAny<WmanUser>())).ReturnsAsync(IdentityResult.Success);

            return mgr;
        }

        public static Mock<RoleManager<IdentityRole>> GetMockRoleManager()
        {
            var roleStore = new Mock<IRoleStore<IdentityRole>>();

            return new Mock<RoleManager<IdentityRole>>(
                         roleStore.Object, null, null, null, null);
        }
    }
}
