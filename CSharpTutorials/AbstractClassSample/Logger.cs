using System.IO;

namespace AbstractClass
{
    /// <summary>
    /// 抽象类
    /// 特性：
    /// 1、抽象类不允许被实例化，只能被继承。也就是说，你不能 new 一个抽象类的对象出来（Logger logger = new Logger(…); 会报编译错误）。
    /// 2、抽象类可以包含属性和方法。方法既可以包含代码实现（比如 Logger 中的 log() 方法），也可以不包含代码实现（比如 Logger 中的 doLog() 方法）。不包含代码实现的方法叫作抽象方法。
    /// 3、子类继承抽象类，必须实现抽象类中的所有抽象方法。对应到例子代码中就是，所有继承 Logger 抽象类的子类，都必须重写 doLog() 方法。
    /// 
    /// 接口特性：
    /// 1、接口不能包含属性（也就是成员变量）。
    /// 2、接口只能声明方法，方法不能包含代码实现。
    /// 3、类实现接口的时候，必须实现接口中声明的所有方法。
    /// </summary>
    public abstract class Logger : ILogger
    {
        private readonly string name;
        private readonly bool enabled;
        private readonly LoggerLevel minPermittedLevel;

        public Logger(string name, bool enabled, LoggerLevel minPermittedLevel)
        {
            this.name = name;
            this.enabled = enabled;
            this.minPermittedLevel = minPermittedLevel;
        }

        /// <summary>
        /// 实现继承的接口中的方法
        /// </summary>
        /// <param name="level"></param>
        /// <param name="message"></param>
        public virtual void Log(LoggerLevel level, string message)
        {
            bool loggable = enabled && ((int)minPermittedLevel <= (int)level);
            if (!loggable) return;
            DoLog(level, message);
        }

        /// <summary>
        /// 抽象方法
        /// </summary>
        /// <param name="level"></param>
        /// <param name="message"></param>
        protected abstract void DoLog(LoggerLevel level, string message);
    }
}
