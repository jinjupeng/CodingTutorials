using System;
using System.Diagnostics;
using System.Net;

namespace AsyncAwaitSample
{
    class MyDownloadString
    {
        Stopwatch sw = new Stopwatch();

        public void DoRun()
        {
            const int LargeNumber = 6_000_000;
            sw.Start();
            int t1 = Count
        }

        private int CountCharacters(int id, string uriString)
        {
            WebClient wc1 = new WebClient();
            Console.WriteLine($"Starting call {id}：{sw.Elapsed.TotalMilliseconds}ms");
            string 
        }
    }
}
