using System;
using System.Collections.Generic;
using System.Text;

namespace ApiFormat
{
    class PantryDTO
    {
        public int PantryID { get; set; }
        public string Name { get; set; }
        public int GroupID { get; set; }
        public List<ItemDTO> PantryItems { get; set; }
    }
}
