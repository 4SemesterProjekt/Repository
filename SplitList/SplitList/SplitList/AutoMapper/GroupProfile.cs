using System;
using System.Collections.Generic;
using System.Text;
using ApiFormat.Group;
using AutoMapper;
using SplitList.Models;

namespace SplitList.AutoMapper
{
    class GroupProfile : Profile
    {
        public GroupProfile()
        {
            CreateMap<GroupDTO, Group>()
                .PreserveReferences()
                .ForMember(g => g.GroupId, opt => opt.MapFrom(gDto => gDto.ModelId));
            CreateMap<Group, GroupDTO>()
                .PreserveReferences()
                .ForMember(gDto => gDto.ModelId, opt => opt.MapFrom(g => g.GroupId));
        }
    }
}
