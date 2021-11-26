using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Wman.Data.DB_Models;
using Wman.Logic.Classes;
using Wman.Logic.DTO_Models;
using Wman.Test.Builders;
using Wman.Test.Builders.LogicBuilders;

namespace Wman.Test.Tests
{
    public class AuthLogicTests
    {
        private Mock<UserManager<WmanUser>> userManager;
        private Mock<RoleManager<IdentityRole<int>>> roleManager;
        
        private IConfiguration config;
        private IMapper mapper;

        private List<WmanUser> users;

        [SetUp]
        public void Setup()
        {
            this.users = UserManagerBuilder.GetWmanUsers();
            this.userManager = UserManagerBuilder.GetUserManager(users);

            this.roleManager = AuthLogicBuilder.GetMockRoleManager();
            this.config = AuthLogicBuilder.GetConfiguration();
            this.mapper = MapperBuilder.GetMapper();
        }

        [Test]
        public async Task LoginUser_TokenGranted_OnNewlyCreatedUser()
        {
            //Arrange
            AuthLogic authLogic = new(this.userManager.Object, this.roleManager.Object, this.config, this.mapper);

            RegisterDTO user = new RegisterDTO()
            {
                Username = "fogvaratartottGyik",
                Email = "maszkosfutocsiga@gmail.com",
                Password = "miAR3dV2Sf0s",
                Firstname = "Kronikus",
                Lastname = "VeszettMacska",
            };

            LoginDTO model = new LoginDTO() { LoginName = user.Email, Password = user.Password };

            //Act
            var akarmi = await authLogic.CreateWorker(user);
            var result = await authLogic.LoginUser(model);

            //Assert
            Assert.That(result.Token != null);
            
            //CreateWorker
            this.userManager.Verify(x => x.Users, Times.Exactly(2)); //2 because of 1 CreatWorker and 1 LoginUser call
            this.userManager.Verify(x => x.CreateAsync(It.IsAny<WmanUser>(), It.IsAny<string>()), Times.Once);
            this.userManager.Verify(x => x.AddToRoleAsync(It.IsAny<WmanUser>(), It.IsAny<string>()), Times.Once);
            //LoginUser
            this.userManager.Verify(x => x.FindByNameAsync(It.IsAny<string>()), Times.Never);
            this.userManager.Verify(x => x.CheckPasswordAsync(It.IsAny<WmanUser>(), It.IsAny<string>()), Times.Once);
            this.userManager.Verify(x => x.GetRolesAsync(It.IsAny<WmanUser>()), Times.Once);
        }

        [Test]
        public async Task GetOneUser()
        {
            //Arrange
            AuthLogic authLogic = new(this.userManager.Object, this.roleManager.Object, this.config, this.mapper);

            //Act
            var result = await authLogic.GetOneUser(users[0].UserName);

            //Assert
            Assert.AreEqual(result.Username, users[0].UserName);

            this.userManager.Verify(x => x.Users, Times.Once);
        }

        [Test]
        public async Task GetAllUsers_ReturnsRepoProperly()
        {
            //Arrange
            AuthLogic authLogic = new(this.userManager.Object, this.roleManager.Object, this.config, this.mapper);

            //Act
            var result = await authLogic.GetAllUsers();

            //Assert
            Assert.AreEqual(users.Count(), result.Count());

            this.userManager.Verify(x => x.Users, Times.Once);
        }

        [Test]
        public async Task CreateWorker_SucceededCreation()
        {
            //Arrange
            AuthLogic authLogic = new(this.userManager.Object, this.roleManager.Object, this.config, this.mapper);

            RegisterDTO user = new RegisterDTO()
            {
                Username = "fogvaratartottGyik",
                Email = "maszkosfutocsiga@gmail.com",
                Password = "miAR3dV2Sf0s",
                Firstname = "Kronikus",
                Lastname = "VeszettMacska",
            };

            //Act
            var result = await authLogic.CreateWorker(user);

            //Assert
            Assert.True(result.Succeeded);
            Assert.That(users.Count == 4);

            this.userManager.Verify(x => x.Users, Times.Once);
            this.userManager.Verify(x => x.CreateAsync(It.IsAny<WmanUser>(), It.IsAny<string>()), Times.Once);
            this.userManager.Verify(x => x.AddToRoleAsync(It.IsAny<WmanUser>(), It.IsAny<string>()), Times.Once);
        }

