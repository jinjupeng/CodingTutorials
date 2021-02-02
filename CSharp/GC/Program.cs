using System;

namespace GC
{
    class Program
    {
        static void Main(string[] args)
        {
            var gc = new StandardDispose();
            gc.Close();
            gc.Dispose();
            gc.PublicMethod(); // 会抛出以释放资源的异常
            Console.WriteLine("Hello World!");
        }
    }
}
