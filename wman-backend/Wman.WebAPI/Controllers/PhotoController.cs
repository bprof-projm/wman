using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Wman.Data.DB_Models;
using Wman.Logic.Services;

namespace Wman.WebAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PhotoController : ControllerBase
    {
        IPhotoService _photoService;

        public PhotoController(IPhotoService photoService)
        {
            _photoService = photoService;
        }

        [HttpPost("AddPhoto")]
        public async Task<ActionResult<Pictures>> AddProfilePhoto(IFormFile file)
        {
            
            var result = await _photoService.AddProfilePhotoAsync(file);
            if (result.Error != null)
            {
                return BadRequest(result.Error.Message);
            }
            var pictures = new Pictures
            {
                Url = result.SecureUrl.AbsoluteUri,
                CloudPhotoID = result.PublicId
            };

            //user.Photos.Add(photo);
            //if (await _unitOfWork.Complete())
            //{
            //    // return _mapper.Map<PhotoDto>(photo);
            //    return CreatedAtRoute("GetUser", new { Username = user.UserName }, _mapper.Map<PhotoDto>(photo));
            //}
            //return BadRequest("Problem adding photo");
            return Ok();
        }
    }
}
