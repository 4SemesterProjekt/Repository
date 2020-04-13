using System.Collections.Generic;
using ApiFormat.Item;

namespace ApiFormat.ShoppingList
{
    public class IShoppingListDTO : IDTO
    {
        public int GroupID { get; set; }
        public string GroupName { get; set; }
        public List<ItemDTO> Items { get; set; }
        public double Id { get; set; }
        public string Name { get; set; }
    }
}