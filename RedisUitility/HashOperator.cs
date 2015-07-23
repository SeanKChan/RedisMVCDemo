using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ServiceStack.Text;

namespace RedisUitility.Redis
{
    public class HashOperator:RedisOperatorBase
    {
       
        public HashOperator() : base()
        {
        }

        /// <summary>
        /// 判断某个数据是否已经被缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="hashId"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool Exsit<T>(string hashId, string key)
        {
            return Redis.HashContainsEntry(hashId,key);
        }
        /// <summary>
        /// 存储数据到hash表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="hashId"></param>
        /// <param name="key"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public bool Set<T>(string hashId, string key, T t)
        {
            string value = JsonSerializer.SerializeToString<T>(t);
            return Redis.SetEntryInHash(hashId,key,value);
        }
        /// <summary>
        /// 移除hash表中的某值
        /// </summary>
        /// <param name="hashId"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool Remove(string hashId, string key)
        {
            return Redis.RemoveEntryFromHash(hashId, key);
        }
        /// <summary>
        /// 移除整个hash中key对应的记录
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool Remove(string key)
        {
            return Redis.Remove(key);
        }

        /// <summary>
        /// 从hash表中获取数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="hashId"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public T Get<T>(string hashId, string key)
        {
            string value = Redis.GetValueFromHash(hashId, key);
            return JsonSerializer.DeserializeFromString<T>(value);
        }

        public List<T> GetAll<T>(string hashId)
        {
            var valueList = Redis.GetHashValues(hashId);
            if (valueList == null || valueList.Count < 0)
            {
                return null;
            }
            List<T> list = new List<T>();

            valueList.ForEach(x =>
            {
                var value = JsonSerializer.DeserializeFromString<T>(x);
                list.Add(value);
            });
            return list;
        }
        /// <summary>
        /// 设置缓存过期时间
        /// </summary>
        /// <param name="key"></param>
        /// <param name="datetime"></param>
        public void SetExpire(string key, DateTime datetime)
        {
            Redis.ExpireEntryAt(key, datetime);
        }



    }
}