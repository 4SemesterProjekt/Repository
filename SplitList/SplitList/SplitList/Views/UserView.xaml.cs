using SplitList.Models;
using SplitList.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SplitList.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UserView : ContentPage
    {
        public UserView(User user, int groupId, string userId)
        {
            InitializeComponent();
            BindingContext = new UserViewModel(Navigation, this, groupId, userId);
        }
    }
}