using System.Collections.Generic;
using ApiFormat.Item;
using ApiFormat.ShadowTables;

namespace ApiFormat.ShoppingList
{
    public class ShoppingList : IShoppingListModel, IShoppingListDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int GroupID { get; set; }
        public string GroupName { get; set; }
        public ICollection<IItemDTO> Items { get; set; }
        public IGroupModel Group { get; set; }
        public ICollection<ShoppingListItem> ShoppingListItems { get; set; }
    }
}