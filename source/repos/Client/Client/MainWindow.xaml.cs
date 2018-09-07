using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        BKClient client = new BKClient();

        AvailableEventHandler AvailableHandler;
        ReceivedEventHandler ReceivedHandler;

        public MainWindow()
        {
            //Events
            InitializeComponent();

            client.LoginOK += new EventHandler(_LoginOK);
            client.RegisterOK += new EventHandler(_RegisterOK);
            client.LoginFailed += new ErrorEventHandler(_LoginFailed);
            client.RegisterFailed += new ErrorEventHandler(_RegisterFailed);
            client.Disconnected += new EventHandler(_Disconnected);

            AvailableHandler = new AvailableEventHandler(_UserAvailable);
            ReceivedHandler = new ReceivedEventHandler(_MessegeReceived);

            client.UserAvailable += AvailableHandler;
            client.MessageReceived += ReceivedHandler;

        }

       
        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
               
                client.IsAvailable(Receiver.Text);

                Chat.Text = "Chat started!\r\n";
                StartButton.IsEnabled = false;
                EndButton.IsEnabled = true;
                SendMessageButton.IsEnabled = true;
                Receiver.IsEnabled = false;
            }), DispatcherPriority.Background);
        }

        private void EndButton_Click(object sender, RoutedEventArgs e)
        {
            

            Dispatcher.BeginInvoke(new Action(() =>
            {
                Chat.Text = "Chat ended!";
                EndButton.IsEnabled = false;
                StartButton.IsEnabled = true;
                SendMessageButton.IsEnabled = false;
                Receiver.IsEnabled = true;
            }), DispatcherPriority.Background);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            client.Login(UsernameText.Text, PasswordText.Password);
            Status.Content = "Login...";
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            client.Register(UsernameText.Text, PasswordText.Password);
            Status.Content = "Register...";
        }

        private void SendMessageButton_Click(object sender, RoutedEventArgs e)
        {
            client.SendMessage(Receiver.Text, MessageText.Text);
            Chat.Text += String.Format("[{0}] {1}\r\n", client.Username, MessageText.Text);
            MessageText.Text = "";
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            client.Disconnect();
            Status.Content = "Disconnecting...";
        }

        private void _MessegeReceived(object sender, ReceivedEventArgs e)
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                    Chat.Text += String.Format("[{0}] {1}\r\n", e.Username, e.Message);
            }), DispatcherPriority.Background);
        }

        bool lastAvailable;

        private void _UserAvailable(object sender, AvailableEventArgs e)
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                if (e.Username == Receiver.Text)
                {
                    if(lastAvailable != e.IsAvailable)
                    {
                        lastAvailable = e.IsAvailable;
                        Chat.Text += String.Format("[{0}] is {1}\r\n", Receiver.Text, (e.IsAvailable ? "available" : "unavailable"));
                    }
                }
            }), DispatcherPriority.Background);
        }


        void _LoginOK(object sender, EventArgs e)
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                Status.Content = "Logged in!";
                RegisterButton.IsEnabled = false;
                LoginButton.IsEnabled = false;
                LogoutButton.IsEnabled = true;
                StartButton.IsEnabled = true;
            }), DispatcherPriority.Background);
        }
        private void _Disconnected(object sender, EventArgs e)
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                Status.Content = "Disconnected!";
                RegisterButton.IsEnabled = true;
                LoginButton.IsEnabled = true;
                LogoutButton.IsEnabled = false;
                StartButton.IsEnabled = false;
            }), DispatcherPriority.Background);
        }

        private void _RegisterFailed(object sender, ErrorEventArgs e)
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                Status.Content = "Register Failed";
            }), DispatcherPriority.Background);
        }

        private void _LoginFailed(object sender, ErrorEventArgs e)
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                Status.Content = "Login Failed";
            }), DispatcherPriority.Background);
        }

        private void _RegisterOK(object sender, EventArgs e)
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                Status.Content = "Registered!";
                RegisterButton.IsEnabled = false;
                LoginButton.IsEnabled = false;
                LogoutButton.IsEnabled = true;
                SendMessageButton.IsEnabled = true;
            }), DispatcherPriority.Background);
        }
    }
}
