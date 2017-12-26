using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

//zad 15A
namespace IOzadania
{
    class Program
    {
        static async Task serverTask()
        {
            Console.WriteLine("Tworzenie serwera");

            TcpListener server = new TcpListener(IPAddress.Any, 2048);
            server.Start();
            while (true)
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
                            client.GetStream().WriteAsync(buffer, 0, i);
                            i = await client.GetStream().ReadAsync(buffer, 0, buffer.Length);
                        }
                    });
            }
        }
        static async Task clientTask()
        {
            Console.WriteLine("Tworzenie klienta" );
            TcpClient client = new TcpClient();
            client.Connect(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 2048));
            while (true)
            {
                string textToSend = DateTime.Now.ToString();
                byte[] message = ASCIIEncoding.ASCII.GetBytes(textToSend);
                await client.GetStream().WriteAsync(message, 0, message.Length).ContinueWith(
                async (t) =>
                {
                    int i = 0;

                    await client.GetStream().WriteAsync(message, 0, message.Length);
                    i = await client.GetStream().ReadAsync(message, 0, message.Length);
                    //Console.WriteLine("Otrzymalem: " + System.Text.Encoding.Default.GetString(message));

                });
            }
        }
    

        static void Main(string[] args)
        {
            Task serwer = serverTask();
            Task client = clientTask();
            Task client2 = clientTask();
            Task.WaitAll(serwer, client, client2);
			
			serwer.StopRunning();

            Console.ReadKey();
        }
        
    }
}
