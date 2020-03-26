using System;
using System.Collections.Generic;
using System.Text;

namespace ApiFormat
{
    class PantryDTO
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int GroupID { get; set; }
        public string GroupName { get; set; }
        public List<ItemDTO> Items { get; set; }
    }
}
