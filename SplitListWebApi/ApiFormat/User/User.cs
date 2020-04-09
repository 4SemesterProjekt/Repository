using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using ApiFormat.ShadowTables;
using Microsoft.AspNetCore.Identity;

namespace ApiFormat.User
{
    public class User : UserModel, IUserDTO
    {
        public List<IGroupDTO> Groups { get; set; }
    }
}