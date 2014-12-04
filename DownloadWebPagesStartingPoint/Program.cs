using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Threading;
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
            ThreadMethod();
            ParallelMethod();
            
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
            List<Task> taskList = new List<Task>();
            
            
                Stopwatch stopwatch = Stopwatch.StartNew();
                foreach (string address in Addresses)
                {
                    int bytes = 0;
                    Task task = new Task(()=> bytes = GetNumberOfBytes(address));
                    //Console.WriteLine("{0} {1} bytes", address, bytes);
                    taskList.Add(task);
                    task.Start();
                }
            
            Task.WaitAll(taskList.ToArray());
            stopwatch.Stop();
            Console.WriteLine("Task: {0}", stopwatch.Elapsed);
            
            
        }

        private static void ThreadMethod()
        {
            List<Thread> ThreadList = new List<Thread>();

            Stopwatch stopwatch = Stopwatch.StartNew();
            foreach (var address in Addresses)
            {
                int bytes = 0;
                Thread runthread = new Thread(()=> bytes = GetNumberOfBytes(address));
                ThreadList.Add(runthread);
                runthread.Start();
            }

            foreach (var thread in ThreadList)
            {
                thread.Join();
            }
            stopwatch.Stop();
            Console.WriteLine("Thread: {0}", stopwatch.Elapsed);
        }

        

        private static void PLinqMethod()
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            
            var plinqmethod = from i in Addresses.AsParallel()
                select GetNumberOfBytes(i);

            foreach (var i in plinqmethod)
            {

            }
            
            stopwatch.Stop();
            Console.WriteLine("PLinq: {0}", stopwatch.Elapsed);
        }

        private static void ParallelMethod()
        {
            Stopwatch stopwatch = Stopwatch.StartNew();

            int bytes = 0;
            Parallel.ForEach(Addresses, address => GetNumberOfBytes(address));

            stopwatch.Stop();
            Console.WriteLine("Parallel method: {0}", stopwatch.Elapsed);
        }
    }
}
