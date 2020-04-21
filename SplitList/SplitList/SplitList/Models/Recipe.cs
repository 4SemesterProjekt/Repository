using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Prism.Mvvm;

namespace SplitList.Models
{
    public class Recipe : BindableBase
    {
        public Recipe()
        {
            Items = new ObservableCollection<Item>();
            Name = "";
            Introduction = "";
            Instructions = "";
        }

        public Recipe(int id, string name, string introduction, string instructions)
        {
            Items = new ObservableCollection<Item>();
            Name = name;
            Introduction = introduction;
            Instructions = instructions;
            Id = id;
        }

        #region Properties

        private int _id;

        public int Id
        {
            get => _id; 
            set => SetProperty(ref _id, value);
        }

        private string _name;
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }


        private string _introduction;
        public string Introduction
        {
            get => _introduction;
            set => SetProperty(ref _introduction, value);
        }


        private ObservableCollection<Item> _items;

        public ObservableCollection<Item> Items
        {
            get => _items; 
            set => SetProperty(ref _items, value);
        }

        private string _instructions;
        public string Instructions { 
            get => _instructions; 
            set => SetProperty(ref _instructions, value); }


        #endregion
    }
}
