using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Wman.Data.DB_Models;
using Wman.Logic.Classes;
using Wman.Logic.DTO_Models;
using Wman.Logic.Services;
using Wman.Repository.Interfaces;
using Wman.Test.Builders;
using Wman.Test.Builders.LogicBuilders;

namespace Wman.Test.Tests
{
    class PhotoLogicTests
    {
        private IMapper mapper;
        private Mock<IPhotoService> photoService;

        private Mock<UserManager<WmanUser>> userManager;
        private List<WmanUser> users;

        private Mock<IPicturesRepo> picturesRepo;
        private List<Pictures> pictureList;


        [SetUp]
        public void SetUp()
        {
            this.users = UserManagerBuilder.GetWmanUsers();
            this.userManager = UserManagerBuilder.GetUserManager(users);

            this.mapper = MapperBuilder.GetMapper();

            this.pictureList = PhotoLogicBuilder.GetPictures();
            this.picturesRepo = PhotoLogicBuilder.GetPicturesRepo(pictureList);
            this.photoService = PhotoLogicBuilder.GetPhotoService();
        }

    }
}
