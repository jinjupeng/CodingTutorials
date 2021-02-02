using System;

namespace yield
{
    class Program
    {
        // http://www.manongjc.com/article/55235.html
        /*
        yield return作用:
            在 return 时，保存当前函数的状态，下次调用时继续从当前位置处理。
        示例说明:
            如下代码所示，主函数使用 foreach 输出 GetNumbers() 方法的数据。函数一共有3个数据，调用了三次此函数的处理部分，而初始化部分只调用了一次。
        使用方法解析:
            这个函数在处理循环时可以每生成一个数据就返回一个数据让主函数进行处理。
            在单线程的程序中，由于不需要等所有数据都处理好再返回，所以可以减少对内存占用。
            比如说有3个数据，如果一次性处理好返回，需要占用3个内存单位，而一个个返回，只需要占用1个内存单位。
            在多线程的处理程序中，还可以加快程序的处理速度。当数据处理以后，主程序可以进行处理，而被调用函数可以继续处理一下个数据。
            一个经典的应用是Socket，主线程处理对Socket接收到的数据进行处理，而另外一个线程负责读取Socket的内容。
            当接收的数据量比较大时，两个线程可以加快处理速度。
         */
        static void Main(string[] args)
        {
            foreach (var item in GetNumbers())
                Console.WriteLine("Main process. item = " + item);
        }

        static IEnumerable<int> GetNumbers()
        {
            // 以[0, 1, 2] 初始化数列 list
            Console.WriteLine("Initializating...");
            List<int> list = new List<int>();
            for (int i = 0; i < 3; i++)
                list.Add(i);

            // 每次 yield return 返回一个list的数据
            Console.WriteLine("Processing...");
            for (int i = 0; i < list.Count; i++)
            {
                Console.WriteLine("Yield called.");
                yield return list[i];
            }
            Console.WriteLine("Done.");
        }
    }
}
