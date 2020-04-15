using System.Collections.Generic;
using ApiFormat.Pantry;
using ApiFormat.ShoppingList;
using ApiFormat.User;

namespace ApiFormat.Group
{
    public class GroupDTO : IDTO
    {
        public int ModelId { get; set; }
        public string Name { get; set; }
        public int OwnerID { get; set; }
        public List<UserDTO> Users { get; set; }
        public PantryDTO Pantry { get; set; }
        public List<ShoppingListDTO> ShoppingLists { get; set; }
    }
}