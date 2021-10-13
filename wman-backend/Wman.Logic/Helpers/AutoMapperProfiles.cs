﻿using AutoMapper;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wman.Data.DB_Models;
using Wman.Logic.DTO_Models;

namespace Wman.Logic.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<IdentityResult, UserDTO>();
            CreateMap<CreateEventDTO, AddressHUNDTO>();
            CreateMap<AddressHUNDTO, AddressHUN>();
            CreateMap<CreateEventDTO, WorkEvent>()
    // chose the destination-property and map the source itself
            .ForMember(dest => dest.Address, x => x.MapFrom(src => src.Address));
        }
    }
}