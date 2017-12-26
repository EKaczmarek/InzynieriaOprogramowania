using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zad12
{
    class Program
    {
        public class TResultDataStructure
        {
            public int a;
            public int b;
            public TResultDataStructure(int a, int b)
            {
                this.a = a;
                this.b = b;
            }
        }

        public static Task<TResultDataStructure> OperationTask()
        {
            return Task.Run(() =>
                              new TResultDataStructure(2, 2)
            );
        }

        static void Main(string[] args)
        {
            var task =  OperationTask();
            Console.WriteLine("main");
        }

    }
}
