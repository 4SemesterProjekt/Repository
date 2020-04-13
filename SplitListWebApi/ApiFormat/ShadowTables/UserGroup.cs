using ApiFormat.Group;
using ApiFormat.User;

namespace ApiFormat.ShadowTables
{
    public class UserGroup
    {
        public double UserID { get; set; }
        public UserModel User { get; set; }
        public double GroupID { get; set; }
        public GroupModel Group { get; set; }
    }
}