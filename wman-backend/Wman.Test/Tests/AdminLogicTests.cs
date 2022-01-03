using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Moq;
using NUnit.Framework;
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
            this.userManager = UserManagerBuilder.GetUserManager(this.users);
            this.mapper = MapperBuilder.GetMapper();

            this.photoLogic = AdminLogicBuilder.PhotoLogicFactory(this.userManager.Object,this.mapper);
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
                Password = "miAR3dV2Sf0s",
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
    }
}
