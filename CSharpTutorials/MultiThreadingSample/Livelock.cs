using System;
using System.Threading;

namespace MultiThreadingSample
{
    /// <summary>
    /// 活锁
    /// 我们把这种线程一直处于运行状态但其任务却一直无法进展的现象称为活锁。
    /// 活锁和死锁的区别在于，处于活锁的线程是运行状态，而处于死锁的线程表现为等待；活锁有可能自行解开，死锁则不能。
    /// </summary>
    public class LiveLock
    {
        static void Main(string[] args)
        {
            var workers = new LiveLockWorkers();
            workers.StartThreads();
            var output = workers.GetResult();
            Console.WriteLine(output);
        }
    }
    /// <summary>
    /// 活锁的出现：
    /// 线程 1 和线程 2 一直在相互让步，然后不断重新开始。
    /// 两个线程都无法进入 Monitor.TryEnter 代码块，虽然都在运行，但却没有真正地干活。
    /// 解决方式：
    /// 1、要避免活锁，就要合理预估各线程对独占资源的占用时间，并合理安排任务调用时间间隔，要格外小心。
    /// 2、推荐的做法是使用信号量机制代替锁。
    /// </summary>
    //class LiveLockWorkers
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
    //        bool mustDoWork = true;
    //        Thread.Sleep(100);
    //        while (mustDoWork)
    //        {
    //            lock (resourceA)
    //            {
    //                Console.WriteLine("T1 重做");
    //                Thread.Sleep(1000);
    //                // 判断在0s内是否能获得该对象的访问权
    //                if (Monitor.TryEnter(resourceB, 0)) // 检查某个对象是否被占用
    //                {
    //                    output += "T1#";
    //                    mustDoWork = false;
    //                    Monitor.Exit(resourceB);
    //                }
    //            }
    //            if (mustDoWork)
    //            {
    //                // 把该线程已占用的其它资源交还给CPU
    //                Thread.Yield();
    //            }
    //        }
    //    }

    //    public void Thread2DoWork()
    //    {
    //        bool mustDoWork = true;
    //        Thread.Sleep(100); 
    //        while (mustDoWork)
    //        {
    //            lock (resourceB)
    //            {
    //                Console.WriteLine("T2 重做");
    //                Thread.Sleep(1100);
    //                if (Monitor.TryEnter(resourceA, 0))
    //                {
    //                    output += "T2#";
    //                    mustDoWork = false;
    //                    Monitor.Exit(resourceB);
    //                }
    //            }
    //            if (mustDoWork) Thread.Yield();
    //        }
    //    }
    //}

    /// <summary>
    /// 信号量避免活锁
    /// </summary>
    class LiveLockWorkers
    {
        Thread thread1, thread2;
        SimpleWaitLock sema = new SimpleWaitLock(1);
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
            sema.Enter();
            for (int i = 0; i < 4; i++)
            {
                Thread.Sleep(100);
                output += "T1#";
            }

            sema.Leave();
        }

        public void Thread2DoWork()
        {
            sema.Enter();
            for (int i = 0; i < 4; i++)
            {
                Thread.Sleep(100); 
                output += "T2#";
            }

            sema.Leave();
        }
    }

}
