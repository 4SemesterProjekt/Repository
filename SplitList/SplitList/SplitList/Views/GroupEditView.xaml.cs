using SplitList.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SplitList.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GroupEditView : ContentPage
    {
        public GroupEditView(int groupId, string userId)
        {
            InitializeComponent();
            BindingContext = new GroupEditViewModel(Navigation, this, groupId, userId);
        }
    }
}