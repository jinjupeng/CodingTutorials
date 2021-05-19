using System;

namespace GCSample
{
    /// <summary>
    /// 标准Dispose模式
    /// https://mp.weixin.qq.com/s/OjEqlNg8CSzszZRs39gA_A
    /// https://www.cnblogs.com/wilber2013/archive/2004/01/13/4357910.html
    /// https://www.cnblogs.com/wilber2013/archive/2004/01/13/4362760.html
    /// 析构函数：被动清理
    /// Dispose：主动清理
    /// </summary>
    public class StandardDispose : IDisposable
    {
        /// <summary>
        /// 演示创建一个非托管资源
        /// </summary>
        private string _unmanagedResource = "未被托管的资源";

        /// <summary>
        /// 演示创建一个托管资源
        /// </summary>
        private string _manageResource = "托管的资源";

        /// <summary>
        /// 是否已释放
        /// </summary>
        private bool _disposed = false;

        /// <summary>
        /// 实现IDisposable中的Dispose方法
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true); // 必须为true
            GC.SuppressFinalize(this); // 通知垃圾回收机制不再调用终结器（析构器）（析构函数）
        }

        /// <summary>
        /// 不是必要的，提供一个Close方法仅仅是为了更符合其他语言（如C++）的规范
        /// </summary>
        public void Close()
        {
            this.Dispose();
        }

        /// <summary>
        /// 析构函数
        /// 必须，以备程序员忘记了显式调用Dispose方法
        /// </summary>
        ~StandardDispose()
        {
            // 必须为false
            this.Dispose(false);
        }

        /// <summary>
        /// 非密封类修饰用protected virtual
        /// 密封类修饰用private
        /// </summary>
        /// <param name="disposing">是否清理托管资源</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!this._disposed) // 避免已经被释放的抛异常
            {
                if (disposing)
                {
                    // 清理托管资源
                    if (this._manageResource != null)
                    {
                        // Dispose
                        this._manageResource = null;
                    }
                }

                // 清理非托管资源
                if (this._unmanagedResource != null)
                {
                    // Dispose  conn.Dispose()
                    this._unmanagedResource = null;
                }
            }

            // 让类型知道自己已经被释放
            this._disposed = true;
        }

        public void PublicMethod()
        {
            if (this._disposed)
            {
                throw new ObjectDisposedException("StandardDispose", "StandardDispose is disposed");
            }
        }
    }
}