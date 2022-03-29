using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace MultiThreadingSample
{
    /// <summary>
    /// AutoResetEvent一次只能通知一个等待线程，通知后自动关闭
    /// 举例：像一个收费站，一次允许一辆车通过，并且通过之后自动关闭
    /// </summary>
    public class AutoResetEventTest
    {
        //若要将初始状态设置为终止，则为 true；若要将初始状态设置为非终止，则为 false
        static AutoResetEvent autoResetEvent = new AutoResetEvent(false);

        static void Main(string[] args)
        {
            Thread t1 = new Thread(() =>
            {
                Console.WriteLine("客户1在排队等待付钱...");

                //客户1调用AutoResetEvent上的WaitOne来等待付钱通知
                autoResetEvent.WaitOne();
                Console.WriteLine("通知来了，客户1付钱");
            });
            t1.IsBackground = true;
            t1.Start();

            Pay();//发送通知
            Console.ReadKey();
        }

        static void Pay()
        {
            string tip = Console.ReadLine();
            if (tip == "next")
            {
                autoResetEvent.Set();//收银员发送付钱通知，通过调用Set来通知客户付钱
            }
        }
    }
}
