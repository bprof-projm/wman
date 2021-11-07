using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wman.Logic.DTO_Models;

namespace Wman.Logic.Interfaces
{
    public interface IPhotoLogic
    {
        Task<PhotoDTO> AddProfilePhoto(string userName, IFormFile file);
        Task RemoveProfilePhoto(string publicId);
        Task<PhotoDTO> UpdateProfilePhoto(string publicId, IFormFile file);
    }
}
