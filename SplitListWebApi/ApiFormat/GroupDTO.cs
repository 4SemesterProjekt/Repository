using System;
using System.Collections.Generic;
using System.Text;

namespace ApiFormat
{
    public class GroupDTO
    {
        public int GroupID { get; set; }
        public string Name { get; set; }
        public string OwnerID { get; set; }
        public List<ShoppingListDTO> ShoppingLists { get; set; }
        public PantryDTO Pantry { get; set; }
        public List<UserDTO> Users { get; set; }
    }
}
