using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using SplitListWebApi.Models;

namespace SplitListWebApi.Areas.Identity.Data.Models
{
    public class Group
    {
        public int GroupID { get; set; }
        public string Name { get; set; }
        public string OwnerID { get; set; }
        public Pantry Pantry { get; set; }
        public ICollection<ShoppingList> ShoppingLists { get; set; }
        public ICollection<UserGroup> UserGroups { get; set; }
    }
}