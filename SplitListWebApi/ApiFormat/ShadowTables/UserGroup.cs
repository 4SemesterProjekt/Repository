using ApiFormat.User;

namespace ApiFormat.ShadowTables
{
    public class UserGroup
    {
        public int UserID { get; set; }
        public UserModel User { get; set; }
        public int GroupID { get; set; }
        public GroupModel Group { get; set; }
    }
}