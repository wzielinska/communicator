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
        }

        void CloseConnection()
        {
            br.Close();
            bw.Close();
            ssl.Close();
            strumien.Close();
            client.Close();
        }

        public static bool ValidateCert(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return true; //Dla niezaufanych certifikatow
        }
    }
}
