using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Wman.Data.DB_Models;
using Wman.Logic.Classes;
using Wman.Test.Builders;
using Wman.Test.Builders.LogicBuilders;

namespace Wman.Test.Tests
{
    public class UserLogicTests
    {
        private Mock<UserManager<WmanUser>> userManager;
        private IMapper mapper;

        private List<WmanUser> users;

        [SetUp]
        public void SetUp()
        {
            this.users = UserManagerBuilder.GetWmanUsers();
            UserLogicBuilder.AssignWorkEvents(this.users);
            this.userManager = UserManagerBuilder.GetUserManager(this.users);

            this.mapper = MapperBuilder.GetMapper();
        }

        [Test]
        public async Task GetWorkLoads_ReturnsRepoProperly_SuccessfulOperation()
        {
            //Arrange
            UserLogic userLogic = new (this.userManager.Object, this.mapper);

            //Act
            var call = await userLogic.GetWorkLoads();

            //Assert
            Assert.That(call.ElementAt(2).Username == users[2].UserName);
            this.userManager.Verify(x => x.Users, Times.Once);
            this.userManager.Verify(x => x.IsInRoleAsync(It.IsAny<WmanUser>(), It.IsAny<string>()), Times.Exactly(users.Count()));
        }
    }
}
