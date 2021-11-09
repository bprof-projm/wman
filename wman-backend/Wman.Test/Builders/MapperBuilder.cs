using AutoMapper;
using Wman.Logic.Helpers;

namespace Wman.Test.Builders
{
    public class MapperBuilder
    {
        public static IMapper GetMapper()
        {
            var mapperconf = new MapperConfiguration(x => { x.AddProfile(new AutoMapperProfiles()); });

            IMapper mapper = mapperconf.CreateMapper();

            return mapper;
        }
    }
}
