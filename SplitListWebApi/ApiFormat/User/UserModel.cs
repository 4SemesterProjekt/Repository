using System.Collections.Generic;
using ApiFormat.ShadowTables;
using Microsoft.AspNetCore.Identity;

namespace ApiFormat.User
{
    public class UserModel : IdentityUser<int>, IModel
    {
        public List<UserGroup> UserGroups { get; set; }
        public string Name { get; set; }
    }
    public class ApplicationRole : IdentityRole<int>
    {

    }
}