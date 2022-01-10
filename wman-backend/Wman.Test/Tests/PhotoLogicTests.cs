using AutoMapper;
using Microsoft.AspNetCore.Http;
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
        private IFormFile file;
        private IMapper mapper;
        private Mock<IPhotoService> photoService;

        private Mock<UserManager<WmanUser>> userManager;
        private List<WmanUser> users;

        private Mock<IPicturesRepo> picturesRepo;
        private List<Pictures> pictureList;

        private List<ProofOfWork> proofList;
        private Mock<IProofOfWorkRepo> proofOfWorkRepo;

        private Mock<IWorkEventRepo> eventRepo;
        private List<WorkEvent> eventList;

        [SetUp]
        public void SetUp()
        {
            this.file = FormFileBuilder.GetFormFile();
            this.mapper = MapperBuilder.GetMapper();
            this.photoService = PhotoLogicBuilder.GetPhotoService();

            this.users = UserManagerBuilder.GetWmanUsers();
            this.userManager = UserManagerBuilder.GetUserManager(users);

            this.pictureList = PhotoLogicBuilder.GetPictures();
            this.picturesRepo = PhotoLogicBuilder.GetPicturesRepo(pictureList);

            this.proofList = PhotoLogicBuilder.GetProofList();
            this.proofOfWorkRepo = PhotoLogicBuilder.GetProofOfWorkRepo(proofList);

            this.eventList = EventLogicBuilder.GetWorkEvents();
            this.eventRepo = EventLogicBuilder.GetEventRepo(eventList);
        }

        [Test]
        public async Task AddProfilePhoto_AddNewPhoto_SuccessfulOperation()
        {
            //Arrange
            PhotoLogic photoLogic = new(this.photoService.Object, this.userManager.Object, this.picturesRepo.Object, this.mapper, this.eventRepo.Object, this.proofOfWorkRepo.Object);

            //Act
            var call = await photoLogic.AddProfilePhoto(users[0].UserName, file);

            //Assert
            Assert.That(call.CloudPhotoID == "Test");
            this.userManager.Verify(x => x.Users, Times.Once);
            this.photoService.Verify(x => x.AddProfilePhotoAsync(It.IsAny<FormFile>()), Times.Once);
            this.picturesRepo.Verify(x => x.Add(It.IsAny<Pictures>()), Times.Once);
        }

        [Test]
        public async Task RemoveProfilePhoto_RemovedExistingPhoto_Successful()
        {
            //Arrange
            PhotoLogic photoLogic = new(this.photoService.Object, this.userManager.Object, this.picturesRepo.Object, this.mapper, this.eventRepo.Object, this.proofOfWorkRepo.Object);

            //Act
            await photoLogic.RemoveProfilePhoto(users[0].UserName);

            //Assert
            this.userManager.Verify(x => x.Users, Times.Once);
            this.photoService.Verify(x => x.DeleteProfilePhotoAsync(It.IsAny<string>()), Times.Once);
            this.picturesRepo.Verify(x => x.Delete(It.IsAny<int>()), Times.Once);
        }

        [Test]
        public async Task UpdateProfilePhoto_UpdateExistingPhoto_Successful()
        {
            //Arrange
            PhotoLogic photoLogic = new(this.photoService.Object, this.userManager.Object, this.picturesRepo.Object, this.mapper, this.eventRepo.Object, this.proofOfWorkRepo.Object);

            //Act
            var call = await photoLogic.UpdateProfilePhoto(users[0].UserName, file);

            //Assert
            Assert.That(call.CloudPhotoID == "Test");
            this.userManager.Verify(x => x.Users, Times.Once);
            //deletions
            this.photoService.Verify(x => x.DeleteProfilePhotoAsync(It.IsAny<string>()), Times.Once);
            this.picturesRepo.Verify(x => x.Delete(It.IsAny<int>()), Times.Once);
            //additions
            this.photoService.Verify(x => x.AddProfilePhotoAsync(It.IsAny<FormFile>()), Times.Once);
            this.picturesRepo.Verify(x => x.Add(It.IsAny<Pictures>()), Times.Once);
        }

        [Test]
        public async Task AddProofOfWorkPhoto_SuccessfulOperation()
        {
            //Arrange
            PhotoLogic photoLogic = new(this.photoService.Object, this.userManager.Object, this.picturesRepo.Object, this.mapper, this.eventRepo.Object, this.proofOfWorkRepo.Object);

            //Act
            var call = await photoLogic.AddProofOfWorkPhoto(eventList[0].Id, file);

            //Assert
            this.eventRepo.Verify(x => x.GetOne(It.IsAny<int>()), Times.Once);
            this.photoService.Verify(x => x.AddProofOfWorkPhotoAsync(It.IsAny<FormFile>()), Times.Once);
            this.proofOfWorkRepo.Verify(x => x.Add(It.IsAny<ProofOfWork>()), Times.Once);
        }

        [Test]
        public async Task RemoveProofOfWorkPhoto_SuccessfulRemoval()
        {
            //Arrange
            PhotoLogic photoLogic = new(this.photoService.Object, this.userManager.Object, this.picturesRepo.Object, this.mapper, this.eventRepo.Object, this.proofOfWorkRepo.Object);

            //Act
            var call = photoLogic.RemoveProofOfWorkPhoto(eventList[0].Id, pictureList[0].CloudPhotoID);

            //Assert
            this.eventRepo.Verify(x => x.GetOne(It.IsAny<int>()), Times.Once);
            this.photoService.Verify(x => x.DeleteProfilePhotoAsync(It.IsAny<string>()), Times.Once);
            this.proofOfWorkRepo.Verify(x => x.Delete(It.IsAny<int>()), Times.Once);
        }
    }
}
