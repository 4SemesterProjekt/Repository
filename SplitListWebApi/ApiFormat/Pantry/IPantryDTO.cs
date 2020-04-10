using System.Collections.Generic;
using ApiFormat.Item;

namespace ApiFormat.Pantry
{
    public interface IPantryDTO : IDTO
    {
        double GroupModelID { get; set; }
        string GroupName { get; set; }
        List<IItemDTO> Items { get; set; }
    }
}