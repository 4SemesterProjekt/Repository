using System;
using System.Collections.Generic;
using System.Text;
using ApiFormat;
using SplitList.Models;

namespace SplitList.Mapping
{
    public class GroupMapper
    {
        public static Group GroupDtoToGroup(GroupDTO dto)
        {
            Group newGroup = new Group();
            newGroup.GroupId = dto.GroupID;
            newGroup.Name = dto.Name;
            return newGroup;
        }

        public static GroupDTO GroupToGroupDto(Group group)
        {
            GroupDTO newDto = new GroupDTO();
            newDto.GroupID = group.GroupId;
            newDto.Name = group.Name;
            return newDto;
        }
    }
}
