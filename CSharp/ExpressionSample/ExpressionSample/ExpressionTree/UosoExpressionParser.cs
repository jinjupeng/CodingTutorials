using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace TestDemo.ExpressionTree
{
    public class UosoExpressionParser<T> where T : class
    {
        private ParameterExpression parameter;
        public Expression<Func<T, bool>> ParserConditions(IEnumerable<UosoConditions> conditions)
        {
            // 将条件转化成表达式的Body
            var query = ParseExpressionBody(conditions);
            return Expression.Lambda<Func<T, bool>>(query, parameter);
        }

        /// <summary>
        /// 条件转表达式
        /// </summary>
        /// <param name="conditions"></param>
        /// <returns></returns>
        private Expression ParseExpressionBody(IEnumerable<UosoConditions> conditions)
        {
            if (conditions == null || conditions.Count() == 0)
            {
                return Expression.Constant(true, typeof(bool));
            }
            else if (conditions.Count() == 1)
            {
                return ParseCondition(conditions.First());
            }
            else
            {
                Expression left = ParseCondition(conditions.First());
                Expression right = ParseExpressionBody(conditions.Skip(1));
                return Expression.AndAlso(left, right);
            }
        }

        private Expression ParseCondition(UosoConditions condition)
        {
            ParameterExpression p = parameter;
            Expression key = Expression.Property(p, condition.Key);
            Expression value = Expression.Constant(condition.Value);
            switch (condition.Operator)
            {
                case UosoOperatorEnum.Contains:
                    return Expression.Call(key, typeof(string).GetMethod("Contains", new Type[] { typeof(string) }), value); // 错误细节：https://www.cnblogs.com/goldenbiu/articles/10755120.html
                case UosoOperatorEnum.Equal:
                    return Expression.Equal(key, Expression.Convert(value, key.Type));
                case UosoOperatorEnum.Greater:
                    return Expression.GreaterThan(key, Expression.Convert(value, key.Type));
                case UosoOperatorEnum.GreaterEqual:
                    return Expression.GreaterThanOrEqual(key, Expression.Convert(value, key.Type));
                case UosoOperatorEnum.Less:
                    return Expression.LessThan(key, Expression.Convert(value, key.Type));
                case UosoOperatorEnum.LessEqual:
                    return Expression.LessThanOrEqual(key, Expression.Convert(value, key.Type));
                case UosoOperatorEnum.NotEqual:
                    return Expression.NotEqual(key, Expression.Convert(value, key.Type));
                case UosoOperatorEnum.In:
                    return ParaseIn(p, condition);
                case UosoOperatorEnum.Between:
                    return ParaseBetween(p, condition);
                default:
                    throw new NotImplementedException("不支持此操作");
            }
        }

        /// <summary>
        /// between条件转换
        /// </summary>
        /// <param name="parameter"></param>
        /// <param name="conditions"></param>
        /// <returns></returns>
        private Expression ParaseBetween(ParameterExpression parameter, UosoConditions conditions)
        {
            ParameterExpression p = parameter;
            Expression key = Expression.Property(p, conditions.Key);
            var valueArr = conditions.Value.Split(',');
            if (valueArr.Length != 2)
            {
                throw new NotImplementedException("ParaseBetween参数错误");
            }
            try
            {
                int.Parse(valueArr[0]);
                int.Parse(valueArr[1]);
            }
            catch
            {
                throw new NotImplementedException("ParaseBetween参数只能为数字");
            }
            Expression expression = Expression.Constant(true, typeof(bool));
            //开始位置
            Expression startvalue = Expression.Constant(int.Parse(valueArr[0]));
            Expression start = Expression.GreaterThanOrEqual(key, Expression.Convert(startvalue, key.Type));

            Expression endvalue = Expression.Constant(int.Parse(valueArr[1]));
            Expression end = Expression.GreaterThanOrEqual(key, Expression.Convert(endvalue, key.Type));
            return Expression.AndAlso(start, end);
        }

        /// <summary>
        /// in条件转换
        /// </summary>
        /// <param name="parameter"></param>
        /// <param name="conditions"></param>
        /// <returns></returns>
        private Expression ParaseIn(ParameterExpression parameter, UosoConditions conditions)
        {
            ParameterExpression p = parameter;
            Expression key = Expression.Property(p, conditions.Key);
            var valueArr = conditions.Value.Split(',');
            Expression expression = Expression.Constant(true, typeof(bool));
            foreach (var itemVal in valueArr)
            {
                Expression value = Expression.Constant(itemVal);
                Expression right = Expression.Equal(key, Expression.Convert(value, key.Type));
                expression = Expression.Or(expression, right);
            }
            return expression;
        }
    }
}
