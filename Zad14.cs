using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Zad12
{
    class Program
    {
        public static async Task zadanie(string page)
        {
            using (WebClient client = new WebClient())
            {
                client.DownloadStringCompleted += (sender, e) =>
                {
                    string pageSourceCode = e.Result;
                    Console.Write(pageSourceCode);
                };

                client.DownloadStringAsync(new Uri(page));                
            }

            Console.ReadLine();

        }

        static void Main(string[] args)
        {
            Program test = new Program();

            var task = zadanie("http://www.feedforall.com/sample.xml");

            Console.ReadKey();

        }

    }
}
