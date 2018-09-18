
using App1;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

[assembly: Xamarin.Forms.Dependency(typeof(PageNavigation))]

namespace App1
{
    
    class PageNavigation : IPageNavigation
    {
        #region methods
        public void CreateLoginPage()
        {
            Application.Current.MainPage = new NavigationPage(new LoginPage());
        }

        public void GoBackToLoginPage()
        {
            Application.Current.MainPage.Navigation.PopToRootAsync();
        }

        public void GoBackToMessenger()
        {
            Application.Current.MainPage.Navigation.PopAsync();
        }

        public void GoToChat(object bindingContext)
        {
            Application.Current.MainPage.Navigation.PushAsync(new ChatPage_.ChatPage()
            {
                BindingContext = bindingContext
            });
        }

        public void GoToMessenger(object bindingContext)
        {
           Application.Current.MainPage.Navigation.PushAsync(new MessengerPage_.MessengerPage() {
                BindingContext = bindingContext
            });
        }
        #endregion

    }
}
