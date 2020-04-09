using System.Collections.Generic;
using ApiFormat.Item;

namespace ApiFormat.Pantry
{
    public interface IPantryDTO : IDTO
    {
        public int GroupID { get; set; }
        public string GroupName { get; set; }
        public ICollection<IItemDTO> Items { get; set; }
    }
}