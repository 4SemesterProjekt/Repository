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
    public partial class GroupEditView : ContentPage
    {
        public GroupEditView(int groupId)
        {
            InitializeComponent();
            BindingContext = new GroupEditViewModel(groupId);
        }
    }
}