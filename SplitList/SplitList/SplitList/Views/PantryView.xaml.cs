using SplitList.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SplitList.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PantryView : ContentPage
    {
        public PantryView(int groupId, string userId)
        {
            InitializeComponent();
            BindingContext = new PantryViewModel(Navigation, this, groupId, userId);
        }

    }
}