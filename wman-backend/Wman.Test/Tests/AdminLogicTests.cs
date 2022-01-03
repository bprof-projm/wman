using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Wman.Data.DB_Models;
using Wman.Logic.Classes;
using Wman.Logic.DTO_Models;
using Wman.Logic.Interfaces;
using Wman.Logic.Services;
using Wman.Repository.Interfaces;
using Wman.Test.Builders;
using Wman.Test.Builders.LogicBuilders;

namespace Wman.Test.Tests
{
    public class AdminLogicTests
    {
        private Mock<UserManager<WmanUser>> userManager;
        private IMapper mapper;

        private IPhotoLogic photoLogic;
        private List<WmanUser> users;

        [SetUp]
        public void Setup()
        {
            this.users = UserManagerBuilder.GetWmanUsers();
            this.userManager = UserManagerBuilder.GetUserManagerWithFalseRoleCheck(this.users); 
            this.mapper = MapperBuilder.GetMapper();

            
            this.photoLogic = AdminLogicBuilder.PhotoLogicFactory(this.userManager.Object,this.mapper);
        }

        [Test]
        public async Task CreateWorkForce_FailedCreation_EmailAlreadyInRepo()
        {
            //Arrange
            AdminLogic adminLogic = new(this.userManager.Object, this.photoLogic, this.mapper);
            RegisterDTO user = new()
            {
                Username = "fogvaratartottGyik",
                Email = "sanyesz@gmail.com",
                Password = "miAR35V2S50s",
                Firstname = "Kronikus",
                Lastname = "VeszettMacska",
                PhoneNumber = "0690123456",
                Role = "Worker"
            };

            //Act
            async Task testDelegate() => await adminLogic.CreateWorkforce(user);

            //Assert
            Assert.ThrowsAsync<InvalidOperationException>(testDelegate);

            this.userManager.Verify(x => x.Users, Times.Once);
            this.userManager.Verify(x => x.CreateAsync(It.IsAny<WmanUser>(), It.IsAny<string>()), Times.Never);
            this.userManager.Verify(x => x.AddToRoleAsync(It.IsAny<WmanUser>(), It.IsAny<string>()), Times.Never);
        }

        [Test]
        public async Task CreateWorkForce_CreatesWorkerProperly()
        {
            //Arrange
            AdminLogic adminLogic = new(this.userManager.Object, this.photoLogic, this.mapper);
            RegisterDTO user = new()
            {
                Username = "fogvaratartottGyik",
                Email = "maszkosfutocsiga@gmail.com",
                Password = "miAR35V2S50s",
                Firstname = "Kronikus",
                Lastname = "VeszettMacska",
                PhoneNumber = "0690123456",
                Role = "Worker"
            };

            //Act
            var result = await adminLogic.CreateWorkforce(user);
            
            //Assert
            Assert.True(result.Succeeded);
            Assert.That(users.Count == 4);

            this.userManager.Verify(x => x.Users, Times.Once);
            this.userManager.Verify(x => x.CreateAsync(It.IsAny<WmanUser>(), It.IsAny<string>()), Times.Once);
            this.userManager.Verify(x => x.AddToRoleAsync(It.IsAny<WmanUser>(), It.IsAny<string>()), Times.Once);
        }

        [Test]
        public async Task DeleteWorkforce_SucceededDeletion_OnRecentlyCreatedUserAndByName()
        {
            //Arrange
            AdminLogic adminLogic = new(this.userManager.Object, this.photoLogic, this.mapper);
            RegisterDTO user = new()
            {
                Username = "Test012",
                Email = "maszkosfutocsiga@gmail.com",
                Password = "miAR35V2S50s",
                Firstname = "Kronikus",
                Lastname = "VeszettMacska",
                PhoneNumber = "0690123456",
                Role = "Worker"
            };

            //Act
            await adminLogic.CreateWorkforce(user);

            //we have 4 users in the repo right now
            var result = await adminLogic.DeleteWorkforce(user.Username);
            var result2 = await adminLogic.DeleteWorkforce("ArnoldBalValla");

            //Assert
            //Should be 2 users in the repo after 2 deletions
            Assert.True(result.Succeeded);
            Assert.AreEqual(2, users.Count);

            //CreateWorkforce
            this.userManager.Verify(x => x.Users, Times.Exactly(3)); //3 because of 2 DeleteUser calls and 1 CreateWorker
            this.userManager.Verify(x => x.CreateAsync(It.IsAny<WmanUser>(), It.IsAny<string>()), Times.Once);
            this.userManager.Verify(x => x.AddToRoleAsync(It.IsAny<WmanUser>(), It.IsAny<string>()), Times.Once);
            //DeleteWorkforce
            this.userManager.Verify(x => x.DeleteAsync(It.IsAny<WmanUser>()), Times.Exactly(2));
        }

        [Test]
        public async Task UpdateWorkforce_UpdateExistingUser_SuccessfulOperation()
        {
            //Arrange
            AdminLogic adminLogic = new(this.userManager.Object, this.photoLogic, this.mapper);

            WorkerModifyDTO user = new()
            {
                Email = "maszkosfutocsiga@gmail.com",
                Firstname = "Kronikus",
                Lastname = "VeszettMacska",
                PhoneNumber = "0690123455",
                Role="Worker"
            };

            string helper = users[0].UserName;

            //Act
            var result = await adminLogic.UpdateWorkforce(helper,  user);

            //Assert
            Assert.That(result.Succeeded);

            this.userManager.Verify(x => x.Users, Times.Once);
            this.userManager.Verify(x => x.UpdateAsync(It.IsAny<WmanUser>()), Times.Once);
            this.userManager.Verify(x => x.IsInRoleAsync(It.IsAny<WmanUser>(), It.IsAny<string>()), Times.Once);
            this.userManager.Verify(x => x.GetRolesAsync(It.IsAny<WmanUser>()), Times.Once);
            this.userManager.Verify(x => x.RemoveFromRoleAsync(It.IsAny<WmanUser>(), It.IsAny<string>()), Times.Once);
            this.userManager.Verify(x => x.AddToRoleAsync(It.IsAny<WmanUser>(), It.IsAny<string>()), Times.Once);
        }
    }
}
