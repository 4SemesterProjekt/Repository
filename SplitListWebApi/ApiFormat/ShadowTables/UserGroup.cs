using ApiFormat.User;

namespace ApiFormat.ShadowTables
{
    public class UserGroup
    {
        public string UserID { get; set; }
        public IUserModel User { get; set; }
        public int GroupID { get; set; }
        public IGroupModel Group { get; set; }
    }
}