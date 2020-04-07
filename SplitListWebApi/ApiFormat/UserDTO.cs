using System;
using System.Collections.Generic;
using System.Text;

namespace ApiFormat
{
    public class UserDTO
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<GroupDTO> Groups { get; set; }
    }
}
