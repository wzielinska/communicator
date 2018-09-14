using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Windows.Input;
using Xamarin.Forms;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;

namespace App1
{
    class LoginPageViewModel : INotifyPropertyChanged
    {

        #region fields
        private readonly IPageNavigation _navigation;

        #endregion

        #region properties


        BKClient client = new BKClient();

        public Command AddFriend { get; set; }

        public Command RemoveFriend { get; set; }

        public Command GoBackToMessengerPageCommand { get; set; }

        public Command GoToChatPageCommand { get; set; }

        public ICommand LoginAndGoCommand
        {
            get
            {
                return new Command(() =>
                {
                    //OnLogin();
                    if (OnLogin()) ExecuteGoToMessengerPage();
                });
            }
        }

        public ICommand RegisterAndGoCommand
        {
            get
            {
                return new Command(() =>
                {
                    if (OnRegister()) ExecuteGoToMessengerPage();
                });
            }
        }

        public ICommand LogoutAndBackCommand
        {
            get
            {
                return new Command(() =>
                {
                    OnLogout();
                    ExecuteGoBackToLoginPage();
                });
            }
        }

        public Action DisplayInvalidLoginPrompt;

        public Action DisplayInvalidConfirmationPrompt;

        public Action DisplayInvalidRegisterPrompt;

        public Action DisplayUserNoExist;

        public Action DisplayWrongPassword;

        public Action DisplayUserExist;

        public Action DisplayInvalidChatUserPrompt;

        public Action DisplayNoFriendName;

        private string _addfriend;
        public string addFriend
        {
            get { return _addfriend; }
            set
            {
                if (_addfriend != value)
                {
                    _addfriend = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _ItemSelected;
        public string objItemSelected
        {
            get { return _ItemSelected; }
            set
            {
                if (_ItemSelected != value)
                {
                    _ItemSelected = value;
                    OnPropertyChanged("Item Selected");
                }
            }
        }

        private ObservableCollection<string> _FriendList;
        public ObservableCollection<string> FriendList
        {
            get { if (_FriendList != null) return _FriendList;
                else return _FriendList = new ObservableCollection<string>();
            }
            set
            {
                if (_FriendList !=value)
                {
                    _FriendList = value;
                    OnPropertyChanged();
                }
            }
        }
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

            GoBackToMessengerPageCommand = new Command(ExecuteGoBackToMessengerPage);

            GoToChatPageCommand = new Command(ExecuteGoToChatPage);
            
            AddFriend = new Command(ExecuteAddFriend);

            RemoveFriend = new Command(ExecuteRemoveFriend);

            client.LoginOK += new EventHandler(_LoginOK);

            client.RegisterOK += new EventHandler(_RegisterOK);

            client.LoginFailed += new ErrorEventHandler(_LoginFailed);

            client.RegisterFailed += new ErrorEventHandler(_RegisterFailed);

            client.Disconnected += new EventHandler(_Disconnected);


        }


        #region methods

        public Boolean OnLogin()
        {
            if (_username != null && _password != null)
            {
                client.Login(_username, _password);
                return true;
            }
            else
            {
                DisplayInvalidLoginPrompt();
                return false;
            }
        }

        public Boolean OnRegister()
        {
            if (_username != null && _password != null)
                if (_password == _confirmpassword)
                {
                    client.Register(_username, _password);
                    return true;
                }
                else
                {
                    DisplayInvalidConfirmationPrompt();
                    return false;
                }
            else
            {
                DisplayInvalidRegisterPrompt();
                return false;
            }
        }

        public void OnLogout()
        {
            client.Disconnect();
        }

        void _LoginOK(object sender, EventArgs e)
        {
            //ExecuteGoToMessengerPage();

        }

        void _RegisterOK(object sender, EventArgs e)
        {
            //ExecuteGoToMessengerPage();
        }

        void _LoginFailed(object sender, ErrorEventArgs e)
        {
            //if (e.Error == ErrorEvent.WrongPassword) DisplayWrongPassword();
            //else if (e.Error == ErrorEvent.NoExists) DisplayUserNoExist();
            //else DisplayInvalidLoginPrompt();
        }

        void _RegisterFailed(object sender, ErrorEventArgs e)
        {
            //if (e.Error == ErrorEvent.Exists) DisplayUserExist();
            //else DisplayInvalidRegisterPrompt();
        }

        private void _Disconnected(object sender, EventArgs e)
        {
            //ExecuteGoBackToLoginPage();
        }

        private void ExecuteGoToChatPage()
        {
            if (_ItemSelected != null) _navigation.GoToChat(this);
            else DisplayInvalidChatUserPrompt();
        }

        private void ExecuteGoBackToMessengerPage()
        {
            _navigation.GoBackToMessenger();
        }

        private void ExecuteGoToMessengerPage()
        {
            _navigation.GoToMessenger(this);
        }

        private void ExecuteGoBackToLoginPage()
        {
            _navigation.GoBackToLoginPage();
        }

        private void ExecuteRemoveFriend()
        {
            FriendList.Remove(objItemSelected);
            objItemSelected = null;
        }

        private void ExecuteAddFriend()
        {
            if (addFriend != null && addFriend != "") FriendList.Add(addFriend);
            else DisplayNoFriendName();
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
