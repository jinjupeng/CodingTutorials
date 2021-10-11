using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TaskSample
{
    /*
     * 异步方法的一般执行流程
     * await 之前的代码同步运行在调用一部方法的线程里
     * 当运行到await所在行后，调用线程会在这里立即返回，await等待的异步任务会被调度程序重新排队到线程池申请可用的工作线程异步执行
     * 当await异步等待的任务完成后，await之后的代码重新排队从线程池中申请可用的线程继续执行
     * 
     */
    class TaskTest
    {
        static void Main(string[] args)
        {
            Console.WriteLine($"Main Begin ,CurrentThreadId：{Thread.CurrentThread.ManagedThreadId}，是否线程池线程：{Thread.CurrentThread.IsThreadPoolThread}\r\n");

            Task<string> task1 = TaskMethod0101("task1", 3);
            //string result= task1.Result; // 获取Task<T>的Result值会阻塞当前线程，直到异步任务正常完成返回结果，或者传播异常信息返回

            Console.WriteLine($"Main Ok ,CurrentThreadId：{Thread.CurrentThread.ManagedThreadId}，是否线程池线程：{Thread.CurrentThread.IsThreadPoolThread}\r\n");
            Console.WriteLine(task1.Result);
        }

        public static async Task<string> TaskMethod0101(string taskName, double seconds)
        {
            Console.WriteLine($"await 之前的代码运行在调用异步方法的线程里，CurrentThreadId：{Thread.CurrentThread.ManagedThreadId},是否线程池线程：{Thread.CurrentThread.IsThreadPoolThread} \r\n");

            // 当运行到await所在行后，调用线程在这里会立即返回
            // await Task.Delay(TimeSpan.FromSeconds(seconds)); // 如果这里没有await，则这里不会异步等待而是继续同步运行await后面的代码

            /*
             * 总结一下 await func()后续代码会从线程池中申请新的线程去执行
             * 而func().GetAwaiter.GetResult()则会返回最初的线程去执行
             */
            // AnotherTask().GetAwaiter().GetResult();
            await AnotherTask();

            Console.WriteLine($"await 之后的代码重新排队从线程池中申请可用工作线程继续执行，CurrentThreadId：{Thread.CurrentThread.ManagedThreadId},是否线程池线程：{Thread.CurrentThread.IsThreadPoolThread} \r\n");

            return $"{taskName} Complete";
        }

        public static async Task<string> AnotherTask()
        {
            await Task.Delay(TimeSpan.FromSeconds(3));
            Console.WriteLine($"AnotherTask CurrentThreadId：{Thread.CurrentThread.ManagedThreadId}，是否线程池线程：{Thread.CurrentThread.IsThreadPoolThread}\r\n");
            return await Task.FromResult("AnotherTask Ok");
        }
    }
}
