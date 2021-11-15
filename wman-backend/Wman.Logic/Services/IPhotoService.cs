using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wman.Logic.Services
{
    public interface IPhotoService
    {
        Task<ImageUploadResult> AddProfilePhotoAsync(IFormFile file);
        Task<DeletionResult> DeleteProfilePhotoAsync(string publicId);
    }
}
