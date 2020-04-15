using System.Collections.Generic;
using ApiFormat.Group;

namespace ApiFormat.User
{
    public class UserDTO : IDTO
    {
        public List<GroupDTO> Groups { get; set; }
        public int ModelId { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
    }
}