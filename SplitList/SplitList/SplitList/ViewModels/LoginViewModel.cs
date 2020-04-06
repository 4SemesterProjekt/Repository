using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Android.Support.Design.Behavior;
using Prism.Commands;
using Prism.Mvvm;
using SplitList.Views;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace SplitList.ViewModels
{
    class LoginViewModel
    {
        private Uri AuthenticationUri = new Uri("https://splitlistwebapi.azurewebsites.net/api/account/googleresponse"); //URL på vores webapi, som appen skal requeste et link til en loginside fra.
        private Uri CallBackUri = new Uri("SplitList://"); //URL til vores app, som loginsiden bruger til at åbne for appen, når login er gennemført.

        private ICommand _LoginCommand;
        public ICommand LoginCommand
        {
            get => _LoginCommand ?? (_LoginCommand = new DelegateCommand(LoginCommandExecute));
        }

        //Laver en http-request til vores web-api, bliver viderestillet til Google-login-siden. Efter login sendes brugeren tilbage til appen via callbackURL.
        public async void LoginCommandExecute()
        {
            WebAuthenticatorResult authResult;
            string accessToken = "";

            try
            {
                authResult = await WebAuthenticator.AuthenticateAsync(AuthenticationUri, CallBackUri);

                accessToken = authResult?.AccessToken;
            }
            catch (System.Threading.Tasks.TaskCanceledException e)
            {

            }

            if(accessToken != "")
            {
                //Application.Current.MainPage = new MDP();
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Login failed. Try again.", "OK");
            }
        }
    }
}