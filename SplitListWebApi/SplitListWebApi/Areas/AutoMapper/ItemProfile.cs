using ApiFormat.Item;
using ApiFormat.ShadowTables;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

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

            CreateMap<PantryItem, ItemDTO>()
                .PreserveReferences()
                .ForMember(dto => dto.Name, opt => opt.MapFrom(pi => pi.ItemModel.Name))
                .ForMember(dto => dto.ModelId, opt => opt.MapFrom(pi => pi.ItemModelID))
                .ForMember(dto => dto.Type, opt => opt.MapFrom(pi => pi.ItemModel.Type))
                .ForMember(dto => dto.Amount, opt => opt.MapFrom(pi => pi.Amount));

            CreateMap<RecipeItem, ItemDTO>()
                .PreserveReferences()
                .ForMember(dto => dto.Name, opt => opt.MapFrom(ri => ri.ItemModel.Name))
                .ForMember(dto => dto.ModelId, opt => opt.MapFrom(ri => ri.ItemModelID))
                .ForMember(dto => dto.Type, opt => opt.MapFrom(ri => ri.ItemModel.Type))
                .ForMember(dto => dto.Amount, opt => opt.MapFrom(ri => ri.Amount));

            CreateMap<ItemDTO, ItemModel>()
                .PreserveReferences();

            CreateMap<ItemModel, ItemDTO>()
                .PreserveReferences()
                .ForMember(dto => dto.Amount, opt => opt.Ignore());
        }
    }
}
