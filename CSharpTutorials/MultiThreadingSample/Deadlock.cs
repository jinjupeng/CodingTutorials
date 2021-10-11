using System;
using System.Threading;

namespace MultiThreadingSample
{
    /// <summary>
    /// 死锁
    /// 简单来说，当线程在请求独占资源得不到满足而等待时，又不释放已占有资源，就会出现死锁
    /// </summary>
    public class Deadlock
    {
        static void Main(string[] args)
        {
            var workers = new DeadlockWorkers();
            workers.StartThreads();
            var output = workers.GetResult();
            Console.WriteLine(output);
        }
    }

    /// <summary>
    /// 死锁的发生：
    /// 线程 1 工作时锁定了资源 A，期间需要锁定使用资源 B；
    /// 但此时资源 B 被线程 2 独占，恰巧资线程 2 此时又在待资源 A 被释放；而资源 A 又被线程 1 占用......，
    /// 如此，双方陷入了永远的循环等待中
    /// </summary>
    //class DeadlockWorkers
    //{
    //    Thread thread1, thread2;

    //    object resourceA = new object();
    //    object resourceB = new object();

    //    string output;

    //    public void StartThreads()
    //    {
    //        thread1 = new Thread(Thread1DoWork);
    //        thread2 = new Thread(Thread2DoWork);
    //        thread1.Start();
    //        thread2.Start();
    //    }

    //    public string GetResult()
    //    {
    //        thread1.Join();
    //        thread2.Join();
    //        return output;
    //    }

    //    public void Thread1DoWork()
    //    {
    //        lock (resourceA)
    //        {
    //            Thread.Sleep(100);
    //            lock (resourceB)
    //            {
    //                output += "T1#";
    //            }
    //        }
    //    }

    //    public void Thread2DoWork()
    //    {
    //        lock (resourceB)
    //        {
    //            Thread.Sleep(100);
    //            lock (resourceA)
    //            {
    //                output += "T2#";
    //            }
    //        }
    //    }
    //}

    /// <summary>
    /// 死锁的处理
    /// </summary>
    class DeadlockWorkers
    {
        Thread thread1, thread2;

        object resourceA = new object();
        object resourceB = new object();

        string output;

        public void StartThreads()
        {
            thread1 = new Thread(Thread1DoWork);
            thread2 = new Thread(Thread2DoWork);
            thread1.Start();
            thread2.Start();
        }

        public string GetResult()
        {
            thread1.Join();
            thread2.Join();
            return output;
        }

        public void Thread1DoWork()
        {
            bool mustDoWork = true;
            lock (resourceA)
            {
                Thread.Sleep(100);
                // 判断在0s内是否能获得该对象的访问权
                if(Monitor.TryEnter(resourceB, 0)) // 检查某个对象是否被占用
                {
                    output += "T1#";
                    mustDoWork = false;
                    Monitor.Exit(resourceB);
                }
            }
            if (mustDoWork)
            {
                // 把该线程已占用的其它资源交还给CPU
                Thread.Yield();
            }
        }

        public void Thread2DoWork()
        {
            lock (resourceB)
            {
                Thread.Sleep(100);
                lock (resourceA)
                {
                    output += "T2#";
                }
            }
        }
    }
}
