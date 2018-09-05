using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;

namespace Server_Communicator
{
    class Program
    {
        public IPAddress ip = IPAddress.Parse("127.0.0.1");
        public int port = 1234;
        public bool dziala = true;
        public TcpListener server; //TCP server
        public X509Certificate2 certyfikat = new X509Certificate2("server.pfx", "admin");



        public Program()
        {
            Console.Title = "Bezpieczny Komunikator Server";
            Console.WriteLine("------ Bezpieczny Komunikator Server -----");
            server = new TcpListener(ip, port);
            server.Start();
        }

        void Listen() //Nadchodzące połączenia
        {
            while (dziala)
            {
                TcpClient tcpClient = server.AcceptTcpClient();
                Client client = new Client(this, tcpClient);
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
