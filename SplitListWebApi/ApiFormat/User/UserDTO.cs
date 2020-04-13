using System.Collections.Generic;
using ApiFormat.Group;

namespace ApiFormat.User
{
    public class UserDTO : IDTO
    {
        public List<GroupDTO> Groups { get; set; }
        public double Id { get; set; }
        public string Name { get; set; }
    }
}