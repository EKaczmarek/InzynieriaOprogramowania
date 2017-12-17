using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Zad1
{
    class Program
    {
        static void Main(string[] args)
        {
            zad1();

            Console.ReadKey();

        }
        //zad 1
        static void zad1()
        {
            ManualResetEvent manualEvent = new ManualResetEvent(false);

            FileStream fStream =
               new FileStream("TestFile.txt", FileMode.OpenOrCreate,
               FileAccess.ReadWrite, FileShare.None, 1024, true);

            byte[] buffer = new byte[1024];

            fStream.BeginRead(
              buffer, 0, buffer.Length,
              EndReadCallback, new object[] { fStream, buffer });

            manualEvent.WaitOne(5000, false);
        }
        static void EndReadCallback(IAsyncResult asyncResult)
        {
            // mozna tez object [] ob = asyncResult.AsyncState as object [];
            object[] ob = (object[])asyncResult.AsyncState;

            byte[] b = new byte[1024];
            b = (byte [])ob[1];

            using (FileStream f = (FileStream)ob[0])
            {
                f.Read(b, 0, b.Length);
                f.Close();
            }           

            Console.WriteLine(Encoding.ASCII.GetString(b));
        }
    }
}
