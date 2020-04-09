using System.Collections.Generic;
using ApiFormat.Item;

namespace ApiFormat.ShoppingList
{
    public interface IShoppingListDTO : IDTO
    {
        public int GroupID { get; set; }
        public string GroupName { get; set; }
        public ICollection<IItemDTO> Items { get; set; }
    }
}