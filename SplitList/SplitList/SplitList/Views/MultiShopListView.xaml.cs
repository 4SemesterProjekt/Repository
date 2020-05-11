using SplitList.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SplitList.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MultiShopListView : ContentPage
    {
        public MultiShopListView(int groupId, string userId)
        {
            InitializeComponent();
            BindingContext = new MultiShopListViewModel(Navigation,this, groupId, userId);
        }
    }
}
