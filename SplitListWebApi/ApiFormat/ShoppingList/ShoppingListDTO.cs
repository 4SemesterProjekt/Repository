using System.Collections.Generic;
using ApiFormat.Group;
using ApiFormat.Item;

namespace ApiFormat.ShoppingList
{
    public class ShoppingListDTO : IDTO
    {
        public int GroupID { get; set; }
        public GroupDTO Group { get; set; }
        public List<ItemDTO> Items { get; set; }
        public double Id { get; set; }
        public string Name { get; set; }
    }
}