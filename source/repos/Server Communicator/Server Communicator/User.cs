using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server_Communicator
{
    [Serializable] class User
    {
        public string Username;
        public string Password;
        [NonSerialized] public bool IsLogged; //Czy jest aktualnie zalogowany
        [NonSerialized] public Client Connection; //Informacje na temat połączenia

        public User(string User, string pass)
        {
            this.Username = User;
            this.Password = pass;
            this.IsLogged = false;
        }
        public User(string User, string pass, Client Conn)
        {
            this.Username = User;
            this.Password = pass;
            this.IsLogged = true;
            this.Connection = Conn;
        }

    }
}
