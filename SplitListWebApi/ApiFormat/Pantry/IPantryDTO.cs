using System.Collections.Generic;
using ApiFormat.Item;

namespace ApiFormat.Pantry
{
    public interface IPantryDTO : IDTO
    {
        int GroupID { get; set; }
        string GroupName { get; set; }
        List<IItemDTO> Items { get; set; }
    }
}