using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App1
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class LoginPage : ContentPage
	{
        LoginPageViewModel vm = new LoginPageViewModel();

        public LoginPage ()
		{
            this.BindingContext = vm;
            vm.DisplayInvalidLoginPrompt += () => DisplayAlert("Error", "Invalid Login, try again!", "OK");
            vm.DisplayUserNoExist += () => DisplayAlert("Error", "User do not exist, try again!", "OK");
            vm.DisplayWrongPassword += () => DisplayAlert("Error", "Wrong Password, try again!", "OK");
            vm.DisplayUserExist += () => DisplayAlert("Error", "User already exists, try again!", "OK");
            vm.DisplayInvalidConfirmationPrompt += () => DisplayAlert("Error", "Passwords do not match!", "OK");
            vm.DisplayInvalidRegisterPrompt += () => DisplayAlert("Error", "Invalid Register, try again!", "OK");
            vm.DisplayInvalidChatUserPrompt += () => DisplayAlert("Error", "Choose user!", "OK");
            vm.DisplayNoFriendName += () => DisplayAlert("Error", "Enter Valid Friend's Name!", "OK");
            vm.DisplayRcvMssg += () => DisplayAlert("Message Received", "You Received Message from " + vm.sender , "OK");
            vm.DisplayUnavailable += () => DisplayAlert("Error", "Username you are trying to reach is currently unavailable!", "OK");

            InitializeComponent ();

            Username.Completed += (object sender, EventArgs e) =>
            {
                Password.Focus();
            };
            
		}
	}
}