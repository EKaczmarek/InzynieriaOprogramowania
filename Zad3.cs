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
		
		  static void writeConsoleMessage(string message, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(message);
            Console.ResetColor();
        }

        static void ThreadServer2(Object stateInfo)
        {
            TcpListener server = new TcpListener(IPAddress.Parse("127.0.0.1"), 2048);
            server.Start();

            while (true)
            {
                TcpClient client = server.AcceptTcpClient();
                ThreadPool.QueueUserWorkItem(ThreadSecServer, client);

            }
        }


        static void ThreadSecServer(Object c)
        {
            TcpClient client = (TcpClient)c;
            while (true)
            {
                if (!client.Connected)
                    break;
                byte[] buffer = new byte[32];
                client.GetStream().Read(buffer, 0, 32);
                if (buffer[0] != 13)
                {
                    client.GetStream().Write(buffer, 0, buffer.Length);
                    writeConsoleMessage("Otrzymalem od klienta " + ":  " + System.Text.Encoding.Default.GetString(buffer), ConsoleColor.Red);

                }
            }
            client.Close();

        }

        static void Main(string[] args)
        {
            //dodanie serwera do puli wątków
            ThreadPool.QueueUserWorkItem(ThreadServer2);

            //ThreadPool.QueueUserWorkItem(ThreadClient2);

        }

       
	}
}