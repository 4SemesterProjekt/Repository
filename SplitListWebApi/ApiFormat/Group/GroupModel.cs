using System.Collections.Generic;
using ApiFormat.Pantry;
using ApiFormat.ShadowTables;
using ApiFormat.ShoppingList;

namespace ApiFormat
{
    public class GroupModel : IModel
    {
        public int OwnerId { get; set; }
        public PantryModel Pantry { get; set; }
        public List<ShoppingListModel> ShoppingLists { get; set; }
        public List<UserGroup> UserGroups { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
    }
}