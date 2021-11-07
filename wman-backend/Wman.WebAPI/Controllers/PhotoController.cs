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
            try
            {
                var result = await photoLogic.AddProfilePhoto(userName, file);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error : {ex}");
            }
        }
        [HttpDelete("RemoveProfilePhoto/{userName}")]
        public async Task<ActionResult> RemoveProfilePhoto(string userName)
        {
            try
            {
                 await photoLogic.RemoveProfilePhoto(userName);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error : {ex}");
            }
        }
        [HttpPut("UpdateProfilePhoto/{userName}")]
        public async Task<ActionResult<PhotoDTO>> UpdateProfilePhoto(string userName, IFormFile file) 
        {
            try
            {
                var result = await photoLogic.UpdateProfilePhoto(userName, file);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error : {ex}");
            }
        }
    }
}
