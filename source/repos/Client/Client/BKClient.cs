using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Client
    //Klasa obsługująca i wysyłająca wszystkie pakiety protokołu
{
    class BKClient
    {
        Thread tcpThread;
        bool _connected = false;
        bool _logged = false;
        string _username;
        string _password;
        bool _registering = false;
        
        public string Server
        {
            get { return "localhost"; }
        }
        public int Port
        {
            get { return 1234; }
        }
        public bool IsLogged
        {
            get { return _logged; }
        }
        public string Username
        {
            get { return _username; }
        }
        public string Password
        {
            get { return _password; }
        }

        void SetupConnection()
        {

        }

        void CloseConnection()
        {

        }

        void Connect(string username, string password, bool register){
            if (!_connected)
            {
                _connected = true;
                _username = username;
                _password = password;
                _registering = register;

                tcpThread = new Thread(new ThreadStart(SetupConnection));
                tcpThread.Start();
            }
        }

        public void Login(string username, string password)
        {
            Connect(username, password, false);
        }
        
        public void Register(string username, string password)
        {
            Connect(username, password, true);
        }

        public void Disconnect()
        {
            if (_connected)
            {
                CloseConnection();
            }
        }
    }
}
