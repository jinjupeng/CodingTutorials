using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace TestDemo.ExpressionTree
{
    public static class ExpressionExtensions
    {
        /// <summary>
        /// 扩展查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="conditions"></param>
        /// <returns></returns>
        public static IQueryable<T> QueryConditions<T>(this IQueryable<T> query, IEnumerable<UosoConditions> conditions) where T : class
        {
            var parser = new UosoExpressionParser<T>();
            var filter = parser.ParserConditions(conditions);
            return query.Where(filter);
        }

        /// <summary>
        /// 扩展多条件排序
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="orderConditions"></param>
        /// <returns></returns>
        public static IQueryable<T> OrderConditions<T>(this IQueryable<T> query, IEnumerable<UosoOrderConditions> orderConditions)
        {
            foreach (var orderinfo in orderConditions)
            {
                var t = typeof(T);
                var propertyInfo = t.GetProperty(orderinfo.Key);
                var parameter = Expression.Parameter(t);
                Expression propertySelector = Expression.Property(parameter, propertyInfo);

                var orderby = Expression.Lambda<Func<T, object>>(propertySelector, parameter);
                if (orderinfo.Order == OrderSequence.DESC)
                    query = query.OrderByDescending(orderby);
                else
                    query = query.OrderBy(orderby);

            }
            return query;
        }

        /// <summary>
        /// 扩展分页
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="pageindex"></param>
        /// <param name="pagesize"></param>
        /// <param name="itemCount"></param>
        /// <returns></returns>
        public static IQueryable<T> Pager<T>(this IQueryable<T> query, int pageindex, int pagesize, out int itemCount)
        {
            itemCount = query.Count();
            if (pageindex == 0 || pagesize == 0)
            {
                return query;
            }
            else
            {
                return query.Skip((pageindex - 1) * pagesize).Take(pagesize);
            }
        }

        #region 扩展and和or方法

        // https://www.cnblogs.com/liyouming/p/9447984.html

        /// <summary>
        /// True，只能选择And
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static Expression<Func<T, bool>> True<T>() { return _ => true; }

        /// <summary>
        /// False,对应只能Or
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static Expression<Func<T, bool>> False<T>() { return _ => false; }


        /// <summary>
        /// 添加And条件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <returns></returns>
        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> first, Expression<Func<T, bool>> second)
        {
            return first.AndAlso<T>(second, Expression.AndAlso);
        }

        /// <summary>
        /// 添加Or条件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <returns></returns>
        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> first, Expression<Func<T, bool>> second)
        {
            return first.AndAlso<T>(second, Expression.OrElse);
        }

        /// <summary>
        /// 合并表达式以及参数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expr1"></param>
        /// <param name="expr2"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        private static Expression<Func<T, bool>> AndAlso<T>(this Expression<Func<T, bool>> expr1, Expression<Func<T, bool>> expr2, Func<Expression, Expression, BinaryExpression> func)
        {
            var parameter = Expression.Parameter(typeof(T));
            var leftVisitor = new ReplaceExpressionVisitor(expr1.Parameters[0], parameter);
            var left = leftVisitor.Visit(expr1.Body);

            var rightVisitor = new ReplaceExpressionVisitor(expr2.Parameters[0], parameter);
            var right = rightVisitor.Visit(expr2.Body);

            return Expression.Lambda<Func<T, bool>>(func(left, right), parameter);
        }


        private class ReplaceExpressionVisitor : ExpressionVisitor
        {
            private readonly Expression _oldValue;
            private readonly Expression _newValue;
            public ReplaceExpressionVisitor(Expression oldValue, Expression newValue)
            {
                _oldValue = oldValue;
                _newValue = newValue;
            }

            public override Expression Visit(Expression node)
            {
                if (node == _oldValue)
                    return _newValue;
                return base.Visit(node);
            }
        }
        #endregion
    }
}
