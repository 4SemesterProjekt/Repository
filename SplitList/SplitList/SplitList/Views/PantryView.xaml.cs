using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApiFormat;
using ClientLibAPI;
using SplitList.Mapping;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SplitList.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PantryView : ContentPage
    {
        public PantryView()
        {
            InitializeComponent();
            PantryViewModel.Page = this;
            //ToDo Get GroupID from user active group
        }

        protected override async void OnDisappearing()
        {
            base.OnDisappearing();
            PantryDTO dto = PantryMapper.PantryToPantryDto(PantryViewModel.Pantry);
            var result = await SerializerPantry.PostPantry(dto);
        }
    }
}