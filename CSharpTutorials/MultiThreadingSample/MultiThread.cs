using System;
using System.Threading;

namespace MultiThreadingSample
{
    class MultiThread
    {
        static void Main(string[] args)
        {
            // 创建新线程
            Thread oneThread = new Thread(PrintNumber);
            // 开启线程
            oneThread.Start();

            // 睡眠2s
            // Thread.Sleep(TimeSpan.FromSeconds(2));

            // 线程等待，必须等待oneThread线程执行完毕后才能执行下一步操作
            oneThread.Join();

            // 主线程中输出
            PrintNumber();
            Console.ReadKey();
        }

        static void PrintNumber()
        {
            Console.WriteLine("开始......");
            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine(i);
            }
        }
    }
}
