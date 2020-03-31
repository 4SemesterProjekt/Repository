using System;
using System.Collections.Generic;

namespace ApiFormat
{
    public class PantryDTO
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int GroupID { get; set; }
        public string GroupName { get; set; }
        public List<ItemDTO> Items { get; set; }
    }
}
