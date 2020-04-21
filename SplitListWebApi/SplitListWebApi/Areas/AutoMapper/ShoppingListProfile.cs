using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiFormat.ShoppingList;
using AutoMapper;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SplitListWebApi.Areas.AutoMapper
{
    public class ShoppingListProfile : Profile
    {
        public ShoppingListProfile()
        {
            CreateMap<ShoppingListDTO, ShoppingListModel>().PreserveReferences()
                .ForMember(sm => sm.ShoppingListItems, opt => opt.MapFrom(dto => dto.Items))
                .ForMember(sm => sm.GroupModel, opt => opt.MapFrom(dto => dto.Group))
                .ForMember(sm => sm.GroupModelId, opt => opt.MapFrom(dto => dto.GroupID));

            CreateMap<ShoppingListModel, ShoppingListDTO>().PreserveReferences()
                .ForMember(
                    dto => dto.Items,
                    opt =>
                        opt.MapFrom(
                            sm => sm.ShoppingListItems.Select(st => st.ItemModel)))
                .ForMember(
                    dto => dto.Group,
                    opt =>
                        opt.Ignore())
                .ForMember(
                    dto => dto.GroupID,
                    opt => 
                        opt.MapFrom(
                            sm => sm.GroupModelId));
        }
    }
}
