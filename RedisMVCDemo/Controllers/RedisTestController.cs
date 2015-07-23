using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RedisMVCDemo.Redis;
using RedisMVCDemo.Models;
using ServiceStack.Text;

namespace RedisMVCDemo.Controllers
{
    public class RedisTestController : Controller
    {
        // GET: RedisTest
        public ActionResult Index()
        {
            using (var redisClient = RedisManager.GetClient())
            {
                string listId = "UserList";

               /*
                //先伪造一些数据
                var qiujialong = new UserModels
                {
                    Id = 0,
                    Name = "qiujialong",
                    Department = ".NET"
                };
                redisClient.AddItemToList(listId, JsonSerializer.SerializeToString<UserModels>(qiujialong));
                var chenxingxing = new UserModels
                {
                    Id = 1,
                    Name = "chenxingxing ",
                    Department = ".NET"
                };
                redisClient.AddItemToList(listId, JsonSerializer.SerializeToString<UserModels>(chenxingxing));
                var luwei = new UserModels
                {
                    Id = 2,
                    Name = "luwei ",
                    Department = ".NET"
                };
                
                redisClient.AddItemToList(listId, JsonSerializer.SerializeToString<UserModels>(luwei));
                var zhourui = new UserModels
                {
                    Id = 3,
                    Name = "zhourui ",
                    Department = "Java"
                };
                redisClient.AddItemToList(listId, JsonSerializer.SerializeToString<UserModels>(zhourui));
*/

                if (redisClient.GetListCount(listId) > 0)
                {
                    var tempUserList = redisClient.GetAllItemsFromList(listId);
                    List<UserModels> userList = new List<UserModels>();
                    tempUserList.ForEach(x =>
                    {
                        var item = JsonSerializer.DeserializeFromString<UserModels>(x);
                        userList.Add(item);
                    });
                    //return PartialView("UserList", userList);
                    ViewBag.UserList = userList;
                    ViewBag.Title = "一共多少" + redisClient.GetListCount(listId)+"条数据";
                }

              
            }
            return View();
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Add()
        {
            return View("Index");
        }

        public ActionResult ReStart()
        {
            System.Diagnostics.Process.Start("D:\\redis\\redis-server.exe");//此处为Redis的存储路径
            ViewBag.ProcessInfo = "服务已启动";
            return View("Index");
        }
    }
}