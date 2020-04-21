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
            newGroup.GroupOwnerId = dto.OwnerID;
            foreach (var dtoUser in dto.Users)
            {
                newGroup.Users.Add(UserMapper.UserDtoToUser(dtoUser));
            }
            return newGroup;
        }

        public static GroupDTO GroupToGroupDto(Group group)
        {
            GroupDTO newDto = new GroupDTO();
            newDto.Users = new List<UserDTO>();
            newDto.GroupID = group.GroupId;
            newDto.Name = group.Name;
            newDto.OwnerID = group.GroupOwnerId;
            foreach (var groupUser in group.Users)
            {
                newDto.Users.Add(UserMapper.UserToUserDto(groupUser));
            }
            return newDto;
        }
    }
}
