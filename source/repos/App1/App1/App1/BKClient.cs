using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace App1
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

        public TcpClient client;
        public NetworkStream strumien;
        public SslStream ssl; //szyfrowany kanał SSL
        public BinaryReader br;
        public BinaryWriter bw;


        //Events 

        public event EventHandler LoginOK;
        public event EventHandler RegisterOK;
        public event ErrorEventHandler LoginFailed;
        public event ErrorEventHandler RegisterFailed;
        public event EventHandler Disconnected;
        public event AvailableEventHandler UserAvailable;
        public event ReceivedEventHandler MessageReceived;

        protected void OnLoginOK()
        {
            if (LoginOK != null) LoginOK(this, EventArgs.Empty);
        }

        protected void OnRegisterOK()
        {
            if (RegisterOK != null) RegisterOK(this, EventArgs.Empty);
        }

        protected void OnLoginFailed(ErrorEventArgs e)
        {
            if (LoginFailed != null) LoginFailed(this, e);
        }

        protected void OnRegisterFailed(ErrorEventArgs e)
        {
            if (RegisterFailed != null) RegisterFailed(this, e);
        }

        protected void OnDisconnectedOK()
        {
            if (Disconnected != null) Disconnected(this, EventArgs.Empty);
        }

        protected void OnUserAvailable(AvailableEventArgs e)
        {
            if (UserAvailable != null) UserAvailable(this, e);
        }

        protected void OnMessageReceived(ReceivedEventArgs e)
        {
            if (MessageReceived != null) MessageReceived(this, e);
        }

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
            client = new TcpClient(Server, Port);
            strumien = client.GetStream();
            ssl = new SslStream(strumien, false, new RemoteCertificateValidationCallback(ValidateCert));
            ssl.AuthenticateAsClient("");
            br = new BinaryReader(ssl, Encoding.UTF8);
            bw = new BinaryWriter(ssl, Encoding.UTF8);
            int hello = br.ReadInt32();
            if (hello != Packets.Hello)
            {
                //Zamkniecie polaczenia, wyjatek (niespodziewany pakiet)
            }
            bw.Write(Packets.Hello);
            bw.Flush();
            bw.Write(_registering ? Packets.Register : Packets.Login);
            bw.Write(Username);
            bw.Write(Password);
            bw.Flush();
            byte packet = br.ReadByte();
            if(packet == Packets.Done)
            {
                if (_registering) OnRegisterOK();
                OnLoginOK();
                Receiver();
            }
            else
            {
                ErrorEventArgs err = new ErrorEventArgs((ErrorEvent)packet);
                if (_registering) OnRegisterFailed(err);
                else OnLoginFailed(err);
            }
            if(_connected) CloseConnection();
        }

        void CloseConnection()
        {
            br.Close();
            bw.Close();
            ssl.Close();
            strumien.Close();
            client.Close();
            OnDisconnectedOK();
            _connected = false;
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

        public static bool ValidateCert(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return true; //Dla niezaufanych certifikatow
        }

        public void IsAvailable(string user)
        {
            if (_connected)
            {
                bw.Write(Packets.IsAvailable);
                bw.Write(user);
                bw.Flush();
            }
        }

        public void SendMessage(string adresat, string msg)
        {
            if (_connected)
            {
                bw.Write(Packets.SendMessage);
                bw.Write(adresat);
                bw.Write(msg);
                bw.Flush();
            }
        }

        void Receiver()
        {
            _logged = true;

            try
            {
                while (client.Connected)
                {
                    byte type = br.ReadByte();

                    if (type == Packets.IsAvailable)
                    {
                        string user = br.ReadString();
                        bool IsAvailable = br.ReadBoolean();

                        OnUserAvailable(new AvailableEventArgs(user, IsAvailable));
                    }
                    if (type == Packets.Received)
                    {
                        string nadawca = br.ReadString();
                        string msg = br.ReadString();
                        OnMessageReceived(new ReceivedEventArgs(nadawca, msg));
                    }
                }
            }
            catch (IOException)
            {

            }

            _logged = false;
        }


    }
}
