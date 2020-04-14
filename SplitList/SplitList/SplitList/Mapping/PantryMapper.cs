using System.Collections.Generic;
using ApiFormat;
using SplitList.Models;

namespace SplitList.Mapping
{
    public class PantryMapper
    {
        public static Pantry PantryDtoToPantry(PantryDTO dto)
        {
            Pantry newPantry = new Pantry();
            newPantry.Name = dto.Name;
            newPantry.GroupName = dto.GroupName;
            newPantry.GroupId = dto.GroupID;
            newPantry.PantryId = dto.ID;
            if (dto.Items != null)
            {
                foreach (var dtoItem in dto.Items)
                {
                    newPantry.Items.Add(ItemMapper.ItemDtoToItem(dtoItem));
                }
            }

            return newPantry;
        }

        public static PantryDTO PantryToPantryDto(Pantry pantry)
        {
            PantryDTO newDto = new PantryDTO();
            newDto.Items = new List<ItemDTO>();
            newDto.GroupID = pantry.GroupId;
            newDto.Name = pantry.Name;
            newDto.GroupName = pantry.GroupName;
            newDto.ID = pantry.PantryId;
            if (pantry.Items != null)
            {
                foreach (var pantryItem in pantry.Items)
                {
                    newDto.Items.Add(ItemMapper.ItemToItemDto(pantryItem));
                }
            }

            return newDto;
        }
    }
}