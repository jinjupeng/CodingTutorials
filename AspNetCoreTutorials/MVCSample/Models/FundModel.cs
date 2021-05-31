using MVCSample.Common;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        [ScaffoldColumn(false)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        /// <summary>
        /// 日期
        /// </summary>
        public DateTime CurrentDateTime { get; set; }

        /// <summary>
        /// 上证指数
        /// </summary>
        public decimal ShanghaiCompositeIndex { get; set; }

        /// <summary>
        /// 平安银行
        /// </summary>
        public decimal PinganBank { get; set; }
        public decimal PinganBankRelativeGain { get; set; }

        /// <summary>
        /// 贵州茅台
        /// </summary>
        public decimal MoutaiChina { get; set; }
        public decimal MoutaiChinaRelativeGain { get; set; }

        /// <summary>
        /// 中信建设
        /// </summary>
        public decimal CiciCitic { get; set; }
        public decimal CiciCiticRelativeGain { get; set; }

        /// <summary>
        /// 华兴源创
        /// </summary>
        public decimal SuzhouHYCTechnology { get; set; }
        public decimal SuzhouHYCTechnologyRelativeGain { get; set; }

        /// <summary>
        /// 同达创业
        /// </summary>
        public decimal ShanghaiTongdaVentureCapitalCo { get; set; }
        public decimal ShanghaiTongdaVentureCapitalCoRelativeGain { get; set; }

        /// <summary>
        /// 上证指数跌涨幅
        /// </summary>
        public decimal ShanghaiCompositeIndexCompute { get; set; }
    }
}
