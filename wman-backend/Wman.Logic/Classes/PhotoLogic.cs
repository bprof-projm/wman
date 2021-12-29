using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wman.Data.DB_Models;
using Wman.Logic.DTO_Models;
using Wman.Logic.Helpers;
using Wman.Logic.Interfaces;
using Wman.Logic.Services;
using Wman.Repository.Interfaces;

namespace Wman.Logic.Classes
{
    public class PhotoLogic : IPhotoLogic
    {
        IPhotoService _photoService;
        UserManager<WmanUser> userManager;
        IPicturesRepo picturesRepo;
        IMapper mapper;
        IWorkEventRepo workEventRepo;

        public PhotoLogic(IPhotoService photoService, UserManager<WmanUser> userManager, IPicturesRepo picturesRepo, IMapper mapper, IWorkEventRepo workEventRepo)
        {
            _photoService = photoService;
            this.userManager = userManager;
            this.picturesRepo = picturesRepo;
            this.mapper = mapper;
            this.workEventRepo = workEventRepo;
        }

        public async Task<PhotoDTO> AddProfilePhoto(string userName, IFormFile file)
        {
            var selectedUser = await (from x in userManager.Users
                                     where x.UserName == userName
                                     select x).Include(x => x.ProfilePicture).FirstOrDefaultAsync();
            if (selectedUser == null)
            {
                throw new NotFoundException(WmanError.UserNotFound);
            }

            var result = await _photoService.AddProfilePhotoAsync(file);
            if (result.Error != null)
            {
                throw new ArgumentException(result.Error.Message);
            }
            var UploadedPicture = new Pictures
            {
                Url = result.SecureUrl.AbsoluteUri,
                CloudPhotoID = result.PublicId,
                PicturesType = PicturesType.ProfilePic,
                WmanUser = selectedUser

            };
            selectedUser.ProfilePicture = UploadedPicture;
            await picturesRepo.Add(UploadedPicture);

            var photoResult = mapper.Map<PhotoDTO>(selectedUser.ProfilePicture);
            return photoResult;
        }

        public async Task RemoveProfilePhoto(string userName)
        {
            var selectedUser = await (from x in userManager.Users
                                      where x.UserName == userName
                                      select x).Include(x => x.ProfilePicture).FirstOrDefaultAsync();
            if (selectedUser == null)
            {
                throw new NotFoundException(WmanError.UserNotFound);
            }

            var selectedPhoto = await (from x in picturesRepo.GetAll()
                                       where x.WManUserID == selectedUser.Id
                                       select x).FirstOrDefaultAsync();
            if (selectedPhoto == null)
            {
                throw new NotFoundException(WmanError.PhotoNotFound);
            }


            // Remove Photo from the cloud
            await _photoService.DeleteProfilePhotoAsync(selectedPhoto.CloudPhotoID);

            // remove picture data from db
            await picturesRepo.Delete(selectedPhoto.Id);
        }

        public async Task<PhotoDTO> UpdateProfilePhoto(string userName, IFormFile file)
        {
            var selectedUser = await (from x in userManager.Users
                                      where x.UserName == userName
                                      select x).Include(x => x.ProfilePicture).FirstOrDefaultAsync();
            if (selectedUser == null)
            {
                throw new NotFoundException(WmanError.UserNotFound);
            }

            var selectedPhoto = await (from x in picturesRepo.GetAll()
                                       where x.WManUserID == selectedUser.Id
                                       select x).FirstOrDefaultAsync();
            if (selectedPhoto == null)
            {
                throw new NotFoundException(WmanError.PhotoNotFound);
            }

            // Remove Photo from the cloud
            await _photoService.DeleteProfilePhotoAsync(selectedPhoto.CloudPhotoID);

            // remove picture data from db
            await picturesRepo.Delete(selectedPhoto.Id);


            var result = await _photoService.AddProfilePhotoAsync(file);
            if (result.Error != null)
            {
                throw new ArgumentException(result.Error.Message);
            }
            var UploadedPicture = new Pictures
            {
                Url = result.SecureUrl.AbsoluteUri,
                CloudPhotoID = result.PublicId,
                PicturesType = PicturesType.ProfilePic,

            };
            selectedUser.ProfilePicture = UploadedPicture;
            await picturesRepo.Add(UploadedPicture);

            var photoResult = mapper.Map<PhotoDTO>(selectedUser.ProfilePicture);
            return photoResult;
        }
        public async Task<List<PhotoDTO>> AddProofOfWorkPhoto(int eventID, IFormFile file)
        {
            var selectedEvent = await workEventRepo.GetOne(eventID);
            if (selectedEvent == null)
            {
                throw new NotFoundException(WmanError.EventNotFound);
            }

            var result = await _photoService.AddProofOfWorkPhotoAsync(file);
            if (result.Error != null)
            {
                throw new ArgumentException(result.Error.Message);
            }
            var UploadedPicture = new Pictures
            {
                Url = result.SecureUrl.AbsoluteUri,
                CloudPhotoID = result.PublicId,
                PicturesType = PicturesType.ProofOfWorkPic,
                WorkEventID = eventID

            };
            selectedEvent.ProofOfWorkPic.Add(UploadedPicture);
            await picturesRepo.Add(UploadedPicture);

            List<PhotoDTO> photoResult = new List<PhotoDTO>();
            foreach (var item in selectedEvent.ProofOfWorkPic)
            {
                photoResult.Add(mapper.Map<PhotoDTO>(item));
            }
            return photoResult;
        }
        public async Task RemoveProofOfWorkPhoto(int eventID, string cloudCloudPhotoID)
        {
            var selectedEvent = await workEventRepo.GetOne(eventID);
            if (selectedEvent == null)
            {
                throw new NotFoundException(WmanError.EventNotFound);
            }

            var selectedPhoto = await (from x in picturesRepo.GetAll()
                                       where x.CloudPhotoID == cloudCloudPhotoID
                                       select x).FirstOrDefaultAsync();
            if (selectedPhoto == null)
            {
                throw new NotFoundException(WmanError.PhotoNotFound);
            }

            await _photoService.DeleteProfilePhotoAsync(cloudCloudPhotoID);

            // remove picture data from db
            await picturesRepo.Delete(selectedPhoto.Id);

        }
    }
}
