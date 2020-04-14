using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using ApiFormat;
using ApiFormat.Group;
using AutoMapper;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace SplitListWebApi.Areas.AutoMapper
{
    public class GroupProfile : Profile
    {
        public GroupProfile()
        {
            CreateMap<GroupDTO, GroupModel>() //Only the primary keys are needed to search for a model, which is what the conversion is used for.
                .ForMember(gm => gm.ShoppingLists, opt => opt.Ignore()) //Ignoring these properties when converting to model.
                .ForMember(gm => gm.PantryModel, opt => opt.Ignore())
                .ForMember(gm => gm.UserGroups, opt => opt.Ignore());

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
                        opt.MapFrom(st => st.ShoppingLists.Select(sl => sl.ShoppingListItems).ToList()));

        }
    }
}
