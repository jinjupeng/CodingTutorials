using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace DeepCopySample.DeepCopyUtils
{
    /// <summary>
    /// 深拷贝的几种方法实现
    /// </summary>
    public static class DeepCopyUtils
    {
        //字典缓存
        private static Dictionary<string, object> _Dic = new Dictionary<string, object>();

        #region 反射的方式进行实体间的赋值

        /// <summary>
        /// 反射的方式进行实体间的赋值
        /// </summary>
        /// <typeparam name="TIn">赋值的实体类型</typeparam>
        /// <typeparam name="TOut">被赋值的实体类型</typeparam>
        /// <param name="tIn"></param>
        public static TOut ReflectionMapper<TIn, TOut>(TIn tIn)
        {
            TOut tOut = Activator.CreateInstance<TOut>();
            //外层遍历获取【被赋值的实体类型】的属性
            foreach (var itemOut in tOut.GetType().GetProperties())
            {
                //内层遍历获取【赋值的实体类型】的属性
                foreach (var itemIn in tIn.GetType().GetProperties())
                {
                    if (itemOut.Name.Equals(itemIn.Name))
                    {
                        //特别注意这里：SetValue和GetValue的用法
                        itemOut.SetValue(tOut, itemIn.GetValue(tIn));
                        break;
                    }
                }
            }
            return tOut;
        }
        #endregion

        #region 封装-序列化反序列化进行实体见的赋值

        /// <summary>
        /// 序列化反序列化进行实体见的赋值
        /// </summary>
        /// <param name="TIn">赋值的实体类型</typeparam>
        /// <typeparam name="TOut">被赋值的实体类型</typeparam>
        /// <param name="tIn"></param>
        public static TOut SerialzerMapper<TIn, TOut>(TIn tIn)
        {
            return JsonConvert.DeserializeObject<TOut>(JsonConvert.SerializeObject(tIn));
        }

        #endregion

        #region 字典缓存+表达式目录树

        /// <summary>
        /// 字典缓存 + 表达式目录树 = 高效深拷贝
        /// </summary>
        /// <typeparam name="TIn">赋值的实体类型</typeparam>
        /// <typeparam name="TOut">被赋值的实体类型</typeparam>
        /// <param name="tIn"></param>
        public static TOut DicExpressionMapper<TIn, TOut>(TIn tIn)
        {
            string key = string.Format("funckey_{0}_{1}", typeof(TIn).FullName, typeof(TOut).FullName);
            if (!_Dic.ContainsKey(key))
            {
                ParameterExpression parameterExpression = Expression.Parameter(typeof(TIn), "p");
                List<MemberBinding> memberBindingList = new List<MemberBinding>();
                foreach (var item in typeof(TOut).GetProperties())
                {
                    MemberExpression property = Expression.Property(parameterExpression, typeof(TIn).GetProperty(item.Name));
                    MemberBinding memberBinding = Expression.Bind(item, property);
                    memberBindingList.Add(memberBinding);
                }
                MemberInitExpression memberInitExpression = Expression.MemberInit(Expression.New(typeof(TOut)), memberBindingList.ToArray());
                Expression<Func<TIn, TOut>> lambda = Expression.Lambda<Func<TIn, TOut>>(memberInitExpression, new ParameterExpression[]
                {
                    parameterExpression
                });
                Func<TIn, TOut> func = lambda.Compile();//拼装是一次性的
                _Dic[key] = func;
            }
            return ((Func<TIn, TOut>)_Dic[key]).Invoke(tIn);
        }
        #endregion
    }

    /// <summary>
    /// 静态泛型类实现缓存 + 表达式树 = 高效的深拷贝
    /// </summary>
    /// <typeparam name="TIn"></typeparam>
    /// <typeparam name="TOut"></typeparam>
    public class GenericExpressionMapper<TIn, TOut>
        where TIn : class, new()
        where TOut : class, new()//需要无参构造函数，构造表达式树的时候需要利用无参构造函数创建对象
    {
        /// <summary>
        /// 映射表达式，泛型缓存每个类型存一份
        /// </summary>
        private static Func<TIn, TOut> _FUNC = null;

        /// <summary>
        /// 静态构造函数，每个泛型类型只会执行一次
        /// 静态类，在泛型类第一次传入具体的类型进来的时候，只执行一次。
        /// </summary>
        static GenericExpressionMapper()
        {
            ParameterExpression parameterExpression = Expression.Parameter(typeof(TIn), "p"); // 参数p :p => 
            List<MemberBinding> memberBindingList = new List<MemberBinding>();
            foreach (var item in typeof(TOut).GetProperties())
            {
                MemberExpression property = Expression.Property(parameterExpression, typeof(TIn).GetProperty(item.Name)); // p.Name
                MemberBinding memberBinding = Expression.Bind(item, property); // Name = p.Name
                memberBindingList.Add(memberBinding);
            }
            foreach (var item in typeof(TOut).GetFields())
            {
                MemberExpression property = Expression.Field(parameterExpression, typeof(TIn).GetField(item.Name));
                MemberBinding memberBinding = Expression.Bind(item, property);
                memberBindingList.Add(memberBinding);
            }
            MemberInitExpression memberInitExpression = Expression.MemberInit(Expression.New(typeof(TOut)), memberBindingList.ToArray());
            Expression<Func<TIn, TOut>> lambda = Expression.Lambda<Func<TIn, TOut>>(memberInitExpression, new ParameterExpression[]
            {
                     parameterExpression
            }); // p => new TOut() { Name = p.Name }
            _FUNC = lambda.Compile();//拼装是一次性的
        }

        /// <summary>
        /// 对象拷贝（拷贝private/public：实例成员、属性、静态成员）
        /// </summary>
        /// <param name="t">源对象</param>
        /// <returns></returns>
        public static TOut DeepCopy(TIn t)
        {
            return _FUNC(t);
        }

        /// <summary>
        /// 集合拷贝（拷贝private/public：实例成员、属性、静态成员）
        /// </summary>
        /// <param name="data">源</param>
        /// <returns></returns>
        public static List<TOut> DeepCopyList(List<TIn> data)
        {
            if (data == null)
            {
                return null;
            }
            if (data.Count == 0)
            {
                return new List<TOut>();
            }
            return data.Select(a => DeepCopy(a)).ToList();
        }
    }
}
