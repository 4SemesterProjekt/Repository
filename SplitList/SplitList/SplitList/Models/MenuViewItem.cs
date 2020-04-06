using System;
using System.Collections.Generic;
using System.Text;
using SplitList.Utility;
using Xamarin.Forms;

namespace SplitList.Models
{
    class MenuViewItem
    {
        public string Title { get; set; }

        public ImageSource ImageSource { get; set; }

        public Type TargetType { get; set; }
    }
}
