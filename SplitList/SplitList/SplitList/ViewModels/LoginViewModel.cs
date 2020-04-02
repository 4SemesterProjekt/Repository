using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Prism.Commands;
using Prism.Mvvm;
using SplitList.Views;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace SplitList.ViewModels
{
    class LoginViewModel
    {
        private Uri AuthenticationUri = new Uri(""); //URL på vores webapi, som appen skal requeste et link til en loginside fra.
        private Uri CallBackUri = new Uri("SplitList://"); //URL til vores app, som loginsiden bruger til at åbne for appen, når login er gennemført.

        private ICommand _LoginCommand;
        public ICommand LoginCommand
        {
            get => _LoginCommand ?? (_LoginCommand = new DelegateCommand(LoginCommandExecute));
        }

        //Laver en http-request til vores web-api, bliver viderestillet til Google-login-siden. Efter login sendes brugeren tilbage til appen via callbackURL.
        public void LoginCommandExecute()
        {
            var authResult = WebAuthenticator.AuthenticateAsync(AuthenticationUri, CallBackUri);

            var accesToken = authResult?.Result;

            if(accesToken != null)
            {
                //To Do: Send accesToken til web-api og åben for MultiShopListView.


            }
            else
            {
                Application.Current.MainPage.DisplayAlert("Error", "Login failed. Try again.", "OK");
            }


        }
    }




}