using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SplitListWebApi.Models
{
    public class Group
    {
        public int GroupID { get; set; }
        public string Name { get; set; }
        
        [ForeignKey("OwnerID")]
        public User Owner { get; set; }
        
        public ICollection<Pantry> Pantries { get; set; }
        public ICollection<ShoppingList> ShoppingLists { get; set; }
        
        public ICollection<UserGroup> UserGroups { get; set; }
    }
}