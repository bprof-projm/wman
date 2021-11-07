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

        public PhotoLogic(IPhotoService photoService, UserManager<WmanUser> userManager, IPicturesRepo picturesRepo, IMapper mapper)
        {
            _photoService = photoService;
            this.userManager = userManager;
            this.picturesRepo = picturesRepo;
            this.mapper = mapper;
        }
        public async Task<PhotoDTO> AddProfilePhoto(string userName, IFormFile file)
        {
            var selectedUser = await (from x in userManager.Users
                                     where x.UserName == userName
                                     select x).Include(x => x.ProfilePicture).FirstOrDefaultAsync();

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

        public async Task RemoveProfilePhoto(string publicId)
        {
            var selectedPhoto = await(from x in picturesRepo.GetAll()
                                      where x.CloudPhotoID == publicId
                                      select x).FirstOrDefaultAsync();
            if (selectedPhoto == null)
            {
                throw new ArgumentException("Photo Not found");
            }


            // Remove Photo from the cloud
            await _photoService.DeleteProfilePhotoAsync(publicId);

            // remove picture data from db
            await picturesRepo.Delete(selectedPhoto.Id);
        }

        public async Task<PhotoDTO> UpdateProfilePhoto(string publicId, IFormFile file)
        {
            var selectedPhoto = await(from x in picturesRepo.GetAll()
                                      where x.CloudPhotoID == publicId
                                      select x).FirstOrDefaultAsync();
            if (selectedPhoto == null)
            {
                throw new ArgumentException("Photo Not found");
            }

            // Remove Photo from the cloud
            await _photoService.DeleteProfilePhotoAsync(publicId);

            // remove picture data from db
            await picturesRepo.Delete(selectedPhoto.Id);

            var selectedUser = await(from x in userManager.Users
                                     where x.Id == selectedPhoto.WManUserID
                                     select x).Include(x => x.ProfilePicture).FirstOrDefaultAsync();

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
    }
}
