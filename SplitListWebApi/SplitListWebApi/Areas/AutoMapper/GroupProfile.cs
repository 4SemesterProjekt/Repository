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
        public class GroupDTOToModel : ITypeConverter<GroupDTO, GroupModel>
        {
            public GroupModel Convert(GroupDTO source, GroupModel destination, ResolutionContext context)
            {
                GroupModel result = new GroupModel()
                {
                    Name = source.Name,
                    OwnerId = source.OwnerID,
                    ModelId = source.ModelId,
                    PantryModel = new PantryModel(),
                    ShoppingLists = new List<ShoppingListModel>(),
                    UserGroups = new List<UserGroup>()
                };
                if (source.Users != null)
                {
                    foreach (UserDTO user in source.Users)
                    {
                        result.UserGroups.Add(new UserGroup
                        {
                            GroupModel = result,
                            GroupModelModelID = result.ModelId,
                            UserModel = context.Mapper.Map<UserDTO, UserModel>(user),
                            UserId = user.Id
                        });

                    }
                }
                return result;
            }
        }

        public class GroupModelToDTO : ITypeConverter<GroupModel, GroupDTO>
        {
            public GroupDTO Convert(GroupModel source, GroupDTO destination, ResolutionContext context)
            {
                GroupDTO result = new GroupDTO()
                {
                    OwnerID = source.OwnerId,
                    Name = source.Name,
                    ModelId = source.ModelId,
                    Pantry = context.Mapper.Map<PantryModel, PantryDTO>(source.PantryModel),
                    ShoppingLists = new List<ShoppingListDTO>(),
                    Users = new List<UserDTO>()
                };
                if (source.UserGroups != null)
                {
                    foreach (UserGroup user in source.UserGroups)
                    {
                        result.Users.Add(context.Mapper.Map<UserModel, UserDTO>(user.UserModel));
                    }
                }
                if (source.ShoppingLists != null)
                {
                    foreach (ShoppingListModel shoppingList in source.ShoppingLists)
                    {
                        result.ShoppingLists.Add(context.Mapper.Map<ShoppingListModel, ShoppingListDTO>(shoppingList));
                    }
                }
                return result;
            }
        }

        public GroupProfile()
        {
            CreateMap<GroupDTO, GroupModel>().ConvertUsing(new GroupDTOToModel());

            CreateMap<GroupModel, GroupDTO>().ConvertUsing(new GroupModelToDTO());
        }
    }
}
