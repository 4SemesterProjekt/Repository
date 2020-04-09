using System.Collections.Generic;
using ApiFormat.Pantry;
using ApiFormat.ShadowTables;
using ApiFormat.ShoppingList;
using ApiFormat.User;

namespace ApiFormat
{
    public class Group : GroupModel, IGroupDTO
    {
        List<IShoppingListDTO> IGroupDTO.ShoppingLists { get; set; }

        IPantryDTO IGroupDTO.Pantry { get; set; }

        public List<IUserDTO> Users { get; set; }
    }
}