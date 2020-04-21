﻿using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using SplitList.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SplitList.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MultiShopListView : ContentPage
    {
        public MultiShopListView(int groupId)
        {
            InitializeComponent();
            BindingContext = new MultiShopListViewModel(Navigation,this, groupId);
        }
    }
}
