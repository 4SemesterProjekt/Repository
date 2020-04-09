using System;
using System.Collections.Generic;
using System.Text;
using ApiFormat.Pantry;
using ApiFormat.ShoppingList;
using ApiFormat.User;

namespace ApiFormat
{
    public interface IGroupDTO : IDTO
    {
        int OwnerId { get; set; }
        List<IShoppingListDTO> ShoppingLists { get; set; }
        IPantryDTO Pantry { get; set; }
        List<IUserDTO> Users { get; set; }
    }
}
