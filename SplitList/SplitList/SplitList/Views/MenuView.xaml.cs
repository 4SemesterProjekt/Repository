using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SplitList.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SplitList.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MenuView : ContentPage
    {
        public MenuView()
        {
            InitializeComponent();
            BindingContext = new MenuViewModel();
            NavListView = ListView;
            NavListView1 = ListView1;
        }

        public static ListView NavListView { get; set; }
        public static ListView NavListView1 { get; set; }

    }
}