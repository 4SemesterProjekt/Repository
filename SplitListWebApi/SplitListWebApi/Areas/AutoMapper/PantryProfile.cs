﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiFormat.Pantry;
using AutoMapper;

namespace SplitListWebApi.Areas.AutoMapper
{
    public class PantryProfile : Profile
    {
        public PantryProfile()
        {
            CreateMap<PantryDTO, PantryModel>()
                .ForMember(pm => pm.PantryItems, opt => opt.Ignore())
                .ForMember(pm => pm.GroupModel, opt => opt.Ignore());

            CreateMap<PantryModel, PantryDTO>()
                .ForMember(
                    dto => dto.Items,
                    opt => 
                        opt.MapFrom(
                            pm => pm.PantryItems.Select(im => im.ItemModel.Name).ToList()))
                .ForMember(
                    dto => dto.Group,
                    opt =>
                        opt.MapFrom(
                            pm => pm.GroupModel))
                .ForMember(
                    dto => dto.GroupID,
                    opt =>
                        opt.MapFrom(
                            pm => pm.GroupModelID));
        }
    }
}
