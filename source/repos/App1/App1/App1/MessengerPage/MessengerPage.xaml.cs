using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App1.MessengerPage_
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MessengerPage : ContentPage
    {


        public MessengerPage()
        {
            InitializeComponent();
            
        }

        private void btnAddFriendButton_Clicked(object sender, EventArgs e)
        {
            popupAddFriendView.IsVisible = true;
            activityIndicator.IsRunning = true;
        }
        private void popupAddFriendViewButton_Clicked(object sender, EventArgs e)
        {
            popupAddFriendView.IsVisible = false;
        }
    }
}