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

namespace Server_Communicator
{
    class Client
    {
        Program program;
        public TcpClient client;
        public NetworkStream strumien;
        public SslStream ssl; //szyfrowany kanał SSL
        public BinaryReader br;
        public BinaryWriter bw;

        User user;


        public Client(Program p, TcpClient c)
        {
            program = p;
            client = c;
            new Thread(new ThreadStart(SetupConnection)).Start();
        }

        void SetupConnection()
        {
            strumien = client.GetStream();
            ssl = new SslStream(strumien, false);
            ssl.AuthenticateAsServer(program.certyfikat, false, System.Security.Authentication.SslProtocols.Tls12, true);
            br = new BinaryReader(ssl, Encoding.UTF8);
            bw = new BinaryWriter(ssl, Encoding.UTF8);
            bw.Write(Packets.Hello);
            bw.Flush();
            int hello = br.ReadInt32();
            if (hello != Packets.Hello)
            {
                //gdy hello bedzie nie takie jak oczekiwane nalezy uzyc wyjatku i zamknac polaczenie.
            }
            byte logMode = br.ReadByte();
            string username = br.ReadString();
            string password = br.ReadString();

            if (username.Length < 15)
            {
                if (password.Length == 64) //docelowo bedzie wiecej warunkow bezpicznego hasla
                {

                }
                else bw.Write(Packets.PasswordError);
            }
            else bw.Write(Packets.UsernameError);

            if (logMode == Packets.Register)
            {
                if (!program.users.ContainsKey(username))
                {
                    user = new User(username, password, this);
                    program.users.Add(username, user);
                    Console.WriteLine("[{0}][{1}] został zarejestrowany!", DateTime.Now, username);
                    bw.Write(Packets.Done);
                    bw.Flush();
                    program.SaveUsers();
                    Receiver();
                }
                else bw.Write(Packets.Exists); //juz istnieje taki uzytkownik
            }
            else if (logMode == Packets.Login)
            {
                if (program.users.TryGetValue(username, out user))
                {
                    if (password == user.Password)
                    {
                        //Haslo prawidlowe
                        //Wylogowywanie innych osób zalogowanych na tym koncie
                        if (user.IsLogged) user.Connection.CloseConnection();
                        user.Connection = this;
                        bw.Write(Packets.Done);
                        bw.Flush();
                        Receiver();
                    }
                    else bw.Write(Packets.WrongPassword); // Złe hasło
                }
                else bw.Write(Packets.NoExists); //Nie istnieje taki uzytkownik
            }

            CloseConnection();
        }

        void CloseConnection()
        {
            if (user != null) user.IsLogged = false;
            br.Close();
            bw.Close();
            ssl.Close();
            strumien.Close();
            client.Close();
        }

        void Receiver() //Otrzymywanie pakietów od klienta
        {
            Console.WriteLine("[{0}][{1}] został zalogowany!", DateTime.Now, user.Username);
            user.IsLogged = true;
            try
            {
                while (client.Client.Connected)
                {
                    byte type = br.ReadByte(); //pobieranie typu wiadomosci (rodzaj otrzymany czy wyslany) 
                    if (type == Packets.IsAvailable)
                    {
                        string kto = br.ReadString();

                        bw.Write(Packets.IsAvailable);
                        bw.Write(kto);


                        if (program.users.TryGetValue(kto, out User info))
                        {
                            if (info.IsLogged) bw.Write(true); //Dostępny
                            else bw.Write(false); //Niedostępny
                        }
                        else bw.Write(false); //Nie istnieje
                        bw.Flush();
                    }
                    else if (type == Packets.SendMessage)
                    {
                        string kto = br.ReadString();
                        string msg = br.ReadString();

                        if (program.users.TryGetValue(kto, out User adresat))
                        {
                            if (adresat.IsLogged)
                            {

                                adresat.Connection.bw.Write(Packets.Received);

                                adresat.Connection.bw.Write(user.Username);

                                adresat.Connection.bw.Write(msg);

                                adresat.Connection.bw.Flush();

                                Console.WriteLine("[{0}]({1}->{2}) Wysłano wiadomość!", DateTime.Now, user.Username, adresat.Username);
                            }
                        }

                    }

                }
            }
            catch (IOException)
            {

            }
            user.IsLogged = false;

            Console.WriteLine("[{0}] ({1}) Użytkownik wylogowany!", DateTime.Now, user.Username);
        }
    }
}
