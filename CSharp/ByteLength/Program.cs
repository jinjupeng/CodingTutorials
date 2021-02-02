using System;

namespace ByteLength
{
    class Program
    {
        /// <summary>
        /// C#中的int、long、float、double等类型都占多少个字节的内存
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            // sizeof 操作符是用来取得一个类型在内存中会占几个byte。
            Console.WriteLine("{0}: {1} byte(s) scope:[{2}-{3}]",
                nameof(Byte).PadLeft(8), 
                sizeof(byte).NumberPad(2),
                byte.MinValue.NumberPad(32, true), 
                byte.MaxValue.NumberPad(32));

            Console.WriteLine("{0}: {1} byte(s) scope:[{2}-{3}]",
                nameof(SByte).PadLeft(8), 
                sizeof(sbyte).NumberPad(2),
                sbyte.MinValue.NumberPad(32, true), 
                sbyte.MaxValue.NumberPad(32));

            Console.WriteLine("{0}: {1} byte(s) scope:[{2}-{3}]",
                nameof(Int16).PadLeft(8), 
                sizeof(short).NumberPad(2),
                short.MinValue.NumberPad(32, true), 
                short.MaxValue.NumberPad(32));

            Console.WriteLine("{0}: {1} byte(s) scope:[{2}-{3}]",
                nameof(UInt16).PadLeft(8), 
                sizeof(ushort).NumberPad(2),
                ushort.MinValue.NumberPad(32, true), 
                ushort.MaxValue.NumberPad(32));

            Console.WriteLine("{0}: {1} byte(s) scope:[{2}-{3}]",
                nameof(Int32).PadLeft(8), 
                sizeof(int).NumberPad(2),
                int.MinValue.NumberPad(32, true), 
                int.MaxValue.NumberPad(32));

            Console.WriteLine("{0}: {1} byte(s) scope:[{2}-{3}]",
                nameof(UInt32).PadLeft(8), 
                sizeof(uint).NumberPad(2),
                uint.MinValue.NumberPad(32, true), 
                uint.MaxValue.NumberPad(32));

            Console.WriteLine("{0}: {1} byte(s) scope:[{2}-{3}]",
                nameof(Int64).PadLeft(8), 
                sizeof(long).NumberPad(2),
                long.MinValue.NumberPad(32, true), 
                long.MaxValue.NumberPad(32));

            Console.WriteLine("{0}: {1} byte(s) scope:[{2}-{3}]",
                nameof(UInt64).PadLeft(8), 
                sizeof(ulong).NumberPad(2),
                ulong.MinValue.NumberPad(32, true), 
                ulong.MaxValue.NumberPad(32));

            Console.WriteLine("{0}: {1} byte(s) scope:[{2}-{3}]",
                nameof(Single).PadLeft(8), 
                sizeof(float).NumberPad(2),
                float.MinValue.NumberPad(32, true), 
                float.MaxValue.NumberPad(32));

            Console.WriteLine("{0}: {1} byte(s) scope:[{2}-{3}]",
                nameof(Double).PadLeft(8), 
                sizeof(double).NumberPad(2),
                double.MinValue.NumberPad(32, true), 
                double.MaxValue.NumberPad(32));

            Console.WriteLine("{0}: {1} byte(s) scope:[{2}-{3}]",
                nameof(Decimal).PadLeft(8), 
                sizeof(decimal).NumberPad(2),
                decimal.MinValue.NumberPad(32, true), 
                decimal.MaxValue.NumberPad(32));

            Console.WriteLine("{0}: {1} byte(s)",
                nameof(Boolean).PadLeft(8), sizeof(bool).NumberPad(2));

            Console.WriteLine("{0}: {1} byte(s)",
                nameof(Char).PadLeft(8), sizeof(char).NumberPad(2));

            Console.WriteLine("{0}: {1} byte(s) ",
                nameof(IntPtr).PadLeft(8), IntPtr.Size.NumberPad(2));
            Console.ReadLine();
        }

        // 注意：以上结果需要注意，在32位系统中，IntPtr为4字节，在64位系统中，IntPtr为8字节。
        // [C#开发笔记之22-C#中的int、long、float、double等类型都占多少个字节的内存](https://www.byteflying.com/archives/4396)
        public static string NumberPad<T>(this T value, int length, bool right = false)
        {
            if (right)
            {
                return value.ToString().PadRight(length);
            }
            else
            {
                return value.ToString().PadLeft(length);
            }
        }
    }
}
