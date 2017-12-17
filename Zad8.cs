using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zad3
{
    class Program
    {
        static int silniaRekuren(int i)
        {
            if (i < 1)
                return 1;
            else
                return i * silniaRekuren(i - 1);
        }
        static int silniaIterac(int n)
        {
            int result = 1;
            for (int i = 1; i <= n; i++)
            {
                result *= i;
            }
            return result;
        }
        static int fibRek(int n)
        {
            if ((n == 1) || (n == 2))
                return 1;
            else
                return fibRek(n - 1) + fibRek(n - 2);
        }
        static  int fibIter(int n)
        {
            int a, b;
            if (n == 0) return 0;

            a = 0; b = 1;
            for (int i = 0; i < (n - 1); i++)
            {
                int tempswap = a;
                a = b;
                b = tempswap;
                b += a;
            }
            return b;
        }

        delegate int DelegateType(int arg);
        static DelegateType dn;
        static DelegateType dn1;
        static DelegateType dn2;
        static DelegateType dn3;

        static void Main(string[] args)
        {
            int liczba = 27;

            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            dn = new DelegateType(silniaRekuren);
            IAsyncResult ar = dn.BeginInvoke(liczba, null, null);
            int res = dn.EndInvoke(ar);
            stopWatch.Stop();
            var ts = stopWatch.ElapsedMilliseconds;
            Console.WriteLine("Czas silni rekurencyjnie: " + ts + "ms");
            Console.WriteLine("Wynik: " + res);

            stopWatch.Restart();
            dn1 = new DelegateType(silniaIterac);
            IAsyncResult ar1 = dn1.BeginInvoke(liczba, null, null);
            int res1 = dn1.EndInvoke(ar1);
            stopWatch.Stop();
            var ts2 = stopWatch.ElapsedMilliseconds;
            Console.WriteLine("Czas silni iteracyjnie: " + ts2 + "ms");
            Console.WriteLine("Wynik: " + res1);

            stopWatch.Restart();
            dn2 = new DelegateType(fibRek);
            IAsyncResult ar2 = dn2.BeginInvoke(liczba, null, null);
            int res2 = dn2.EndInvoke(ar2);
            stopWatch.Stop();
            var ts3 = stopWatch.ElapsedMilliseconds;
            Console.WriteLine("Czas ciag fibbonacciego rekurencyjnie: " + ts3 + "ms");
            Console.WriteLine("Wynik: " + res2);

            stopWatch.Restart();
            dn3 = new DelegateType(fibIter);
            IAsyncResult ar3 = dn3.BeginInvoke(liczba, null, null);
            int res3 = dn3.EndInvoke(ar3);
            stopWatch.Stop();
            var ts4 = stopWatch.ElapsedMilliseconds;
            Console.WriteLine("Czas ciag fibbonacciego iteracyjnie: " + ts4 + "ms");
            Console.WriteLine("Wynik: " + res3);


            Console.ReadKey();
        }
    }
}
