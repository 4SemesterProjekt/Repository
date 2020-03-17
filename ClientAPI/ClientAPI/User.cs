using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ClientAPI
{
    public class User
    {
        [JsonPropertyName("userID")]
        public int UserID { get; set; }
        
        [JsonPropertyName("name")]
        public string Name { get; set; }
        
        [JsonPropertyName("userGroups")]
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