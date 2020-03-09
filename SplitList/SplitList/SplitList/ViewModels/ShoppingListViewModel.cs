using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Prism.Mvvm;
using SplitList.Models;

namespace SplitList.ViewModels
{
    public class ShoppingListViewModel : BindableBase
    { 
    public ShoppingListViewModel()
    {
    _items = new ObservableCollection<Item>();
    _items.Add(new Item("Banana", 4, "Fruit"));
    _items.Add(new Item("Apple", 3, "Fruit"));
    _items.Add(new Item("Milk", 1, "Dairy"));
    _items.Add(new Item("Rye Bread", 1, "Bread"));
    }

    private ObservableCollection<Item> _items;

    public ObservableCollection<Item> Items
    {
    get => _items;
    set => SetProperty(ref _items, value);
    }
}
