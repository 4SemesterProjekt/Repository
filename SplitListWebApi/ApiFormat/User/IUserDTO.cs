using System.Collections.Generic;

namespace ApiFormat.User
{
    public interface IUserDTO : IDTO
    {
        public ICollection<IGroupDTO> Groups { get; set; }
    }
}