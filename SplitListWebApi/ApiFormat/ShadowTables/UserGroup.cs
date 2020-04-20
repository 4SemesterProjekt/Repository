using ApiFormat.Group;
using ApiFormat.User;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiFormat.ShadowTables
{
    public class UserGroup
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string UserId { get; set; }
        public UserModel UserModel { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int GroupModelModelID { get; set; }
        public GroupModel GroupModel { get; set; }
    }
}