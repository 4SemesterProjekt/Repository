using System;
using System.Collections.Generic;
using System.Text;
using ApiFormat.ShoppingList;
using AutoMapper;
using SplitList.Models;

namespace SplitList.AutoMapper
{
    class ShoppingListProfile : Profile
    {
        public ShoppingListProfile()
        {
            CreateMap<ShoppingListDTO, ShoppingList>()
                .PreserveReferences()
                .ForMember(sl => sl.ShoppingListId, opt => opt.MapFrom(slDto => slDto.ModelId));

            CreateMap<ShoppingList, ShoppingListDTO>()
                .PreserveReferences()
                .ForMember(slDto => slDto.ModelId, opt => opt.MapFrom(sl => sl.ShoppingListId));
        }
    }
}
