using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SplitList.Utility
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NavigationItem : Grid
    {
        public NavigationItem()
        {
            InitializeComponent();
        }

        public static readonly BindableProperty TextProperty = BindableProperty.Create(
            nameof(Text),
            typeof(string),
            typeof(NavigationItem),
            string.Empty,
            propertyChanging: (bindable, oldValue, newValue) =>
            {
                var ctrl = (NavigationItem)bindable;
                ctrl.Text = (string)newValue;
            },
            defaultBindingMode: BindingMode.OneWay);

        private string _text;

        public string Text
        {
            get { return _text; }
            set
            {
                _text = value;
                OnPropertyChanged();
            }
        }
    }
}