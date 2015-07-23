using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ServiceStack.Redis;

namespace RedisUitility.Redis
{
    /// <summary>
    /// redis操作基类(主要用于释放内存)
    /// </summary>
    public abstract class RedisOperatorBase:IDisposable
    {

        protected IRedisClient Redis { get; private set; }

        /// <summary>
        /// 标识（用于判断redis是否已经释放内存）
        /// </summary>
        private bool _disposed = false;

        /// <summary>
        /// 自定义构造函数
        /// </summary>
        protected RedisOperatorBase()
        {
            Redis = RedisManager.GetClient();
        }
        /// <summary>
        /// 内存释放
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    Redis.Dispose();
                    Redis = null;
                }
            }
            this._disposed = true;
        }
        /// <summary>
        /// 实现IDispose接口中的方法
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 保存数据DB到硬盘
        /// </summary>
        public void Save()
        {
            Redis.Save();
        }

        /// <summary>
        /// 异步保存数据DB到硬盘
        /// </summary>
        public void SaveAsync()
        {
            Redis.SaveAsync();
        }
    }
}