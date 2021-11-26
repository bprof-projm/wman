using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Moq;
using System.Collections.Generic;
using Wman.Data.DB_Models;

namespace Wman.Test.Builders.LogicBuilders
{
    public class AuthLogicBuilder
    {
        public static Mock<RoleManager<IdentityRole<int>>> GetMockRoleManager()
        {
            var roleStore = new Mock<IRoleStore<IdentityRole<int>>>();

            var role = new Mock<RoleManager<IdentityRole<int>>>(
                         roleStore.Object, null, null, null, null);

            role.Setup(x => x.RoleExistsAsync(It.IsAny<string>())).ReturnsAsync(true);
            role.Setup(x => x.CreateAsync(It.IsAny<IdentityRole<int>>())).ReturnsAsync(IdentityResult.Success);
            role.Setup(x => x.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(new IdentityRole<int>
            {
                Id = 0,
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
    }
}
