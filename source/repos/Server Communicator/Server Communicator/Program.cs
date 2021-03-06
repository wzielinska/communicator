﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;


/*TODO:
    Sprawdzanie dostepnosci innych klientow.
    Wysylanie wiadomosci do wybranych klientow jezeli sa dostepni.
    Szyfrowanie plikow serwera odnosnie kont (aes??)
    Przechowywanie hasel w formie haszow (SHA-3??)
    Książka kontaktów
    Opcjonalnie: Xamarin, wysyłanie plików, streaming video (szyfrowanie strumieniowe??)
   Pomysły:
    Zamiast nazwy użytkownika stworzyć identyfikatory przydzielane odgórnie (nr telefonu coś w tym stylu)
    Być może jakaś opcja konferencji (Stworzenie jakby grup użytkowników, do których są adresowane wiadomości czyli jeden do wielu)
*/
namespace Server_Communicator
{
    class Program
    {
        public IPAddress ip = IPAddress.Parse("127.0.0.1");
        public int port = 1234;
        public bool dziala = true;
        public TcpListener server; //TCP server
        public X509Certificate2 certyfikat = new X509Certificate2(Environment.CurrentDirectory + "\\server.pfx", "admin");

        string usersFileName = Environment.CurrentDirectory + "\\users.dat";

        public Dictionary<string, User>
        users = new Dictionary<string, User>(); //Informacje na temat użytkowników oraz ich połączenia


        public Program()
        {
            Console.Title = "Bezpieczny Komunikator Server";
            Console.WriteLine("------ Bezpieczny Komunikator Server -----");
            server = new TcpListener(ip, port);
            LoadUsers();
            Console.WriteLine("[{0}] Rozpoczynanie pracy serwera...", DateTime.Now);
            server.Start();
            Console.WriteLine("[{0}] Serwer został uruchomiony prawidłowo!", DateTime.Now);

            Listen();
        }

        void Listen() //Nadchodzące połączenia
        {
            while (dziala)
            {
                TcpClient tcpClient = server.AcceptTcpClient();
                Client client = new Client(this, tcpClient);
            }
        }

        public void SaveUsers()
        {
            try
            {
                Console.WriteLine("[{0}] Zapisywanie bazy danych użytkowników!", DateTime.Now);
                BinaryFormatter bf = new BinaryFormatter();
                FileStream File = new FileStream(usersFileName, FileMode.Create, FileAccess.Write);
                bf.Serialize(File, users.Values.ToArray());
                File.Close();
                Console.WriteLine("[{0}]Zapisano bazę danych użytkowników!", DateTime.Now);
                //TODO szyfrowanie serwera pliku
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public void LoadUsers()
        {
            try
            {
                Console.WriteLine("[{0}] Wczytywanie bazy danych użytkowników!", DateTime.Now);
                BinaryFormatter bf = new BinaryFormatter();
                FileStream File = new FileStream(usersFileName, FileMode.Open, FileAccess.Read);
                User[] userinfo = (User[])bf.Deserialize(File);
                users = userinfo.ToDictionary((u) => u.Username, (u) => u); // konwersja tablicy do Dictionary
                Console.WriteLine("[{0}] Wczytano bazę danych użytkowników!", DateTime.Now);
                File.Close();
                //TODO odszyfrowanie pliku serweera
            }
            catch
            {

            }
        }

        static void Main(string[] args)
        {
            Program p = new Program();
            Console.WriteLine();
            Console.WriteLine("Wciśnij Enter, aby zakończyć działanie programu.");
            Console.ReadLine();
        }
    }
}
