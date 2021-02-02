using System;

namespace ref_out
{
   class Program
    {
        // 总结ref和out的区别https://blog.csdn.net/qq373011556/article/details/81944690

        // 通俗易懂的ref和out区别https://segmentfault.com/a/1190000019906493

        static void Main(string[] args)
        {
            Program pg = new Program();
            int x = 10;
            int y = 20;
            pg.GetValue(ref x, ref y);
            Console.WriteLine("x={0},y={1}", x, y);
            Console.ReadLine();
        }

        public void GetValue(ref int x, ref int y)
        {
            x = 521;
            y = 520;
        }


        /*      static void Main(string[] args)
                {
                    Program pg = new Program();
                    int x;
                    int y;        // 此处x,y没有进行初始化，则编译不通过。
                    pg.GetValue(ref x, ref y);
                    Console.WriteLine("x={0},y={1}", x, y);

                    Console.ReadLine();
                }

                public void GetValue(ref int x, ref int y)
                {
                    x = 1000;
                    y = 1;
                }
        */

        /*
                static void Main(string[] args)
                {
                    Program pg = new Program();
                    int x = 10;
                    int y = 233;
                    pg.Swap(out x, out y);
                    Console.WriteLine("x={0},y={1}", x, y);

                    Console.ReadLine();
                }

                public void Swap(out int a, out int b)
                {
                    int temp = a;   //a,b在函数内部没有赋初值，则出现错误。
                    a = 521;
                    b = 520;
                }
        */
    }
}
