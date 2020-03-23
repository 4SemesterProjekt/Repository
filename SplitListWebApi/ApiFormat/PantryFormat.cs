using System;
using System.Collections.Generic;
using System.Text;

namespace ApiFormat
{
    class PantryFormat
    {
        public int PantryID { get; set; }
        public string Name { get; set; }
        public int GroupID { get; set; }
        public List<ItemFormat> PantryItems { get; set; }
    }
}
