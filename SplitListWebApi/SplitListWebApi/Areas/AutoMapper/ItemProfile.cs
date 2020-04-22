using ApiFormat.Item;
using ApiFormat.ShadowTables;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SplitListWebApi.Areas.AutoMapper
{
    public class ItemProfile : Profile
    {
        public ItemProfile()
        {
            CreateMap<ShoppingListItem, ItemDTO>()
                .PreserveReferences()
                .ForMember(dto => dto.Amount, opt => opt.MapFrom(model => model.Amount))
                .ForMember(dto => dto.Name, opt => opt.MapFrom(model => model.ItemModel.Name))
                .ForMember(dto => dto.ModelId, opt => opt.MapFrom(model => model.ItemModelID))
                .ForMember(dto => dto.Type, opt => opt.MapFrom(model => model.ItemModel.Type));
        }
    }
}
