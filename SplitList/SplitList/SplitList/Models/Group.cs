using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Prism.Mvvm;

namespace SplitList.Models
{
    public class Group : BindableBase
    {
        private string _name;
        private int _groupId;

        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);

        }

        public int GroupId
        {
            get => _groupId;
            set => SetProperty(ref _groupId, value);
        }

    }
}
