using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ServiceStack.Redis;

namespace RedisMVCDemo.Redis
{
    /// <summary>
    /// 创建连接池管理对象
    /// </summary>
    public class RedisManager
    {
        /// <summary>
        /// redis配置文件信息
        /// </summary>
        private static RedisConfigInfo _redisConfigInfo = RedisConfigInfo.GetConfig();

        /// <summary>
        /// redis程序池客户端对象
        /// </summary>
        private static PooledRedisClientManager _prcm;
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
            string[] writeServerList = _redisConfigInfo.WriteServerList.Split(new char[] {','});
            string[] readServerList = _redisConfigInfo.ReadServerList.Split(new char[] {','});

            _prcm = new PooledRedisClientManager(readServerList,writeServerList,new RedisClientManagerConfig
            {
                MaxWritePoolSize = _redisConfigInfo.MaxWritePoolSize,
                MaxReadPoolSize = _redisConfigInfo.MaxReadPoolSize,
                AutoStart = _redisConfigInfo.AutoStart,
            });

        }
        /// <summary>
        /// 获取客户端缓存操作对象
        /// </summary>
        /// <returns></returns>
        public static IRedisClient GetClient()
        {
            if (_prcm == null)
            {
                CreateManager();
            }
            return _prcm.GetClient();
        }
    }
}