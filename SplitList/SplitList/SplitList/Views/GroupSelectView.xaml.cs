using SplitList.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SplitList.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GroupSelectView : ContentPage
    {
        public GroupSelectView(string userId)
        {
            InitializeComponent();
            BindingContext = new GroupSelectViewModel(Navigation, this, 0, userId );
        }
    }
}