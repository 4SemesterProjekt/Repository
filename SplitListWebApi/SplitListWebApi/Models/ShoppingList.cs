using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SplitListWebApi.Models
{
    public class ShoppingList
    {
        public int ShoppingListID { get; set; }
        public string Name { get; set; }
        
        [ForeignKey("GroupID")]
        public int GroupID { get; set; }
        public Group Group { get; set; }
        
        public ICollection<ShoppingListItem> ShoppingListItems { get; set; }
    }

    public class ShoppingListItem
    {
        public int ShoppingListID { get; set; }
        public ShoppingList ShoppingList { get; set; }
        
        public int ItemID { get; set; }
        public Item Item { get; set; }
        
        public int Amount { get; set; }
    }
}