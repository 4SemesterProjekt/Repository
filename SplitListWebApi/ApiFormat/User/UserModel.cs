using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ApiFormat.ShadowTables;
using Microsoft.AspNetCore.Identity;

namespace ApiFormat.User
{
    public class UserModel : IdentityUser<int>, IModel
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public override int Id { get; set; }
        public List<UserGroup> UserGroups { get; set; }
        public string Name { get; set; }
    }
    public class ApplicationRole : IdentityRole<int>
    {

    }
}