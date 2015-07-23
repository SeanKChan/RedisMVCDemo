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
        private static readonly string ListId1 = "UserList";
        // GET: RedisTest
        public ActionResult Index()
        {
            using (var redisClient = RedisManager.GetClient())
            {
               

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

                if (redisClient.GetListCount(ListId1) > 0)
                {
                    var tempUserList = redisClient.GetAllItemsFromList(ListId1);
                    List<UserModels> userList = new List<UserModels>();
                    tempUserList.ForEach(x =>
                    {
                        var item = JsonSerializer.DeserializeFromString<UserModels>(x);
                        userList.Add(item);
                    });
                    //return PartialView("UserList", userList);
                    ViewBag.UserList = userList;
                    ViewBag.Title = "一共多少" + redisClient.GetListCount(ListId1)+"条数据";
                }

              
            }
            return View();
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Index(UserModels model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            using (var redisClient = RedisManager.GetClient())
            {
                model.Id = redisClient.GetListCount(ListId1) + 1;
                redisClient.AddItemToList(ListId1, JsonSerializer.SerializeToString<UserModels>(model));
                var tempUserList = redisClient.GetAllItemsFromList(ListId1);
                List<UserModels> userList = new List<UserModels>();
                tempUserList.ForEach(x =>
                {
                    var item = JsonSerializer.DeserializeFromString<UserModels>(x);
                    userList.Add(item);
                });
              
                ViewBag.UserList = userList;
                ViewBag.Title = "一共多少" + redisClient.GetListCount(ListId1) + "条数据";
            }


            return View();
        }

        public JsonResult ReStart()
        {
            System.Diagnostics.Process.Start("D:\\redis\\redis-server.exe");//此处为Redis的存储路径
            //ViewBag.ProcessInfo = "服务已启动";
            var json = new
            {
                Success = true,
                Message = "服务已启动"
            };
            return Json(json,JsonRequestBehavior.AllowGet);
        }

        public JsonResult Delete(int id)
        {
            using (var redisClient = RedisManager.GetClient())
            {
                var userList = redisClient.Lists[ListId1];
                userList.RemoveAt(id);
            }
            var json = new
            {
                Success = true
            };

            return Json(json,JsonRequestBehavior.AllowGet);
        }

       


    }
}