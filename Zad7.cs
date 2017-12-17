using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Zad2
{
    class Program
    {
        static void Main(string[] args)
        {
            zad2();
            Console.ReadKey();
        }

        static void zad2()
        {
            //ManualResetEvent manualEvent = new ManualResetEvent(false);

            FileStream f =
               new FileStream("TestFile.txt", FileMode.Open);

            byte[] a = new byte[1024];

            IAsyncResult result = f.BeginRead(a, 0, a.Length, null, null);

            //dowolne operacje rownolegle do czytania

            for (int i = 0; i < 100; i++)
                Console.Write(i + " ");


            int size = f.EndRead(result);

            Console.WriteLine(Encoding.ASCII.GetString(a, 0, size));

            //manualEvent.WaitOne(5000, false);
        }
        
    }
}
