using System.Collections.Generic;
using SplitListWebApi.Models;

namespace SplitListWebApi.Areas.Identity.Data.Models
{
    public class Group
    {
        public int GroupID { get; set; }
        public string Name { get; set; }
        public string OwnerID { get; set; }
        public ICollection<Pantry> Pantries { get; set; }
        public ICollection<ShoppingList> ShoppingLists { get; set; }
        public ICollection<UserGroup> UserGroups { get; set; }
    }
}