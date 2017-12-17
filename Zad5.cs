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
	    private static Object mojLock = new Object();
        
        private static AutoResetEvent[] mre;
        //wynik
        private static int result;

        static void Blah(Object stateInfo)
        {
            var table = (int[])((object[])stateInfo)[0];
            var index = (int) ((object[])(stateInfo))[1];
            CThread(table, index);
        }

        static void CThread(int[] table, int index)
        {
            int sum = 0;
            foreach (int item in table)
                sum += item;

            lock (thisLock)
            {
                Program.result += sum;
            }

            mre[index].Set();
        }


        static void Main(string[] args)
        {
            Program.result = 0;
            int size = 128;
            int block = 8;

            int[] table = new int[size];
            int[] blockData = new int[block];

            mre = new AutoResetEvent[size / block];

            Random r1 = new Random();

            for (int i = 0; i < size; i++)
            {
                table[i] = r1.Next();
            }

            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            for (int i = 0; i < size / block; i++)
            {
                for (int j = 0; j < block; j++)
                {
                    blockData[j] = table[i * block + j];
                }

                mre[i] = new AutoResetEvent(false);
                ThreadPool.QueueUserWorkItem(Blah, new object[] { blockData, i });

                blockData = new int[block];
            }
            
            WaitHandle.WaitAll(mre);
            Console.WriteLine("Suma: " + Program.result);

            stopWatch.Stop();
            var ts = stopWatch.ElapsedMilliseconds;
            Console.WriteLine("Czas: " + ts + "ms");


            stopWatch.Restart();
            int s = 0;
            foreach (int a in table)
                s += a;
            stopWatch.Stop();
            var ts2 = stopWatch.ElapsedMilliseconds;
            Console.WriteLine("Czas: " + ts2 + "ms");

        }

	}
}
	