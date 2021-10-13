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
using Wman.Test.Builders;

namespace Wman.Test
{
    public class AuthLogicTests
    {
        
        private Mock<UserManager<WmanUser>> userManager;
        private Mock<RoleManager<WmanRole>> roleManager;
        private IConfiguration config;

        private List<WmanUser> users;

        [SetUp]
        public void Setup()
        {
            this.users = AuthLogicBuilder.GetWmanUsers();

            this.userManager = AuthLogicBuilder.GetUserManager(users);
            this.roleManager = AuthLogicBuilder.GetMockRoleManager();
            this.config = AuthLogicBuilder.GetConfiguration();
        }

        [Test]
        public async Task LoginUser_TokenGranted_OnNewlyCreatedUser()
        {
            AuthLogic authLogic = new(this.userManager.Object, this.roleManager.Object, this.config);

            RegisterDTO user = new RegisterDTO()
            {
                Username = "fogvaratartottGyik",
                Email = "maszkosfutocsiga@gmail.com",
                Password = "miAR3dV2Sf0s",
                Firstname = "Kronikus",
                Lastname = "VeszettMacska",
            };

            var akarmi = await authLogic.CreateUser(user);

            LoginDTO model = new LoginDTO() { LoginName=user.Email, Password=user.Password };

            var result = await authLogic.LoginUser(model);

            Assert.That(result.Token != null);

            this.userManager.Verify(x => x.FindByNameAsync(model.LoginName), Times.Never);
            this.userManager.Verify(x => x.CheckPasswordAsync(It.IsAny<WmanUser>(), model.Password), Times.Once);
            this.userManager.Verify(x => x.GetRolesAsync(It.IsAny<WmanUser>()), Times.Once);
        }

        [Test]
        public async Task GetOneUser()
        {
            AuthLogic authLogic = new(this.userManager.Object, this.roleManager.Object, this.config);

            var result = await authLogic.GetOneUser(users[0].UserName);

            Assert.AreEqual(result.UserName, users[0].UserName);

            this.userManager.Verify(x => x.Users, Times.Once);
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

            RegisterDTO user = new RegisterDTO()
            {
                Username = "fogvaratartottGyik",
                Email = "maszkosfutocsiga@gmail.com",
                Password = "miAR3dV2Sf0s",
                Firstname = "Kronikus",
                Lastname = "VeszettMacska",
            };

            var result = await authLogic.CreateUser(user);

            Assert.True(result.Succeeded);
            Assert.That(users.Count == 4);

            this.userManager.Verify(x => x.CreateAsync(It.IsAny<WmanUser>(), It.IsAny<string>()), Times.Once);
            this.userManager.Verify(x => x.AddToRoleAsync(It.IsAny<WmanUser>(), It.IsAny<string>()), Times.Once);
        }

        [Test]
        public async Task CreateUser_FailedCreation_EmailAlreadyInRepo()
        {
            AuthLogic authLogic = new(this.userManager.Object, this.roleManager.Object, this.config);

            RegisterDTO user = new RegisterDTO()
            {
                Username = "fogvaratartottGyik",
                Email = "sanyesz@gmail.com",
                Password = "miAR3dV2Sf0s",
                Firstname = "Kronikus",
                Lastname = "VeszettMacska",
            };

            var result = await authLogic.CreateUser(user);

            Assert.True(!result.Succeeded);
            Assert.That(users.Count == 3);

            this.userManager.Verify(x => x.CreateAsync(It.IsAny<WmanUser>(), It.IsAny<string>()), Times.Never);
            this.userManager.Verify(x => x.AddToRoleAsync(It.IsAny<WmanUser>(), It.IsAny<string>()), Times.Never);
        }

        [Test]
        public async Task DeleteUser_SucceededDeletion_OnRecentlyCreatedUserAndByName()
        {
            AuthLogic authLogic = new(this.userManager.Object, this.roleManager.Object, this.config);

            RegisterDTO user = new RegisterDTO()
            {
                Username = "fogvaratartottGyik",
                Email = "maszkosfutocsiga@gmail.com",
                Password = "miAR3dV2Sf0s",
                Firstname = "Kronikus",
                Lastname = "VeszettMacska",
            };

            await authLogic.CreateUser(user);

            //we have 4 users in the repo right now
            var result = await authLogic.DeleteUser(user.Username);
            var result2 = await authLogic.DeleteUser("ArnoldBalValla");

            //Should be 2 users in the repo after 2 deletions
            Assert.True(result.Succeeded);
            Assert.AreEqual(2, users.Count);

            this.userManager.Verify(x => x.DeleteAsync(It.IsAny<WmanUser>()), Times.Exactly(2));
            this.userManager.Verify(x => x.CreateAsync(It.IsAny<WmanUser>(), It.IsAny<string>()), Times.Once);
            this.userManager.Verify(x => x.AddToRoleAsync(It.IsAny<WmanUser>(), It.IsAny<string>()), Times.Once);
        }