        [Test]
        public async Task CreateWorker_FailedCreation_EmailAlreadyInRepo()
        {
            //Arrange
            AuthLogic authLogic = new(this.userManager.Object, this.roleManager.Object, this.config, this.mapper);

            RegisterDTO user = new RegisterDTO()
            {
                Username = "fogvaratartottGyik",
                Email = "sanyesz@gmail.com",
                Password = "miAR3dV2Sf0s",
                Firstname = "Kronikus",
                Lastname = "VeszettMacska",
            };

            //Act
            var result = await authLogic.CreateWorker(user);

            //Assert
            Assert.True(!result.Succeeded);
            Assert.That(users.Count == 3);

            this.userManager.Verify(x => x.Users, Times.Once);
            this.userManager.Verify(x => x.CreateAsync(It.IsAny<WmanUser>(), It.IsAny<string>()), Times.Never);
            this.userManager.Verify(x => x.AddToRoleAsync(It.IsAny<WmanUser>(), It.IsAny<string>()), Times.Never);
        }

        [Test]
        public async Task DeleteUser_SucceededDeletion_OnRecentlyCreatedUserAndByName()
        {
            //Arrange
            AuthLogic authLogic = new(this.userManager.Object, this.roleManager.Object, this.config, this.mapper);

            RegisterDTO user = new RegisterDTO()
            {
                Username = "fogvaratartottGyik",
                Email = "maszkosfutocsiga@gmail.com",
                Password = "miAR3dV2Sf0s",
                Firstname = "Kronikus",
                Lastname = "VeszettMacska",
            };

            //Act
            await authLogic.CreateWorker(user);

            //we have 4 users in the repo right now
            var result = await authLogic.DeleteUser(user.Username);
            var result2 = await authLogic.DeleteUser("ArnoldBalValla");
            
            //Assert
            //Should be 2 users in the repo after 2 deletions
            Assert.True(result.Succeeded);
            Assert.AreEqual(2, users.Count);
            
            //CreateWorker
            this.userManager.Verify(x => x.Users, Times.Exactly(3)); //3 because of 2 DeleteUser calls and 1 CreateWorker
            this.userManager.Verify(x => x.CreateAsync(It.IsAny<WmanUser>(), It.IsAny<string>()), Times.Once);
            this.userManager.Verify(x => x.AddToRoleAsync(It.IsAny<WmanUser>(), It.IsAny<string>()), Times.Once);
            //DeleteUser
            this.userManager.Verify(x => x.DeleteAsync(It.IsAny<WmanUser>()), Times.Exactly(2));
        }

        [Test]
        public async Task GetAllRolesOfUser_Returns4Roles()
        {
            //Arrange
            AuthLogic authLogic = new(this.userManager.Object, this.roleManager.Object, this.config, this.mapper);

            //Act
            var result = await authLogic.GetAllRolesOfUser(users[2].UserName);

            //Assert
            Assert.AreEqual(4, result.Count());
            
            this.userManager.Verify(x => x.FindByNameAsync(It.IsAny<string>()), Times.Once);
            this.userManager.Verify(x => x.GetRolesAsync(It.IsAny<WmanUser>()), Times.Once);
        }

        [Test]
        public async Task GetAllUsersOfRole_Returns3Users()
        {
            //Arrange
            AuthLogic authLogic = new(this.userManager.Object, this.roleManager.Object, this.config, this.mapper);

            //Act
            var result = await authLogic.GetAllUsersOfRole("Test");

            //Assert
            Assert.AreEqual(3, result.Count());

            this.userManager.Verify(x => x.GetUsersInRoleAsync(It.IsAny<string>()), Times.Once);
            this.roleManager.Verify(x => x.RoleExistsAsync(It.IsAny<string>()), Times.Once);
        }

        [Test]
        public async Task UpdateUser_SucceededUpdate_ExistingUser()
        {
            //Arrange
            AuthLogic authLogic = new(this.userManager.Object, this.roleManager.Object, this.config, this.mapper);

            UserDTO user = new UserDTO()
            {
                Username = "fogvaratartottGyik",
                Email = "maszkosfutocsiga@gmail.com",
                Firstname = "Kronikus",
                Lastname = "VeszettMacska",
            };

            string helper = users[0].UserName;

            //Act
            var result = await authLogic.UpdateUser(helper, "miAR3dV2Sf0s", user);

            //Assert
            Assert.That(result.Succeeded);

            this.userManager.Verify(x => x.Users, Times.Once);
            this.userManager.Verify(x => x.UpdateAsync(It.IsAny<WmanUser>()), Times.Once);
        }

