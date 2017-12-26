using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Zad15d
{

    class Serwer
    {
        public TcpListener server;
        public Serwer()
        {
            TcpListener server = new TcpListener(IPAddress.Any, 2048);
        }
        public async Task RunAsync(CancellationToken token)
        {
            server.Start();
            Logs logs = new Logs();
            while (true && !token.IsCancellationRequested)
            {
                TcpClient client = await server.AcceptTcpClientAsync();
                Console.WriteLine("W  petli serwera");
                byte[] buffer = new byte[1024];
                await client.GetStream().ReadAsync(buffer, 0, buffer.Length).ContinueWith(
                    async (t) =>
                    {
                        Console.WriteLine("W bloku asynchronicznym serwera ");
                        int i = t.Result;
                        while (true)
                        {
                            await client.GetStream().WriteAsync(buffer, 0, i);
                            try
                            {
                                i = await client.GetStream().ReadAsync(buffer, 0, buffer.Length);
                                await logs.writeToFile(System.Text.Encoding.Default.GetString(buffer));
                            }
                            catch (Exception e)
                            {
                                break;
                            }
                        }
                    });
            }
        }
    }
    class Klient
    {

        public TcpClient client;
        public void createClient()
        {
            Console.WriteLine("Tworzenie klienta");
            client = new TcpClient();
        }
        public void con()
        {
            client.Connect(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 2048));
        }
        public async Task<string> clientPing(string textToSend)
        {
            Console.WriteLine("Ping");
            byte[] message = ASCIIEncoding.ASCII.GetBytes(textToSend);
            client.GetStream().WriteAsync(message, 0, message.Length);
            var a = await client.GetStream().ReadAsync(message, 0, message.Length);
            return Encoding.UTF8.GetString(message, 0, a);
        }
        public async Task<IEnumerable<string>> keepPinging(string message, CancellationToken cts)
        {
            Console.WriteLine("Keep pinging");
            List<string> mess = new List<string>();
            bool a = false;
            while (!a)
            {
                if (cts.IsCancellationRequested)
                    a = true;
                mess.Add(await clientPing(message));
            }
            return mess;
        }
    }

    class Logs {
        StreamWriter fileToWrite;

        public Logs()
        {
            fileToWrite = new StreamWriter("logi.txt");
        }

        public async Task writeToFile(string message)
        {
            await fileToWrite.WriteLineAsync(DateTime.Now.ToString() + message);
        }
    }

    class Program
    {
        static async Task MainAsync(string[] args)
        {
            Logs logs = new Logs();
            Serwer s = new Serwer();
            CancellationToken a = new CancellationToken();
            s.RunAsync(a);

            Klient k = new Klient();
            CancellationToken b = new CancellationToken();
            k.keepPinging("hello its mii", b);


            s.server.Stop();
        }
    }
}
