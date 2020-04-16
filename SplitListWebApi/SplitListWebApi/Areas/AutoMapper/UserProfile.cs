using ApiFormat.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;

namespace SplitListWebApi.Areas.AutoMapper
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<UserDTO, UserModel>()
                .ForMember(um => um.UserGroups, opt => opt.MapFrom(dto => dto.Groups));


            CreateMap<UserModel, UserDTO>()
                .ForMember(
                    dto => dto.Groups,
                    opt =>
                        opt.MapFrom(um =>
                            um.UserGroups.Select(st => st.GroupModel).ToList()));
        }
    }
}
