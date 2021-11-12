using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Wman.Data.DB_Models;
using Wman.Test.Builders;

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
            this.userManager = UserManagerBuilder.GetUserManager(users);

            this.mapper = MapperBuilder.GetMapper();
        }
    }
}
