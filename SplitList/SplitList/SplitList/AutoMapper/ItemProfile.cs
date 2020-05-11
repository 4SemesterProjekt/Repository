using System;
using System.Collections.Generic;
using System.Text;
using ApiFormat.Item;
using AutoMapper;
using SplitList.Models;

namespace SplitList.AutoMapper
{
    class ItemProfile : Profile
    {
        public ItemProfile()
        {
            CreateMap<ItemDTO, Item>().PreserveReferences()
                .ForMember(i => i.ItemId, opt => opt.MapFrom(iDto => iDto.ModelId));

            CreateMap<Item, ItemDTO>()
                .PreserveReferences()
                .ForMember(iDto => iDto.ModelId, opt => opt.MapFrom(i => i.ItemId));
        }
    }
}
