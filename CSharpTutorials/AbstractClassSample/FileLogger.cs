using System.IO;

namespace AbstractClass
{
    /// <summary>
    /// 抽象类的子类：输出日志到文件
    /// </summary>
    public class FileLogger : Logger
    {
        private readonly FileStream fileWriter;

        public FileLogger(string name, bool enabled, LoggerLevel minPermittedLevel, string filepath) : base(name, enabled, minPermittedLevel)
        {
            this.fileWriter = new FileStream(filepath, FileMode.Create);
        }

        protected override void DoLog(LoggerLevel level, string mesage)
        {
            // 格式化level和message,输出到日志文件
            //开始写入
            byte[] data = System.Text.Encoding.Default.GetBytes(mesage);
            fileWriter.Write(data, 0, data.Length);
            //清空缓冲区、关闭流
            fileWriter.Flush();
            fileWriter.Close();
        }
    }
}
