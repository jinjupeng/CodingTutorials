using System;

namespace AbstractClassSample
{
    public abstract class AbClass // 抽象类
    {
        // 数据成员（字段和常量）不可以声明为abstract
        public int Count = 10; // 数据成员

        abstract public int MyInt { get; set; } // 抽象属性

        public void IdentifyBase() // 普通方法
        {
            Console.WriteLine("I am AbClass");
        }

        abstract public void IdentifyDerived(); // 抽象方法

    }

    public class DerivedClass : AbClass // 派生类
    {
        private int _myInt;

        public override int MyInt // 覆盖抽象属性
        {
            get { return _myInt; }
            set { _myInt = value; }
        }

        override public void IdentifyDerived() // 抽象方法的实现
        {
            Console.WriteLine("I am DerivedClass");
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            // AbClass a = new AbClass(); // 错误，抽象类不能被实例化

            DerivedClass b = new DerivedClass(); // 实例化派生类
            b.IdentifyBase(); // 调用继承的方法
            b.IdentifyDerived(); // 调用“抽象”方法
            b.MyInt = 28;
        }
    }

}
