using System;

namespace ColtSmart
{
    public class EnjoyGlobals
    {
        private static IServiceProvider appGlobalServiceProvider = null;

        /// <summary>
        /// 设置全局服务提供者
        /// </summary>
        /// <param name="serviceProvider"></param>
        public static void SetGlobalAppServiceProvider(IServiceProvider serviceProvider)
        {
            appGlobalServiceProvider = serviceProvider;
        }

        /// <summary>
        /// 全局服务提供者
        /// </summary>
        public static IServiceProvider ServiceProvider { get { return appGlobalServiceProvider; } }
    }
}
