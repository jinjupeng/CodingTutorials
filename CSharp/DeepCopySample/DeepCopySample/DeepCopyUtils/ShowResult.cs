using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace DeepCopySample.DeepCopyUtils
{
    public static class ShowResult
    {
        public static void Show()
        {
            //0. 准备实体
            User user = new User()
            {
                id = 1,
                userName = "ceshi",
                userAge = 3
            };
            long time1 = 0;
            long time2 = 0;
            long time3 = 0;
            long time4 = 0;
            long time5 = 0;

            #region 1-直接硬编码的形式
            {
                Task.Run(() =>
                {
                    Stopwatch watch = new Stopwatch();
                    watch.Start();
                    for (int i = 0; i < 1000000; i++)
                    {
                        UserCopy userCopy = new UserCopy()
                        {
                            id = user.id,
                            userName = user.userName,
                            userAge = user.userAge,
                        };
                    }
                    watch.Stop();
                    time1 = watch.ElapsedMilliseconds;
                    Console.WriteLine("方案1所需要的时间为:{0}", time1);
                });

            }
            #endregion

            #region 2-反射遍历属性
            {
                Task.Run(() =>
                {
                    Stopwatch watch = new Stopwatch();
                    watch.Start();
                    for (int i = 0; i < 1000000; i++)
                    {
                        DeepCopyUtils.ReflectionMapper<User, UserCopy>(user);
                    }
                    watch.Stop();
                    time2 = watch.ElapsedMilliseconds;
                    Console.WriteLine("方案2所需要的时间为:{0}", time2);
                });
            }
            #endregion

            #region 3-序列化和反序列化
            {
                Task.Run(() =>
                {
                    Stopwatch watch = new Stopwatch();
                    watch.Start();
                    for (int i = 0; i < 1000000; i++)
                    {
                        DeepCopyUtils.SerialzerMapper<User, UserCopy>(user);
                    }
                    watch.Stop();
                    time3 = watch.ElapsedMilliseconds;
                    Console.WriteLine("方案3所需要的时间为:{0}", time3);
                });

            }
            #endregion

            #region 04-字典缓存+表达式目录树
            {
                Task.Run(() =>
                {
                    Stopwatch watch = new Stopwatch();
                    watch.Start();
                    for (int i = 0; i < 1000000; i++)
                    {
                        DeepCopyUtils.DicExpressionMapper<User, UserCopy>(user);
                    }
                    watch.Stop();
                    time4 = watch.ElapsedMilliseconds;
                    Console.WriteLine("方案4所需要的时间为:{0}", time4);
                });
            }
            #endregion

            #region 05-泛型缓存+表达式目录树
            {
                Task.Run(() =>
                {
                    Stopwatch watch = new Stopwatch();
                    watch.Start();
                    for (int i = 0; i < 1000000; i++)
                    {
                        GenericExpressionMapper<User, UserCopy>.DeepCopy(user);
                    }
                    watch.Stop();
                    time5 = watch.ElapsedMilliseconds;
                    Console.WriteLine("方案5所需要的时间为:{0}", time5);
                });
            }
            #endregion

        }
    }
}
