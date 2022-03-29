using System;
using System.Threading;

namespace MultiThreadingSample
{
    /// <summary>
    /// ManualResetEvent一次可通知很多个等待的线程，但要关闭需要调用Reset方法手动关闭。
    /// 举例：像一个门，需要手动关闭
    /// </summary>
    class ManualResetEventTest
    {
        //若要将初始状态设置为终止，则为 true；若要将初始状态设置为非终止，则为 false。
        static ManualResetEvent manualResetEvent = new ManualResetEvent(false);

        static void Main(string[] args)
        {
            Thread t1 = new Thread(() =>
            {
                Console.WriteLine("客户1在排队等待付钱...");

                //客户1调用manualResetEvent上的WaitOne来等待付钱通知
                manualResetEvent.WaitOne();
                Console.WriteLine("已经有人请客，客户1不用付钱");
            });
            t1.IsBackground = true;
            t1.Start();

            Thread t2 = new Thread(() =>
            {
                Console.WriteLine("客户2在排队等待付钱...");

                //客户2调用manualResetEvent上的WaitOne来等待付钱通知
                manualResetEvent.WaitOne();
                Console.WriteLine("已经有人请客，客户2不用付钱！");
            });
            t2.IsBackground = true;
            t2.Start();

            Pay();//发送通知

            Console.ReadKey();
        }

        static void Pay()
        {
            string tip = Console.ReadLine();
            if (tip == "next")
            {
                manualResetEvent.Set();//收银员发送付钱通知，通过调用Set来通知客户付钱
            }
        }
    }
}
