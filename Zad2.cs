using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IO1
{
    class Program
    {

        static void Main(string[] args)
        {
            //dodanie serwera do puli wątków
            ThreadPool.QueueUserWorkItem(ThreadServer);
            //dodanie klienta do puli wątków
            ThreadPool.QueueUserWorkItem(ThreadClient);
            ThreadPool.QueueUserWorkItem(ThreadClient);
        }

        static void ThreadClient(Object stateInfo)
        {
            TcpClient client = new TcpClient();
            client.Connect(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 2048));
            string textToSend = DateTime.Now.ToString();
            byte[] message = ASCIIEncoding.ASCII.GetBytes(textToSend);
            client.GetStream().Write(message, 0, message.Length);
        }

        static void ThreadServer(Object stateInfo)
        {
            TcpListener server = new TcpListener(IPAddress.Any, 2048);
            server.Start();

            while (true)
            {
                TcpClient client = server.AcceptTcpClient();
                byte[] buffer = new byte[1024];
                client.GetStream().Read(buffer, 0, 1024);
                client.GetStream().Write(buffer, 0, buffer.Length);
                Console.Write(Encoding.UTF8.GetString(buffer));
                client.Close();
            }
        }
	}
}