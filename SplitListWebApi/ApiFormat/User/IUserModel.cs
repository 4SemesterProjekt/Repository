using System.Collections.Generic;
using ApiFormat.ShadowTables;

namespace ApiFormat.User
{
    public interface IUserModel
    {
        public ICollection<UserGroup> UserGroups { get; set; }
    }
}