        [Test]
        public async Task SetRoleOfUser_ExistingUser_SuccessfulOperation()
        {
            //Arrange
            AuthLogic authLogic = new(this.userManager.Object, this.roleManager.Object, this.config, this.mapper);

            //Act
            var result = authLogic.SetRoleOfUser(users[2].UserName, "Debug");

            //Assert
            Assert.True(result.IsCompleted);

            this.userManager.Verify(x => x.FindByNameAsync(It.IsAny<string>()), Times.Once);
            this.userManager.Verify(x => x.GetRolesAsync(It.IsAny<WmanUser>()), Times.Once);
            this.userManager.Verify(x => x.AddToRoleAsync(It.IsAny<WmanUser>(), It.IsAny<string>()), Times.Once);
        }

        /*
         TEST CASES FOR DEPRECATED METHODS

        [Test]
        public async Task RemoveUserFromRole_RemovesSuccessfully()
        {
            //Arrange
            AuthLogic authLogic = new(this.userManager.Object, this.roleManager.Object, this.config, this.mapper);

            //Act
            var result = await authLogic.RemoveUserFromRole(users[2].UserName, "Debug");

            //Assert
            Assert.That(result == "Success");

            this.userManager.Verify(x => x.FindByNameAsync(users[2].UserName), Times.Once);
            this.userManager.Verify(x => x.RemoveFromRoleAsync(users[2], "Debug"), Times.Once);
        }
        
        [Test]
        public async Task HasRole_And_HasRoleByName_ReturnsSuccessful()
        {
            //Arrange
            AuthLogic authLogic = new(this.userManager.Object, this.roleManager.Object, this.config, this.mapper);

            //Act
            var result = await authLogic.HasRole(users[2], "Debug");
            var resultByName = await authLogic.HasRoleByName(users[2].UserName, "Debug");

            //Assert
            Assert.That(result && resultByName);

            this.userManager.Verify(x => x.FindByNameAsync(It.IsAny<string>()), Times.Once);
            this.userManager.Verify(x => x.IsInRoleAsync(It.IsAny<WmanUser>(), It.IsAny<string>()), Times.Exactly(2));
        }

        [Test]
        public async Task RoleCreationAndAssignment_Successful()
        {
            //Arrange
            AuthLogic authLogic = new(this.userManager.Object, this.roleManager.Object, this.config, this.mapper);
            List<string> roleTest = new List<string>() { "Test01" };

            //Act
            var createdRole = await authLogic.CreateRole("Test01");
            var addRole = await authLogic.AssignRolesToUser(users[2], roleTest);

            //Assert
            Assert.That(createdRole);
            Assert.That(addRole);

            this.roleManager.Verify(x => x.CreateAsync(It.IsAny<WmanRole>()), Times.Once);
            this.roleManager.Verify(x => x.FindByNameAsync(It.IsAny<string>()), Times.Once);

            this.userManager.Verify(x => x.AddToRolesAsync(It.IsAny<WmanUser>(),
                It.IsAny<IEnumerable<string>>()), Times.Once);
        }

        [Test]
        public async Task SwitchRoleOfUser_ExistingUser_SuccessfulOperation()
        {
            //Arrange
            AuthLogic authLogic = new(this.userManager.Object, this.roleManager.Object, this.config, this.mapper);

            //Act
            var result = await authLogic.SwitchRoleOfUser(users[2].UserName, "Debug");

            //Assert
            Assert.True(result);

            this.userManager.Verify(x => x.AddToRoleAsync(It.IsAny<WmanUser>(),It.IsAny<string>()), Times.Once);
            this.userManager.Verify(x => x.RemoveFromRoleAsync(It.IsAny<WmanUser>(),It.IsAny<string>()), Times.AtLeastOnce);
            this.userManager.Verify(x => x.FindByNameAsync(It.IsAny<string>()), Times.AtLeastOnce);
        }*/
    }
}
