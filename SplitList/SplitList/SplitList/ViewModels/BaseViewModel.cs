using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using AutoMapper;
using Prism.Commands;
using Prism.Mvvm;
using SplitList.AutoMapper;
using SplitList.Models;
using Xamarin.Forms;

namespace SplitList.ViewModels
{
    public abstract class BaseViewModel : BindableBase
    {
        protected BaseViewModel(INavigation nav, Page page, int groupId)
        {
            Navigation = nav;
            Page = page;
            GroupId = groupId;

            var config = new MapperConfiguration(
                cfg =>
                {
                    cfg.AddProfile<GroupProfile>();
                    cfg.AddProfile<ItemProfile>();
                    cfg.AddProfile<PantryProfile>();
                    cfg.AddProfile<ShoppingListProfile>();
                    cfg.AddProfile<UserProfile>();
                });
        }
        public IMapper mapper { get; set; }
        public Page Page { get; set; }
        public INavigation Navigation { get; set; }

        public int GroupId { get; set; }

        private ICommand _onAppearingCommand;

        public ICommand OnAppearingCommand => _onAppearingCommand ?? (_onAppearingCommand = new DelegateCommand(OnAppearingExecute));

        public virtual async void OnAppearingExecute() { }

        private ICommand _onDisappearingCommand;

        public ICommand OnDisappearingCommand => _onDisappearingCommand ?? (_onDisappearingCommand = new DelegateCommand(OnDisappearingExecute));

        public virtual async void OnDisappearingExecute() { }
    }
}
