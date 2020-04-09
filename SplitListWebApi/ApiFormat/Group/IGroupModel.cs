using System.Collections.Generic;
using ApiFormat.Pantry;
using ApiFormat.ShoppingList;

namespace ApiFormat
{
    public interface IGroupModel : IModel
    {
        int OwnerId { get; set; }
        IPantryModel Pantry { get; set; }
        ICollection<IShoppingListModel> ShoppingLists { get; set; }
    }
}