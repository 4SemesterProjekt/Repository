using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using ApiFormat;
using ApiFormat.Group;
using ApiFormat.Pantry;
using ApiFormat.ShadowTables;
using ApiFormat.ShoppingList;
using ApiFormat.User;
using AutoMapper;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace SplitListWebApi.Areas.AutoMapper
{
    public class GroupProfile : Profile
    {
        public GroupProfile()
        {
            CreateMap<GroupDTO, GroupModel>()
                .PreserveReferences()
                .ForMember(gm => gm.PantryModel, opt => opt.MapFrom(dto => dto.Pantry))
                .ForMember(gm => gm.UserGroups, opt => opt.Ignore())
                .ForMember(gm => gm.ShoppingLists, opt => opt.Ignore());

            CreateMap<GroupModel, GroupDTO>()
                .PreserveReferences()
                .ForMember(dto => dto.ShoppingLists, opt => opt.MapFrom(model => model.ShoppingLists))
                .ForMember(dto => dto.Users, opt => opt.MapFrom(model => model.UserGroups.Select(ug => ug.UserModel)))
                .ForMember(dto => dto.Pantry, opt => opt.MapFrom(model => model.PantryModel));
        }
    }
}
