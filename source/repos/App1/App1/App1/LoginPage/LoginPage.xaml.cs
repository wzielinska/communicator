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
            vm.DisplayInvalidConfirmationPrompt += () => DisplayAlert("Error", "Passwords do not match!", "OK");
            vm.DisplayInvalidRegisterPrompt += () => DisplayAlert("Error", "Invalid Register, try again!", "OK");
            

        InitializeComponent ();

            Username.Completed += (object sender, EventArgs e) =>
            {
                Password.Focus();
            };
            
		}
	}
}