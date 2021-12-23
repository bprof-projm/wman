using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Identity;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;
using Wman.Data.DB_Models;
using Wman.Logic.Classes;
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

        [Test]
        public async Task AddProfilePhoto_AddNewPhoto_SuccessfulOperation()
        {
            //Arrange
            PhotoLogic photoLogic = new(this.photoService.Object, this.userManager.Object, this.picturesRepo.Object, this.mapper);
            IFormFile file = PhotoLogicBuilder.GetFormFile();

            //Act
            var call = await photoLogic.AddProfilePhoto(users[0].UserName, file);

            //Assert
            this.photoService.Verify(x => x.AddProfilePhotoAsync(It.IsAny<FormFile>()), Times.Once);
            this.picturesRepo.Verify(x => x.Add(It.IsAny<Pictures>()), Times.Once);
        }
    }
}
