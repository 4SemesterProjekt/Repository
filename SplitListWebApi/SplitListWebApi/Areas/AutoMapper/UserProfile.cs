using ApiFormat.User;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using ApiFormat;
using ApiFormat.Group;
using ApiFormat.Pantry;
using ApiFormat.ShadowTables;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch.Internal;
using Microsoft.AspNetCore.Mvc.Formatters;
using SplitListWebApi.Utilities;

namespace SplitListWebApi.Areas.AutoMapper
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<UserDTO, UserModel>().PreserveReferences()
                .ForMember(um => um.UserGroups, opt => opt.Ignore());

            CreateMap<UserModel, UserDTO>().PreserveReferences()
                .ForMember(dto => dto.Name, opt => opt.MapFrom(um => um.Name))
                .ForMember(dto => dto.Groups, opt => opt.MapFrom(um => um.UserGroups.Select(ug => ug.GroupModel)));
        }
    }
}
