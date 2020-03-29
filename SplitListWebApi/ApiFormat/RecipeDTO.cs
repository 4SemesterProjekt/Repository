using System;
using System.Collections.Generic;
using System.Text;

namespace ApiFormat
{
    class RecipeDTO
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public List<ItemDTO> Items { get; set; }
    }
}
