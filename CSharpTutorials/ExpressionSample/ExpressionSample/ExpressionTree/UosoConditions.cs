using System;
using System.Collections.Generic;
using System.Text;

namespace TestDemo.ExpressionTree
{
    public class UosoConditions
    {
        /// <summary>
        /// 字段名称
        /// </summary>
        public string Key { get; set; }
        /// <summary>
        /// 值
        /// </summary>
        public string Value { get; set; }
        /// <summary>
        /// 值类型
        /// </summary>
        public string ValueType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public UosoOperatorEnum Operator { get; set; }
    }

    public enum UosoOperatorEnum
    {
        Contains,
        Equal,
        Greater,
        GreaterEqual,
        Less,
        LessEqual,
        NotEqual,
        Between,
        In
    }

    public class UosoOrderConditions
    {
        public string Key { get; set; }
        public OrderSequence Order { get; set; }
    }
    public enum OrderSequence
    {
        DESC = 0,
        ASC = 1,
    }
}
