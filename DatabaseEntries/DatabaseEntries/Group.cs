using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DatabaseEntries
{
    public class Group
    {
        public int GroupID { get; set; }
        public string Name { get; set; }
        public int OwnerID { get; set; }
        public ICollection<Pantry> Pantries { get; set; }
        public ICollection<ShoppingList> ShoppingLists { get; set; }
        public ICollection<UserGroup> UserGroups { get; set; }
    }
}