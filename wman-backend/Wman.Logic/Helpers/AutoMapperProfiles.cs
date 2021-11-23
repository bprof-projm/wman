using AutoMapper;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wman.Data.DB_Models;
using Wman.Logic.Classes;
using Wman.Logic.DTO_Models;

namespace Wman.Logic.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<IdentityResult, UserDTO>();
            CreateMap<CreateEventDTO, AddressHUNDTO>();
            CreateMap<AddressHUNDTO, AddressHUN>().ReverseMap();
            CreateMap<WmanUser, UserDTO>()
                .ForMember(dest => dest.Picture, x =>x.MapFrom(src => src.ProfilePicture))
                .ReverseMap();
            CreateMap<WmanUser, WorkerDTO>().ReverseMap();
            CreateMap<WorkEvent, AssignedEventDTO>();
            //.ForMember(dest => dest.Address, x => x.MapFrom(src => src.Address));
            CreateMap<CreateEventDTO, WorkEvent>()
    // chose the destination-property and map the source itself
            .ForMember(dest => dest.Address, x => x.MapFrom(src => src.Address));
            CreateMap<DnDEventDTO, WorkEvent>();
            CreateMap<CreateLabelDTO, Label>();
            CreateMap<WorkEvent, WorkEventForWorkCardDTO>();
            CreateMap<Label, ListLabelsDTO>()
                .ForMember(dest => dest.BackgroundColor, x => x.MapFrom(src => src.Color))
                .ForMember(dest => dest.TextColor, x => x.MapFrom(src => LabelLogic.InverseColor(src.Color)));
            CreateMap<Pictures, PhotoDTO>();
        }
    }
}
