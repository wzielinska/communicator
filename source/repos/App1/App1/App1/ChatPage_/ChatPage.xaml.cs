using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App1.ChatPage_
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ChatPage : ContentPage
	{
        LoginPageViewModel vm;
		public ChatPage ()
		{
			InitializeComponent ();
            Title = "ktoś tam";
            BindingContext = vm = new LoginPageViewModel();

            vm.Messages.CollectionChanged += (sender, e) =>
            {
                var target = vm.Messages[vm.Messages.Count - 1];
                MessagesListView.ScrollTo(target, ScrollToPosition.End, true);
            };
		}
	}
}