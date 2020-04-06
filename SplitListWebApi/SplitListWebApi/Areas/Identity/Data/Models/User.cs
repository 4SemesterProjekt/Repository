using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using SplitListWebApi.Areas.Identity.Data.Models;

namespace SplitListWebApi.Models
{
    public class User : IdentityUser
    {
        public string Name { get; set; }
        public ICollection<UserGroup> UserGroups { get; set; }
    }

    public class UserGroup
    {
        public string Id { get; set; }
        public User User { get; set; }
        
        public int GroupID { get; set; }
        public  Group Group { get; set; }
    }
}