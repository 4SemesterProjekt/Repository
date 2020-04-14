using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiFormat;
using ApiFormat.Group;
using AutoMapper;

namespace SplitListWebApi.Areas.AutoMapper
{
    public class GroupProfile : Profile
    {
        public GroupProfile()
        {
            CreateMap<GroupDTO, GroupModel>()
                .ForMember( 
                    gm => gm.PantryModel, 
                    opt => 
                        opt.MapFrom(tgt => tgt.Pantry))
                .ForMember(gm => gm.ShoppingLists,
                    opt => 
                        opt.MapFrom(tgt => tgt.ShoppingLists))
                .ForMember(
                    gm => gm.UserGroups,
                    opt => 
                        opt.MapFrom(tgt => tgt.Users));

            CreateMap<GroupModel, GroupDTO>()
                .ForMember(
                    gdto => gdto.Pantry,
                    opt =>
                        opt.MapFrom(
                            tgt => tgt.PantryModel))
                .ForMember(
                    gdto => gdto.Users,
                    opt => 
                        opt.MapFrom(tgt => tgt.UserGroups))
                .ForMember(gdto => gdto.ShoppingLists,
                    opt => 
                        opt.MapFrom(tgt => tgt.ShoppingLists));
        }
    }
}
