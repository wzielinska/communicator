using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Windows.Input;
using Xamarin.Forms;
using System.Runtime.CompilerServices;

namespace App1
{
    class LoginPageViewModel : INotifyPropertyChanged
    {

        #region fields
        private readonly IPageNavigation _navigation;

        #endregion

        #region properties
        public Command GoToMessengerPageCommand { get; set; }

        public Command GoBackToLoginPageCommand { get; set; }

        BKClient client = new BKClient();

        public ICommand LoginCommand { protected set; get; }

        public ICommand RegisterCommand { protected set; get; }

        public ICommand LoginAndGo { get; set; }

        public ICommand RegisterAndGo
        {
            get
            {
                return new Command(() =>
                {
                    RegisterCommand.Execute(null);
                    GoToMessengerPageCommand.Execute(null);
                });
            }
        }

        public Action DisplayInvalidLoginPrompt;

        public Action DisplayInvalidConfirmationPrompt;

        public Action DisplayInvalidRegisterPrompt;
        #endregion

        #region LoginCredentials
        private string _username;
        public string Username
        {
            get { return _username; }
            set
            {
                _username = value;
                OnPropertyChanged(nameof(Username));
            }
        }

        private string _password;
        public string Password
        {
            get { return _password; }
            set
            {

                _password = value;
                OnPropertyChanged(nameof(Password));
            }
        }

        private string _confirmpassword;
        public string ConfirmPassword
        {
            get { return _confirmpassword; }
            set
            {
                _confirmpassword = value;
                OnPropertyChanged(nameof(ConfirmPassword));
            }
        }
        #endregion

        

        public LoginPageViewModel()
        {
            _navigation = DependencyService.Get<IPageNavigation>();

            GoToMessengerPageCommand = new Command(ExecuteGoToMessengerPage);

            GoBackToLoginPageCommand = new Command(ExecuteGoBackToLoginPage);

            LoginCommand = new Command(OnLogin);

            RegisterCommand = new Command(OnRegister);

            client.LoginOK += new EventHandler(_LoginOK);

            client.RegisterOK += new EventHandler(_RegisterOK);

            client.LoginFailed += new ErrorEventHandler(_LoginFailed);

            client.RegisterFailed += new ErrorEventHandler(_RegisterFailed);
        }

        #region methods

        public void OnLogin()
        {
            if (_username != null && _password != null)
            {
                client.Login(_username, _password);
            }
            else DisplayInvalidLoginPrompt();
        }

        public void OnRegister()
        {
            if (_username != null && _password != null)
                if (_password == _confirmpassword) client.Register(_username, _password);
                else DisplayInvalidConfirmationPrompt();
            else DisplayInvalidRegisterPrompt();
        }

        void _LoginOK(object sender, EventArgs e)
        {

        }

        void _RegisterOK(object sender, EventArgs e)
        {

        }

        void _LoginFailed(object sender, ErrorEventArgs e)
        {
            DisplayInvalidLoginPrompt();
        }

        void _RegisterFailed(object sender, ErrorEventArgs e)
        {
            DisplayInvalidLoginPrompt();
        }

        private void ExecuteGoToMessengerPage()
        {
            _navigation.GoToMessenger(this);
        }

        private void ExecuteGoBackToLoginPage()
        {
            _navigation.GoBackToLoginPage();
        }
        #endregion

        #region InotifyPropertyChanged implementation
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
