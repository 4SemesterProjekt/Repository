using System.Collections.Generic;

namespace ApiFormat.User
{
    public interface IUserDTO : IDTO
    {
        List<IGroupDTO> Groups { get; set; }
    }
}