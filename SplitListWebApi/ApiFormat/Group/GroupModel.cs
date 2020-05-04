using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ApiFormat.Pantry;
using ApiFormat.ShadowTables;
using ApiFormat.ShoppingList;

namespace ApiFormat
{
    public class GroupModel : IModel
    {
        public PantryModel PantryModel { get; set; }
        public List<ShoppingListModel> ShoppingLists { get; set; }
        public List<UserGroup> UserGroups { get; set; }

        [Key]
        public int ModelId { get; set; }
        public string Name { get; set; }
        public string OwnerId { get; set; }
    }
}