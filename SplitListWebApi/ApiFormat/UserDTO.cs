using System;
using System.Collections.Generic;
using System.Text;

namespace ApiFormat
{
    class UserDTO
    {
        public int UserID { get; set; }
        public string Name { get; set; }
        public List<GroupDTO> Groups { get; set; }
    }
}
