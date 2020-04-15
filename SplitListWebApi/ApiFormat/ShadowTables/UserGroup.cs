using ApiFormat.Group;
using ApiFormat.User;

namespace ApiFormat.ShadowTables
{
    public class UserGroup
    {
        public string UserModelId { get; set; }
        public UserModel UserModel { get; set; }

        public int GroupModelModelID { get; set; }
        public GroupModel GroupModel { get; set; }
    }
}