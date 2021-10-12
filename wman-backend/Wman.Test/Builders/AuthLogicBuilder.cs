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

namespace Wman.Test.Builders
{
    public class AuthLogicBuilder
    {
        public static Mock<UserManager<WmanUser>> GetUserManager(List<WmanUser> userList)
        {
            var store = new Mock<IUserStore<WmanUser>>();
            var mgr = new Mock<UserManager<WmanUser>>(store.Object, null, new PasswordHasher<WmanUser>(), null, null, null, null, null, null);
            var mock = userList.AsQueryable().BuildMock();

            mgr.Setup(x => x.Users).Returns(mock.Object);
            mgr.Setup(x => x.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(userList[2]);
            mgr.Setup(x => x.FindByNameAsync(It.IsAny<string>())).ReturnsAsync(userList[2]);

            mgr.Setup(x => x.DeleteAsync(It.IsAny<WmanUser>())).ReturnsAsync(IdentityResult.Success)
                .Callback<WmanUser>((x) => userList.Remove(x));
            mgr.Setup(x => x.CreateAsync(It.IsAny<WmanUser>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success)
                .Callback<WmanUser, string>((x, y) => userList.Add(x));
            mgr.Setup(x => x.UpdateAsync(It.IsAny<WmanUser>())).ReturnsAsync(IdentityResult.Success);

            mgr.Setup(x => x.CheckPasswordAsync(It.IsAny<WmanUser>(), It.IsAny<string>())).ReturnsAsync(true);

            mgr.Setup(x => x.IsInRoleAsync(It.IsAny<WmanUser>(), It.IsAny<string>())).ReturnsAsync(true);
            mgr.Setup(x => x.GetRolesAsync(It.IsAny<WmanUser>())).ReturnsAsync(new List<string> {
                "Admin", "Debug", "Manager", "Worker" });

            mgr.Setup(x => x.GetUsersInRoleAsync(It.IsAny<string>())).ReturnsAsync(userList);

            return mgr;
        }

        public static Mock<RoleManager<IdentityRole>> GetMockRoleManager()
        {
            var roleStore = new Mock<IRoleStore<IdentityRole>>();

            var role = new Mock<RoleManager<IdentityRole>>(
                         roleStore.Object, null, null, null, null);

            role.Setup(x => x.CreateAsync(It.IsAny<IdentityRole>())).ReturnsAsync(IdentityResult.Success);
            role.Setup(s => s.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(new IdentityRole
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Test03",
                NormalizedName = "TEST03"
            });
            return role;
        }

        public static IConfiguration GetConfiguration()
        {
            var cfg = new Dictionary<string, string>
            {
                { "SigningKey", "TestValueAJKSJDJ2732636auhsdnh"}
            };

            IConfiguration cfgBuild = new ConfigurationBuilder().AddInMemoryCollection(cfg).Build();

            return cfgBuild;
        }

        public static List<WmanUser> GetWmanUsers()
        {
            List<WmanUser> users = new List<WmanUser>();

            users.Add(new WmanUser
            {
                Id = 0,
                UserName = "LmaoRandom",
                Email = "sanyesz@gmail.com",
                FirstName = "Sanyi",
                LastName = "Hurutos",
                Picture = "asd",
                SecurityStamp = Guid.NewGuid().ToString()
            });
            users.Add(new WmanUser
            {
                Id = 1,
                UserName = "TigoleBitties",
                Email = "foksok@gmail.com",
                FirstName = "Hamis",
                LastName = "Süni",
                Picture = "asd",
                SecurityStamp = Guid.NewGuid().ToString()
            });
            users.Add(new WmanUser
            {
                Id = 2,
                UserName = "ArnoldBalValla",
                Email = "zsoltas@gmail.com",
                FirstName = "Medve",
                LastName = "Obudai",
                Picture = "asd",
                SecurityStamp = Guid.NewGuid().ToString()
            });

            return users;
        }
    }
}
