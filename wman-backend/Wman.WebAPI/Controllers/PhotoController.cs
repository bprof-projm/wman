using AutoMapper;
using Microsoft.AspNetCore.Authorization;
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
using Wman.Logic.Interfaces;
using Wman.Logic.Services;
using Wman.Repository.Interfaces;

namespace Wman.WebAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class PhotoController : ControllerBase
    {
        
        IPhotoLogic photoLogic;

        public PhotoController(IPhotoLogic photoLogic)
        {
            this.photoLogic = photoLogic;
        }

        [HttpPost("AddProfilePhoto/{userName}")]
        public async Task<ActionResult<PhotoDTO>> AddProfilePhoto(string userName, IFormFile file)
        {
                var result = await photoLogic.AddProfilePhoto(userName, file);
                return Ok(result);
        }
        [HttpDelete("RemoveProfilePhoto/{userName}")]
        public async Task<ActionResult> RemoveProfilePhoto(string userName)
        {
                 await photoLogic.RemoveProfilePhoto(userName);
                return Ok();
        }
        [HttpPut("UpdateProfilePhoto/{userName}")]
        public async Task<ActionResult<PhotoDTO>> UpdateProfilePhoto(string userName, IFormFile file) 
        {
                var result = await photoLogic.UpdateProfilePhoto(userName, file);
                return Ok(result);
        }
        [HttpPost("AddProofOfWorkPhoto/{eventId}")]
        public async Task<ActionResult<PhotoDTO>> AddProofOfWorkPhoto(int eventId, IFormFile file)
        {
            var result = await photoLogic.AddProofOfWorkPhoto(eventId, file);
            return Ok(result);
        }
        [HttpDelete("RemoveProofOfWorkPhoto")]
        public async Task<ActionResult> RemoveProofOfWorkPhoto(int eventId,string cloudCloudPhotoID)
        {
            await photoLogic.RemoveProofOfWorkPhoto(eventId,cloudCloudPhotoID);
            return Ok();
        }
    }
}
