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
            //program będzie się wykonywal ponad 500 ms
            //Console.ReadKey();
            //QueueUserWorkItem - Metody wykonywania kolejek i określa obiekt 
            // zawierający dane do użycia przez metodę.
            // Metoda jest wykonywana po udostępnieniu wątku z puli wątków.
            ThreadPool.QueueUserWorkItem(CountThread, 200);
            ThreadPool.QueueUserWorkItem(CountThread, 400);
            Thread.Sleep(500);
        }
        static void CountThread(Object stateInfo)
        {
            Thread.Sleep((int)stateInfo);
            Console.WriteLine("Wątek poczekał {0}", stateInfo);

        }

	}
}