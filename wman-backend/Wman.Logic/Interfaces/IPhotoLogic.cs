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
        Task RemoveProfilePhoto(string userName);
        Task<PhotoDTO> UpdateProfilePhoto(string userName, IFormFile file);
        Task<List<ProofOfWorkDTO>> AddProofOfWorkPhoto(int eventID, IFormFile file);
        Task RemoveProofOfWorkPhoto(int eventID, string cloudCloudPhotoID);
    }
}
