using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ServiceStack.Redis;

namespace RedisMVCDemo.Redis
{
    public class RedisManager
    {
        /// <summary>
        /// redis配置文件信息
        /// </summary>
        private static RedisConfigInfo redisConfigInfo = RedisConfigInfo.GetConfig();

        /// <summary>
        /// redis程序池客户端对象
        /// </summary>
        private static PooledRedisClientManager prcm;
        /// <summary>
        /// 静态构造方法，初始化连接池管理对象
        /// </summary>
        static RedisManager()
        {
            CreateManager();
        }

        /// <summary>
        /// 创建连接池管理对象
        /// </summary>
        private static void CreateManager()
        {
            string[] writeServerList = redisConfigInfo.WriteServerList.Split(new char[] {','});
            string[] readServerList = redisConfigInfo.ReadServerList.Split(new char[] {','});

            prcm = new PooledRedisClientManager(readServerList,writeServerList,new RedisClientManagerConfig
            {
                MaxWritePoolSize = redisConfigInfo.MaxWritePoolSize,
                MaxReadPoolSize = redisConfigInfo.MaxReadPoolSize,
                AutoStart = redisConfigInfo.AutoStart,
            });

        }
        /// <summary>
        /// 客户端缓存操作对象
        /// </summary>
        /// <returns></returns>
        public static IRedisClient GetClient()
        {
            if (prcm == null)
            {
                CreateManager();
            }
            return prcm.GetClient();
        }
    }
}