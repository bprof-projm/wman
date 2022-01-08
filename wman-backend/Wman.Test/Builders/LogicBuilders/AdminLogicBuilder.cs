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

            var workeventList = EventLogicBuilder.GetWorkEvents();
            var workeventRepo = EventLogicBuilder.GetEventRepo(workeventList);

            var prooflist = PhotoLogicBuilder.GetProofList();
            var proofOfWorkRepo = PhotoLogicBuilder.GetProofOfWorkRepo(prooflist);

            return new PhotoLogic(photoService.Object, userManager, picturesRepo.Object, mapper, workeventRepo.Object, proofOfWorkRepo.Object);
        }
    }
}
