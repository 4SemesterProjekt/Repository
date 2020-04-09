using System.Collections.Generic;
using ApiFormat.ShadowTables;
using Microsoft.AspNetCore.Identity;

namespace ApiFormat.User
{
    public class User : IdentityUser<int>, IUserModel, IUserDTO
    {
        public ICollection<UserGroup> UserGroups { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<IGroupDTO> Groups { get; set; }
    }
}