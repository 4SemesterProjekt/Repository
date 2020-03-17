using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ClientAPI
{
    public class User
    {
        public int userID { get; set; }
        
        public string name { get; set; }
        
        public ICollection<UserGroup> userGroups { get; set; }
    }

    public class UserGroup
    {
        public int UserID { get; set; }
        public User User { get; set; }
        
        public int GroupID { get; set; }
        public  Group Group { get; set; }
    }
}