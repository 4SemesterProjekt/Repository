using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ApiFormat.ShadowTables;
using Microsoft.AspNetCore.Identity;

namespace ApiFormat.User
{
    public class UserModel : IdentityUser, IModel
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ModelId { get; set; }
        public string Name { get; set; }

        public List<UserGroup> UserGroups { get; set; }
    }
}