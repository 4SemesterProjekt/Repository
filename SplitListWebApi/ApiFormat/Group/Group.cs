using System.Collections.Generic;
using ApiFormat.Pantry;
using ApiFormat.ShoppingList;
using ApiFormat.User;

namespace ApiFormat
{
    public class Group : IGroupModel, IGroupDTO
    {
        private IPantryModel _pantry;
        private ICollection<IShoppingListModel> _shoppingLists;
        private ICollection<IShoppingListDTO> _shoppingLists1;
        private IPantryDTO _pantry1;
        public int Id { get; set; }
        public string Name { get; set; }
        public int OwnerId { get; set; }

        ICollection<IShoppingListDTO> IGroupDTO.ShoppingLists
        {
            get => _shoppingLists1;
            set => _shoppingLists1 = value;
        }

        IPantryDTO IGroupDTO.Pantry
        {
            get => _pantry1;
            set => _pantry1 = value;
        }

        public ICollection<IUserDTO> Users { get; set; }

        IPantryModel IGroupModel.Pantry
        {
            get => _pantry;
            set => _pantry = value;
        }

        public string OwnerID { get; set; }

        ICollection<IShoppingListModel> IGroupModel.ShoppingLists
        {
            get => _shoppingLists;
            set => _shoppingLists = value;
        }
    }
}