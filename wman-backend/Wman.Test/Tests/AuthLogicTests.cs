using AutoMapper;
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
            this.config = AspConfigurationBuilder.GetConfiguration();
            this.mapper = MapperBuilder.GetMapper();
        }

        [Test]
        public async Task LoginUser_TokenGranted()
        {
            //Arrange
            AuthLogic authLogic = new(this.userManager.Object, this.roleManager.Object, this.config, this.mapper);

            LoginDTO model = new() { LoginName = users[0].Email, Password = "random" };

            //Act
            var result = await authLogic.LoginUser(model);

            //Assert
            Assert.That(result.Token != null);
            
            this.userManager.Verify(x => x.Users, Times.Exactly(1)); //2 because of 1 CreatWorker and 1 LoginUser call
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
            Assert.AreEqual(users.Count, result.Count());

            this.userManager.Verify(x => x.Users, Times.Once);
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
            Assert.AreEqual(3, result.Count);

            this.userManager.Verify(x => x.GetUsersInRoleAsync(It.IsAny<string>()), Times.Once);
            this.roleManager.Verify(x => x.RoleExistsAsync(It.IsAny<string>()), Times.Once);
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
        public async Task UpdateUser_SucceededUpdate_ExistingUser()
        {
            //Arrange
            AuthLogic authLogic = new(this.userManager.Object, this.roleManager.Object, this.config, this.mapper);

            UserDTO user = new()
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