        [Test]
        public async Task RemoveUserFromRole_RemovesSuccessfully()
        {
            AuthLogic authLogic = new(this.userManager.Object, this.roleManager.Object, this.config);

            var result = await authLogic.RemoveUserFromRole(users[2].UserName, "Debug");

            Assert.That(result == "Success");

            this.userManager.Verify(x => x.FindByNameAsync(users[2].UserName), Times.Once);
            this.userManager.Verify(x => x.RemoveFromRoleAsync(users[2], "Debug"), Times.Once);
        }

        [Test]
        public async Task GetAllRolesOfUser_Returns4Roles()
        {
            AuthLogic authLogic = new(this.userManager.Object, this.roleManager.Object, this.config);

            var result = await authLogic.GetAllRolesOfUser(users[2]);

            Assert.AreEqual(4, result.Count());

            this.userManager.Verify(x => x.GetRolesAsync(It.IsAny<WmanUser>()), Times.Once);
        }

        [Test]
        public async Task GetAllUsersOfRole_Returns3Users()
        {
            AuthLogic authLogic = new(this.userManager.Object, this.roleManager.Object, this.config);

            var result = await authLogic.GetAllUsersOfRole("Test");

            Assert.AreEqual(3, result.Count());
            this.userManager.Verify(x => x.GetUsersInRoleAsync(It.IsAny<string>()), Times.Once);
        }

        [Test]
        public async Task HasRole_And_HasRoleByName_ReturnsSuccessful()
        {
            AuthLogic authLogic = new(this.userManager.Object, this.roleManager.Object, this.config);

            var result = await authLogic.HasRole(users[2], "Debug");
            var resultByName = await authLogic.HasRoleByName(users[2].UserName, "Debug");

            Assert.That(result && resultByName);

            this.userManager.Verify(x => x.FindByNameAsync(It.IsAny<string>()), Times.Once);
            this.userManager.Verify(x => x.IsInRoleAsync(It.IsAny<WmanUser>(), It.IsAny<string>()), Times.Exactly(2));
        }

        [Test]
        public async Task RoleCreationAndAssignment_Successful()
        {
            AuthLogic authLogic = new(this.userManager.Object, this.roleManager.Object, this.config);

            var createdRole = await authLogic.CreateRole("Test01");

            List<string> roleTest = new List<string>() { "Test01" };
            var addRole = await authLogic.AssignRolesToUser(users[2], roleTest);

            Assert.That(createdRole);
            Assert.That(addRole);

            this.roleManager.Verify(x => x.CreateAsync(It.IsAny<WmanRole>()), Times.Once);
            this.roleManager.Verify(x => x.FindByNameAsync(It.IsAny<string>()), Times.Once);

            this.userManager.Verify(x => x.AddToRolesAsync(It.IsAny<WmanUser>(),
                It.IsAny<IEnumerable<string>>()), Times.Once);
        }

        [Test]
        public async Task UpdateUser_SucceededUpdate_ExistingUser()
        {
            AuthLogic authLogic = new(this.userManager.Object, this.roleManager.Object, this.config);

            UserDTO user = new UserDTO()
            {
                Username = "fogvaratartottGyik",
                Email = "maszkosfutocsiga@gmail.com",
                Firstname = "Kronikus",
                Lastname = "VeszettMacska",
            };

            string helper = users[0].UserName;

            var result = await authLogic.UpdateUser(helper, "miAR3dV2Sf0s", user);

            Assert.That(result.Succeeded);

            this.userManager.Verify(x => x.UpdateAsync(It.IsAny<WmanUser>()), Times.Once);
            this.userManager.Verify(x => x.Users, Times.Once);
        }

        [Test]
        public async Task SwitchRoleOfUser()
        {
            AuthLogic authLogic = new(this.userManager.Object, this.roleManager.Object, this.config);

            var result = await authLogic.SwitchRoleOfUser(users[2].UserName, "Debug");

            Assert.True(result);

            this.userManager.Verify(x => x.AddToRoleAsync(It.IsAny<WmanUser>(),It.IsAny<string>()), Times.Once);
            this.userManager.Verify(x => x.RemoveFromRoleAsync(It.IsAny<WmanUser>(),It.IsAny<string>()), Times.AtLeastOnce);
            this.userManager.Verify(x => x.FindByNameAsync(It.IsAny<string>()), Times.AtLeastOnce);
        }
    }
}
