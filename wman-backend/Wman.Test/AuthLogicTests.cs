using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using MockQueryable.Moq;
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
        private IConfiguration config;

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
            config = GetConfiguration();
        }

        [Test]
        public async Task LoginUser_TokenGranted_OnNewlyCreatedUser()
        {
            AuthLogic authLogic = new(this.userManager.Object, this.roleManager.Object, this.config);

            UserDTO user = new UserDTO()
            {
                Username = "fogvaratartottGyik",
                Email = "maszkosfutocsiga@gmail.com",
                Password = "miAR3dV2Sf0s",
                Firstname = "Kronikus",
                Lastname = "VeszettMacska",
                Picture = "Keka"
            };

            var akarmi = await authLogic.CreateUser(user);

            LoginDTO model = new LoginDTO() { LoginName=user.Email, Password=user.Password };

            var result = await authLogic.LoginUser(model);

            Assert.That(result.Token != null);
        }

        [Test]
        public async Task GetOneUser()
        {
            AuthLogic authLogic = new(this.userManager.Object, this.roleManager.Object, this.config);

            var result = await authLogic.GetOneUser(users[0].UserName);

            Assert.AreEqual(result.UserName, users[0].UserName);
        }

        [Test]
        public async Task GetAllUsers_ReturnsRepoProperly()
        {
            AuthLogic authLogic = new(this.userManager.Object, this.roleManager.Object, this.config);

            var result = await authLogic.GetAllUsers();

            Assert.AreEqual(users.Count(), result.Count());
            this.userManager.Verify(x => x.Users, Times.Once);
        }

        [Test]
        public async Task CreateUser_SucceededCreation()
        {
            AuthLogic authLogic = new(this.userManager.Object, this.roleManager.Object, this.config);

            UserDTO user = new UserDTO() { Username = "fogvaratartottGyik", Email = "maszkosfutocsiga@gmail.com",
                Password = "miAR3dV2Sf0s", Firstname = "Kronikus", Lastname = "VeszettMacska", Picture = "Keka" };

            var result = await authLogic.CreateUser(user);

            Assert.True(result.Succeeded);
            Assert.That(users.Count == 4);
        }

        [Test]
        public async Task CreateUser_FailedCreation_EmailAlreadyInRepo()
        {
            AuthLogic authLogic = new(this.userManager.Object, this.roleManager.Object, this.config);

            UserDTO user = new UserDTO()
            {
                Username = "fogvaratartottGyik",
                Email = "sanyesz@gmail.com",
                Password = "miAR3dV2Sf0s",
                Firstname = "Kronikus",
                Lastname = "VeszettMacska",
                Picture = "Keka"
            };

            var result = await authLogic.CreateUser(user);

            Assert.True(!result.Succeeded);
            Assert.That(users.Count == 3);
        }

        [Test]
        public async Task DeleteUser_SucceededDeletion_OnRecentlyCreatedUserAndByName()
        {
            AuthLogic authLogic = new(this.userManager.Object, this.roleManager.Object, this.config);

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

        [Test]
        public async Task UpdateUser_SucceededUpdate_ExistingUser()
        {
            AuthLogic authLogic = new(this.userManager.Object, this.roleManager.Object, this.config);

            UserDTO user = new UserDTO()
            {
                Username = "fogvaratartottGyik",
                Email = "maszkosfutocsiga@gmail.com",
                Password = "miAR3dV2Sf0s",
                Firstname = "Kronikus",
                Lastname = "VeszettMacska",
                Picture = "Keka"
            };

            string helper = users[0].UserName;

            var result = await authLogic.UpdateUser(helper, user);

            Assert.That(result.Succeeded);
        }

        private Mock<UserManager<WmanUser>> GetUserManager(List<WmanUser> ls)
        {
            var store = new Mock<IUserStore<WmanUser>>();
            var mgr = new Mock<UserManager<WmanUser>>(store.Object, null, new PasswordHasher<WmanUser>(), null, null, null, null, null, null);

            mgr.Setup(x => x.DeleteAsync(It.IsAny<WmanUser>())).ReturnsAsync(IdentityResult.Success)
                .Callback<WmanUser>((x) => ls.Remove(x));
            mgr.Setup(x => x.CreateAsync(It.IsAny<WmanUser>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success)
                .Callback<WmanUser, string>((x, y) => ls.Add(x));
            mgr.Setup(x => x.UpdateAsync(It.IsAny<WmanUser>())).ReturnsAsync(IdentityResult.Success);

            var mock = users.AsQueryable().BuildMock();
            
            mgr.Setup(x => x.Users).Returns(mock.Object);
            mgr.Setup(x => x.CheckPasswordAsync(It.IsAny<WmanUser>(), It.IsAny<string>())).ReturnsAsync(true);
            mgr.Setup(x => x.GetRolesAsync(It.IsAny<WmanUser>())).ReturnsAsync(new List<string> { "Admin", "Manager", "Worker"});
            
            return mgr;
        }

        private Mock<RoleManager<IdentityRole>> GetMockRoleManager()
        {
            var roleStore = new Mock<IRoleStore<IdentityRole>>();

            var role = new Mock<RoleManager<IdentityRole>>(
                         roleStore.Object, null, null, null, null);

            return role;
        }

        private IConfiguration GetConfiguration()
        {
            var cfg = new Dictionary<string, string>
            {
                { "SigningKey", "TestValueAJKSJDJ2732636auhsdnh"}
            };

            IConfiguration cfgBuild = new ConfigurationBuilder().AddInMemoryCollection(cfg).Build();

            return cfgBuild;
        }
    }
}
