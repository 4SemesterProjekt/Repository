using System.Collections.Generic;
using ApiFormat.Group;
using ApiFormat.Item;

namespace ApiFormat.Pantry
{
    public class PantryDTO : IDTO
    {
        public double GroupModelID { get; set; }
        public string GroupName { get; set; }
        public List<ItemDTO> Items { get; set; }
        public double Id { get; set; }
        public string Name { get; set; }
    }
}