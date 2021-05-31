using System;

namespace MVCSample.Models
{
    /// <summary>
    /// https://www.c-sharpcorner.com/article/entity-framework-core-inmemory-database/
    /// https://blog.csdn.net/cslx5zx5/article/details/112272629
    /// </summary>
    public class FundModel
    {
        /// <summary>
        /// 主键
        /// </summary>
        public int ID { get; set; }
        public DateTime CurrentDateTime { get; set; }


    }
}
