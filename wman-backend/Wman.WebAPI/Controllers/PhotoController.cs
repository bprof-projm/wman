using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Wman.Data.DB_Models;
using Wman.Logic.DTO_Models;
using Wman.Logic.Services;
using Wman.Repository.Interfaces;

namespace Wman.WebAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PhotoController : ControllerBase
    {
        IPhotoService _photoService;
        UserManager<WmanUser> userManager;
        IPicturesRepo picturesRepo;
        IMapper mapper;

        public PhotoController(IPhotoService photoService, UserManager<WmanUser> userManager, IPicturesRepo picturesRepo, IMapper mapper)
        {
            _photoService = photoService;
            this.userManager = userManager;
            this.picturesRepo = picturesRepo;
            this.mapper = mapper;
        }

        [HttpPost("AddPhoto/{userName}")]
        public async Task<ActionResult<PhotoDTO>> AddProfilePhoto(string userName,IFormFile file)
        {
            var selectedUser = await (from x in userManager.Users
                               where x.UserName == userName
                               select x).Include(x => x.ProfilePicture).FirstOrDefaultAsync();

            var result = await _photoService.AddProfilePhotoAsync(file);
            if (result.Error != null)
            {
                return BadRequest(result.Error.Message);
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
            return Ok(photoResult);
        }
        [HttpDelete("RemovePhoto/{publicId}")]
        public async Task<ActionResult<PhotoDTO>> RemoveProfilePhoto(string publicId)
        {
            var selectedPhoto = await (from x in picturesRepo.GetAll()
                                 where x.CloudPhotoID == publicId
                                 select x).FirstOrDefaultAsync();
            if (selectedPhoto == null)
            {
                return BadRequest("User Not found");
            }
            var selectedUser = await (from x in userManager.Users
                                      where x.Id == selectedPhoto.WManUserID
                                      select x).FirstOrDefaultAsync();

            // Remove Photo from the cloud
            await _photoService.DeleteProfilePhotoAsync(publicId);

            // remove picture data from db
            await picturesRepo.Delete(selectedPhoto.Id);

            return Ok();
        }
    }
}
