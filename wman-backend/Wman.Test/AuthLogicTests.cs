using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
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

            users.Add(new WmanUser { Id = 0, FirstName = "Sanyi", LastName = "Hurutos", Picture="asd" });
            users.Add(new WmanUser { Id = 1, FirstName = "Hamis", LastName = "Süni", Picture = "asd" });
            users.Add(new WmanUser { Id = 2, FirstName = "Medve", LastName = "Obudai", Picture = "asd" });

            userManager = GetUserManager(users);
            roleManager = GetMockRoleManager();
            config = new Mock<IConfiguration>();
        }

        [Test]
        public async Task CreateUser_SucceededCreation()
        {
            AuthLogic authLogic = new(this.userManager.Object, this.roleManager.Object, config.Object);

            //UserDTO user = new UserDTO() { Username }

            //var result = await authLogic.CreateUser(user);

            //Assert.That(users.Count == 4);
        }

        public static Mock<UserManager<WmanUser>> GetUserManager(List<WmanUser> ls)
        {
            var store = new Mock<IUserStore<WmanUser>>();
            var mgr = new Mock<UserManager<WmanUser>>(store.Object, null, null, null, null, null, null, null, null);
            mgr.Object.UserValidators.Add(new UserValidator<WmanUser>());
            mgr.Object.PasswordValidators.Add(new PasswordValidator<WmanUser>());

            mgr.Setup(x => x.DeleteAsync(It.IsAny<WmanUser>())).ReturnsAsync(IdentityResult.Success);
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
