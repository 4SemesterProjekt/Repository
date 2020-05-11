using System;
using System.Collections.Generic;
using System.Text;
using ApiFormat.Pantry;
using AutoMapper;
using SplitList.Models;

namespace SplitList.AutoMapper
{
    class PantryProfile : Profile
    {
        public PantryProfile()
        {
            CreateMap<PantryDTO, Pantry>()
                .PreserveReferences()
                .ForMember(p => p.PantryId, opt => opt.MapFrom(pDto => pDto.ModelId));

            CreateMap<Pantry, PantryDTO>()
                .PreserveReferences()
                .ForMember(pDto => pDto.ModelId, opt => opt.MapFrom(p => p.PantryId));
        }
    }
}
