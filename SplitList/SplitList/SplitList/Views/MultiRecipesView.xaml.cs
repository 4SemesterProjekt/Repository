using SplitList.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SplitList.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MultiRecipesView : ContentPage
    {
        public MultiRecipesView(int groupId, string userId)
        {
            InitializeComponent();
            BindingContext = new MultiRecipesViewModel(Navigation, this, groupId, userId);
        }
    }
}