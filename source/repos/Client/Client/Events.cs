using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public enum ErrorEvent : byte
     {
            UsernameError = Packets.UsernameError,
            PasswordError = Packets.PasswordError,
            Exists = Packets.Exists,
            NoExists = Packets.NoExists,
            WrongPassword = Packets.WrongPassword
    }




        public class ErrorEventArgs : EventArgs
        {
            ErrorEvent err;

            public ErrorEventArgs(ErrorEvent error)
            {
                this.err = error;
            }

            public ErrorEvent Error
            {
                get { return err; }
            }


        }

        public class AvailableEventArgs : EventArgs
        {
            string username;
            bool available;

            public AvailableEventArgs(string user, bool avail)
            {
                this.username = user;
                this.available = avail;
            }

            public string Username
            {
                get { return username; }

            }

            public bool IsAvailable
            {
                get { return available; }
            }


        }

        public class ReceivedEventArgs : EventArgs
        {
            string username;
            string msg;

            public ReceivedEventArgs(string user, string msg)
            {
                this.username = user;
                this.msg = msg;
            }

            public string Username
            {
                get { return username; }
            }

            public string Message
            {
                get { return msg; }
            }
        }
        public delegate void ErrorEventHandler(object sender, ErrorEventArgs e);
        public delegate void AvailableEventHandler(object sender, AvailableEventArgs e);
        public delegate void ReceivedEventHandler(object sender, ReceivedEventArgs e);

    }
