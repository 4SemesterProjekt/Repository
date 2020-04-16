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
        public class GroupDTOToModel : ITypeConverter<GroupDTO, GroupModel>  //ITypeConverter<GroupDTO, List<UserGroup>>
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

                foreach (UserDTO user in source.Users)
                {
                    result.UserGroups.Add(new UserGroup
                    {
                        GroupModel = result,
                        GroupModelModelID = result.ModelId,
                        UserModel = context.Mapper.Map<UserDTO, UserModel>(user),
                        UserModelId = user.Id
                    });

                }
                return result;
            }
        }

        public GroupProfile()
        {
            CreateMap<GroupDTO, GroupModel>().ConvertUsing(new GroupDTOToModel());
               

            CreateMap<GroupModel, GroupDTO>()
                .ForMember( //Many-To-Many
                    dto => dto.Users, //From DTO
                    opt => 
                        opt.MapFrom(gm =>  //Map from the group-model's
                            gm.UserGroups.Select(st => st.UserModel).ToList())) //Navigational property into its model, and convert to a list.
                .ForMember( //One-To-One
                    dto => dto.Pantry,
                    opt => 
                        opt.MapFrom(gm => 
                                gm.PantryModel))
                .ForMember( //Many-To-Many
                    gm => gm.ShoppingLists,
                    opt =>
                        opt.MapFrom(gm => gm.ShoppingLists));

        }
    }
}
