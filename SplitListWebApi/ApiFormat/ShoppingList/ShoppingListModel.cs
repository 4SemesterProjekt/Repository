using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ApiFormat.Group;
using ApiFormat.ShadowTables;

namespace ApiFormat.ShoppingList
{
    public class ShoppingListModel : IModel
    {
        [Key]
        public int ModelId { get; set; }
        public string Name { get; set; }

        public int GroupModelId { get; set; }
        public GroupModel GroupModel { get; set; }

        public List<ShoppingListItem> ShoppingListItems { get; set; }
    }
}