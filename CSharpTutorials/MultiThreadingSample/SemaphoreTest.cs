using System;
using System.Threading;

namespace MultiThreadingSample
{
    /// <summary>
    /// 信号量学习
    /// </summary>
    public class SemaphoreTest
    {
        static int count { get; set; }
        static SimpleWaitLock sema = new SimpleWaitLock(1);

        static void Main(string[] args)
        {
            for (int i = 1; i <= 2; i++)
            {
                var thread = new Thread(ThreadMethod);
                thread.Start(i);
                Thread.Sleep(500);
            }
        }

        static void ThreadMethod(object threadNo)
        {
            sema.Enter();
            for (int i = 0; i < 4; i++)
            {
                var temp = count;
                Console.WriteLine("线程 " + threadNo + " 读取计数");
                Thread.Sleep(1000); // 模拟耗时工作
                count = temp + 1;
                Console.WriteLine("线程 " + threadNo + " 已将计数增加至: " + count);
                Thread.Sleep(1000);
            }
            sema.Leave();
        }
    }

    /// <summary>
    /// 允许多个线程并发访问一个资源（如果所有线程以只读方式访问资源，就是安全的）
    /// </summary>
    public sealed class SimpleWaitLock : IDisposable
    {
        private Semaphore m_awailable;

        public SimpleWaitLock(Int32 maxConcurrent)
        {
            m_awailable = new Semaphore(maxConcurrent, maxConcurrent);
        }

        public void Enter()
        {
            // 在内核中阻塞直到资源可用
            m_awailable.WaitOne();
        }

        public void Leave()
        {
            // 让其他线程访问资源
            m_awailable.Release(1);
        }

        public void Dispose()
        {
            m_awailable.Close();
        }
    }
}
