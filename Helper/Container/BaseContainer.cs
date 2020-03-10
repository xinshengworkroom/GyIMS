using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GyIMS.Helper.Container
{
    public class BaseContainer
    {

        /// <summary>
        /// IOC 容器
        /// </summary>
        public static IContainer container = null;

        /// <summary>
        /// 获取 IDal 的实例化对象
        /// </summary>
        /// <typeparam name=“T”></typeparam>
        /// <returns></returns>
        public static IDAL Resolve<IDAL,DAL>()
        {
            try
            {
                container = null;

                if (container == null)
                {
                    Initialise<DAL, IDAL>();
                }
            }
            catch (System.Exception ex)
            {
                throw new System.Exception("IOC实例化出错!" + ex.Message);
            }

            return container.Resolve<IDAL>();
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public static void Initialise<DAL, IDAL>()
        {
           
            var builder = new ContainerBuilder();
            //格式：builder.RegisterType<xxxx>().As<Ixxxx>().InstancePerLifetimeScope();
            builder.RegisterType<DAL>().As<IDAL>().InstancePerLifetimeScope();
            container = builder.Build();
        }
    }
}