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
        public string OwnerID { get; set; }
        public ICollection<IShoppingListDTO> ShoppingLists { get; set; }
        public IPantryDTO Pantry { get; set; }
        public ICollection<IUserDTO> Users { get; set; }
    }
}
