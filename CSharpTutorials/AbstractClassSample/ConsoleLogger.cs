using System;

namespace AbstractClass
{
    /// <summary>
    /// 抽象类的子类: 输出日志到控制台
    /// </summary>
    public class ConsoleLogger : Logger
    {
        public ConsoleLogger(string name, bool enabled, LoggerLevel minPermittedLevel) 
            : base(name, enabled, minPermittedLevel)
        {
        }

        protected override void DoLog(LoggerLevel level, string mesage)
        {
            Console.WriteLine(mesage);
        }
    }
}
