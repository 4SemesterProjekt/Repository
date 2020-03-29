using System;
using System.Collections.Generic;
using System.Text;

namespace ApiFormat
{
    public class ItemDTO
    {
        public int ItemID { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public int Amount { get; set; }
    }
}
