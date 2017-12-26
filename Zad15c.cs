using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

/// <summary>
/// Dodaj opcję anulowania zadania KeepPinging oraz RunAsync. Wykorzystaj CancellationToken (w pierwszym wypadku musisz samodzielnie 
/// zaimplementować obsługę tokenu, w drugim przypadku konsumujesz implementację ReadAsync i tam wykorzystujesz token)
/// </summary>
//zad 15B

namespace Zad15c
{
    class Program
    {
        class Serwer
        {
            public  TcpListener server;
            public Serwer()
            {
                TcpListener server = new TcpListener(IPAddress.Any, 2048);
            }
            public async Task RunAsync(CancellationToken token)
            {
                server.Start();
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
                                }
                                catch(Exception e)
                                {
                                    break;
                                }
                            }
                        });
                }
            }
        }
        class Klient {

            public  TcpClient client;
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
            public async Task <IEnumerable<string>> keepPinging(string message, CancellationToken cts)
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


        static void Main(string[] args)
        {
            Serwer serwer = new Serwer();
            CancellationTokenSource cts = new CancellationTokenSource();
            Task s = serwer.RunAsync(cts.Token);

            //anulowanie zadania KeepPinging
            Klient tcpClient1 = new Klient();
            Klient tcpClient2 = new Klient();

            tcpClient1.createClient();
            tcpClient1.con();
            tcpClient2.createClient();
            tcpClient2.con();


            tcpClient1.clientPing(DateTime.Now.ToString());
            tcpClient2.clientPing(DateTime.Now.ToString());

            CancellationToken c1 = new CancellationToken();
            CancellationToken c2 = new CancellationToken();

            var cli1 = tcpClient1.keepPinging(DateTime.Now.ToString(), c1);
            var cli2 = tcpClient1.keepPinging(DateTime.Now.ToString(), c2);


            //anulowanie zadania RunAsync przez przekazanie tokena
            cts.Cancel();
            serwer.server.Stop();

            Console.ReadKey();
        }

    }
}
