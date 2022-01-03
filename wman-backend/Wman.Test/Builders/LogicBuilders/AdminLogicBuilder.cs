using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Wman.Data.DB_Models;
using Wman.Logic.Classes;

namespace Wman.Test.Builders.LogicBuilders
{
    public class AdminLogicBuilder
    {
        public static PhotoLogic PhotoLogicFactory(UserManager<WmanUser> userManager, IMapper mapper)
        {
            var pictureList = PhotoLogicBuilder.GetPictures();
            var picturesRepo = PhotoLogicBuilder.GetPicturesRepo(pictureList);
            var photoService = PhotoLogicBuilder.GetPhotoService();


            return new PhotoLogic(photoService.Object, userManager, picturesRepo.Object, mapper);
        }
    }
}
