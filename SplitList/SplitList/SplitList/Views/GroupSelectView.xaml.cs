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
    public partial class GroupSelectView : ContentPage
    {
        public GroupSelectView()
        {
            InitializeComponent();
            BindingContext = new GroupSelectViewModel();
        }
    }
}