﻿using System;
using PSC.Xamarin.MvvmHelpers;
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

        public Command SendCommand { get; set; }

        public ICommand LoginAndGoCommand
        {
            get
            {
                return new Command(() =>
                {
                    OnLogin();
                });
            }
        }

        public ICommand RegisterAndGoCommand
        {
            get
            {
                return new Command(() =>
                {
                    OnRegister();
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

        private ObservableRangeCollection<Message> _Messages;
        public ObservableRangeCollection<Message> Messages
        {
            get
            {
                if (_Messages != null) return _Messages;
                else return _Messages = new ObservableRangeCollection<Message>();
            }
            set
            {
                if (_Messages != value)
                {
                    _Messages = value;
                    OnPropertyChanged();
                }
            }
        }

        string outgoingText = string.Empty;
        public string OutgoingText
        {
            get { return outgoingText; }
            set
            {
                if (outgoingText != value)
                {
                    outgoingText = value;
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

            SendCommand = new Command(ExecuteSendCommand);

            client.LoginOK += new EventHandler(_LoginOK);

            client.RegisterOK += new EventHandler(_RegisterOK);

            client.LoginFailed += new ErrorEventHandler(_LoginFailed);

            client.RegisterFailed += new ErrorEventHandler(_RegisterFailed);

            client.Disconnected += new EventHandler(_Disconnected);

           // client.UserAvailable = new AvailableEventHandler(_UserAvailable);

           // client.MessageReceived = new ReceivedEventHandler(_MessegeReceived);


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
            Device.BeginInvokeOnMainThread(() =>
            {
                ExecuteGoToMessengerPage();
            });
        }

        void _RegisterOK(object sender, EventArgs e)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                ExecuteGoToMessengerPage();
            });
        }

        void _LoginFailed(object sender, ErrorEventArgs e)
        {
            if (e.Error == ErrorEvent.WrongPassword)
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    DisplayWrongPassword();
                });  
            }
            else if (e.Error == ErrorEvent.NoExists)
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    DisplayUserNoExist();
                });
            }
            else Device.BeginInvokeOnMainThread(() =>
            {
                DisplayInvalidLoginPrompt();
            });
        }

        void _RegisterFailed(object sender, ErrorEventArgs e)
        {
            if (e.Error == ErrorEvent.Exists) Device.BeginInvokeOnMainThread(() =>
            {
                DisplayUserExist();
            });
            else Device.BeginInvokeOnMainThread(() =>
            {
                DisplayInvalidRegisterPrompt();
            });
        }

        private void _MessegeReceived(object sender, ReceivedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void _UserAvailable(object sender, AvailableEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void _Disconnected(object sender, EventArgs e)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                ExecuteGoBackToLoginPage();
            });
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

        private void ExecuteSendCommand()
        {
            var message = new Message
            {
                Text = OutgoingText,
                IsIncoming = false,
                MessageDateTime = DateTime.Now
            };

            Messages.Add(message);

            //wyslij wiadomosc

            OutgoingText = string.Empty;
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
