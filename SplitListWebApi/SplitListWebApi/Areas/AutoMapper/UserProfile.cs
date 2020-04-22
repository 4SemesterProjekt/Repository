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
        public class UserDTOToModel : ITypeConverter<UserDTO, UserModel>
        {
            public UserModel Convert(UserDTO source, UserModel destination, ResolutionContext context)
            {
                var result = new UserModel()
                {
                    ModelId = source.ModelId,
                    Id = source.Id,
                    Name = source.Name,
                    UserGroups = new List<UserGroup>()
                };
                if (source.Groups != null)
                {
                    foreach (var groupDto in source.Groups)
                    {
                        result.UserGroups.Add(new UserGroup()
                        {
                            GroupModel = context.Mapper.Map<GroupDTO, GroupModel>(groupDto),
                            UserModel = result
                        });
                    }
                }
                return result;
            }
        }

        public class UserModelToDTO : ITypeConverter<UserModel, UserDTO>
        {
            public UserDTO Convert(UserModel source, UserDTO destination, ResolutionContext context)
            {
                var result = new UserDTO()
                {
                    ModelId = source.ModelId,
                    Id = source.Id,
                    Name = source.Name,
                    Groups = context.Mapper.Map<List<UserGroup>, List<GroupDTO>>(source.UserGroups)
                };

                return result;
            }
        }

        public class UserGroupToUserDTO : ITypeConverter<UserGroup, UserDTO>
        {
            public UserDTO Convert(UserGroup source, UserDTO destination, ResolutionContext context)
            {
                return new UserDTO()
                {
                    Groups = context.Mapper.Map<List<UserGroup>, List<GroupDTO>>(source.UserModel.UserGroups),
                    Id = source.UserId,
                    ModelId = source.UserModel.ModelId,
                    Name = source.UserModel.Name
                };
            }
        }

        public UserProfile()
        {
            CreateMap<UserDTO, UserModel>().PreserveReferences().ConvertUsing(new UserDTOToModel());

            CreateMap<UserModel, UserDTO>().PreserveReferences().ConvertUsing(new UserModelToDTO());

            CreateMap<UserGroup, UserDTO>().PreserveReferences().ConvertUsing(new UserGroupToUserDTO());
        }
    }
}
