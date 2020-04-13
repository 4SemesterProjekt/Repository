using System.Collections.Generic;

namespace ApiFormat.User
{
    public class IUserDTO : IDTO
    {
        public List<GroupDTO> Groups { get; set; }
        public double Id { get; set; }
        public string Name { get; set; }
    }
}