using System.Collections.Generic;
using ApiFormat.Group;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace ApiFormat.User
{
    public class UserDTO : IDTO
    {
        public List<GroupDTO> Groups { get; set; }
        public int ModelId { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
    }
}