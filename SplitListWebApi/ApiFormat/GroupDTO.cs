﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ApiFormat
{
    class GroupDTO
    {
        public int GroupID { get; set; }
        public string Name { get; set; }
        public int OwnerID { get; set; }
        public List<ShoppingListDTO> ShoppingLists { get; set; }
        public List<PantryDTO> Pantries { get; set; }
        public List<UserDTO> Users { get; set; }
    }
}
