using System;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace DownloadWebPagesStartingPoint
{
    class Program
    {
        private static readonly string[] Addresses = 
        { "http://laerer.rhs.dk/andersb/", "http://easj.dk/", 
            "http://msdn.com/", "http://google.com/" };

        private static int GetNumberOfBytes(String url)
        {
            byte[] data = new WebClient().DownloadData(url);
            return data.Length;
        }

        static void Main(string[] args)
        {
            Sequential();
            TaskMethod();
            PLinqMethod();
        }

        private static void Sequential()
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            foreach (string address in Addresses)
            {
                int bytes = GetNumberOfBytes(address);
                //Console.WriteLine("{0} {1} bytes", address, bytes);
            }
            stopwatch.Stop();
            Console.WriteLine("Sequential: {0}", stopwatch.Elapsed);
        }

        private static void TaskMethod()
        {
            Task t = Task.Run(() =>
            {
                Stopwatch stopwatch = Stopwatch.StartNew();
                foreach (string address in Addresses)
                {
                    int bytes = GetNumberOfBytes(address);
                    //Console.WriteLine("{0} {1} bytes", address, bytes);
                }
                stopwatch.Stop();
                Console.WriteLine("Task: {0}", stopwatch.Elapsed);
            });
            t.Wait();
        }

        private static void PLinqMethod()
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            foreach (string address in Addresses)
            {
                int bytes = GetNumberOfBytes(address);
                //Console.WriteLine("{0} {1} bytes", address, bytes);
            }
            var plinqmethod = from i in Addresses.AsParallel()
                select i;
            
            stopwatch.Stop();
            Console.WriteLine("PLinq: {0}", stopwatch.Elapsed);
        }
    }
}
