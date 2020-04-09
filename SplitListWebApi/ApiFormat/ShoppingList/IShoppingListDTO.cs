using System.Collections.Generic;
using ApiFormat.Item;

namespace ApiFormat.ShoppingList
{
    public interface IShoppingListDTO : IDTO
    {
        int GroupID { get; set; }
        string GroupName { get; set; }
        List<IItemDTO> Items { get; set; }
    }
}