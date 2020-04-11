using System;
using System.Collections.Generic;
using System.Text;
using ApiFormat;
using SplitList.Models;

namespace SplitList.Mapping
{
    public class UserMapper
    {
        public static User UserDtoToUser(UserDTO dto)
        {
            User newUser = new User();
            newUser.Name = dto.Name;
            newUser.Id = dto.Id;
            foreach (var dtoGroup in dto.Groups)
            {
                newUser.Groups.Add(GroupMapper.GroupDtoToGroup(dtoGroup));
            }
            return newUser;
        }

        public static UserDTO UserToUserDto(User user)
        {
            UserDTO newDto = new UserDTO();
            newDto.Groups = new List<GroupDTO>();
            newDto.Name = user.Name;
            newDto.Id = user.Id;
            foreach (var userGroup in user.Groups)
            {
                newDto.Groups.Add(GroupMapper.GroupToGroupDto(userGroup));
            }
            return newDto;
        }
    }
}
