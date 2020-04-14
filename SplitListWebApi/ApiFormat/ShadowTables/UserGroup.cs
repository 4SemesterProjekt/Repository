using ApiFormat.Group;
using ApiFormat.User;

namespace ApiFormat.ShadowTables
{
    public class UserGroup
    {
        public double UserModelID { get; set; }
        public UserModel UserModel { get; set; }
        public double GroupModelID { get; set; }
        public GroupModel GroupModel { get; set; }
    }
}