using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClientAPI
{
    public class User
    {
        public int UserID { get; set; }
        public string Name { get; set; }
        
        public ICollection<UserGroup> UserGroups { get; set; }
    }

    public class UserGroup
    {
        public int UserID { get; set; }
        public User User { get; set; }
        
        public int GroupID { get; set; }
        public  Group Group { get; set; }
    }
}