using System;
using System.Threading;
using System.Threading.Tasks;

namespace MultiThreadingSample
{
    /// <summary>
    /// 线程池
    /// </summary>
    class ThreadPool
    {
        //声明委托
        private delegate string RunOnThreadPool(out int threadId);

        private static void Callback(IAsyncResult ar)
        {
            Console.WriteLine("触发回调");
            Console.WriteLine("异步状态：" + ar.AsyncState);
            Console.WriteLine("是否是线程池的线程：" + Thread.CurrentThread.IsThreadPoolThread);
            Console.WriteLine("ThreadId：" + Thread.CurrentThread.ManagedThreadId);
        }

        private static string Test(out int threadId)
        {
            Console.WriteLine("Test开始");
            Console.WriteLine("是否是线程池的线程：" + Thread.CurrentThread.IsThreadPoolThread);
            Thread.Sleep(TimeSpan.FromSeconds(2));
            threadId = Thread.CurrentThread.ManagedThreadId;
            return "ThreadId：" + threadId;
        }

        static void Main(string[] args)
        {
            int threadId = 0;
            RunOnThreadPool poolDelegate = Test;
            var t = new Thread(() => Test(out threadId));
            t.Start();
            t.Join();

            Console.WriteLine("Thread ID=" + threadId);

            #region NetFramework

            //https://stackoverflow.com/questions/45183294/begininvoke-not-supported-on-net-core-platformnotsupported-exception
            //.net core 平台不支持委托开启线程，会抛异常：Operation is not supported on this platform.

            //IAsyncResult ar = poolDelegate.BeginInvoke(out threadId, Callback, "测试是否可以回调");
            //ar.AsyncWaitHandle.WaitOne();
            //string result = poolDelegate.EndInvoke(out threadId, ar);
            //Console.WriteLine("结果：" + result);

            #endregion

            #region NetCore

            // Task默认使用线程池中的线程
            Task task = Task.Run(() => poolDelegate(out threadId));
            // 回调
            task.ContinueWith((task) => Callback(task)); // Callback方法会在task的异步操作完成后被调用
            task.Wait();

            #endregion

            Console.WriteLine("ID：" + threadId);

            Console.ReadKey();
        }
    }
}
