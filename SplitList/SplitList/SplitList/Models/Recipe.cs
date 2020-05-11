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
            Ingredients = new ObservableCollection<Item>();
            Name = "";
            Introduction = "";
            Instructions = "";
            Id = 0;
        }

        public Recipe(string name, string introduction, string instructions)
        {
            Ingredients = new ObservableCollection<Item>();
            Name = name;
            Introduction = introduction;
            Instructions = instructions;
            Id = 0;
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


        private ObservableCollection<Item> _ingredients;

        public ObservableCollection<Item> Ingredients
        {
            get => _ingredients; 
            set => SetProperty(ref _ingredients, value);
        }

        private string _instructions;
        public string Instructions { 
            get => _instructions; 
            set => SetProperty(ref _instructions, value); }


        #endregion
    }
}
