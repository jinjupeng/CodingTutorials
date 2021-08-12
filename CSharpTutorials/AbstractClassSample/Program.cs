using System;

namespace AbstractClass
{
    class Program
    {
        static void Main(string[] args)
        {
            var logger = new ConsoleLogger("name", enabled: true, LoggerLevel.Message);
            logger.Log(LoggerLevel.Message, "Hello World!");
        }
    }
}
