using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RedisMVCDemo.Redis;
using RedisMVCDemo.Models;
using ServiceStack.Text;
using ServiceStack.Redis.Generic;

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
                    ViewBag.Title = "一共" + redisClient.GetListCount(ListId1) + "条数据";
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
                ViewBag.Title = "一共" + redisClient.GetListCount(ListId1) + "条数据";
            }


            return View();
        }

        public JsonResult ReStart()
        {
            System.Diagnostics.Process.Start("D:\\redis\\redis-server.exe"); //此处为Redis的存储路径
            //ViewBag.ProcessInfo = "服务已启动";
            var json = new
            {
                Success = true,
                Message = "服务已启动"
            };
            return Json(json, JsonRequestBehavior.AllowGet);
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

            return Json(json, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 使用IRedisTypeClient
        /// </summary>
        /// <returns></returns>
        public ActionResult Index1()
        {
            Person p1 = new Person() {Id = 1, Name = "刘备"};
            Person p2 = new Person() {Id = 2, Name = "关羽"};
            Person p3 = new Person() {Id = 3, Name = "张飞"};
            Person p4 = new Person() {Id = 4, Name = "曹操"};
            Person p5 = new Person() {Id = 5, Name = "典韦"};
            Person p6 = new Person() {Id = 6, Name = "郭嘉"};

            List<Person> personList = new List<Person>() {p2, p3, p4, p5, p6};

            using (var redisClient = RedisManager.GetClient())
            {
                IRedisTypedClient<Person> irPerson = redisClient.As<Person>();

                irPerson.DeleteAll();

                //---------------添加数据-----------------
                //添加单条数据
                irPerson.Store(p1);
                //添加多条数据
                irPerson.StoreAll(personList);

                //----------------查询数据----------------------------
                //使用store存数据，才能用GetAll方法获取
                Response.Write(irPerson.GetAll().First(x => x.Id == 1).Name);
                Response.Write("</br>");
                Response.Write(irPerson.GetAll().First(x => x.Id == 2).Name);
                Response.Write("</br>");

                //-----------------删除数据-------------------------------------

                irPerson.Delete(p1); //删除 刘备
                Response.Write(irPerson.GetAll().Count()); //5
                irPerson.DeleteById(2); //删除 关羽
                Response.Write(irPerson.GetAll().Count()); //4
                irPerson.DeleteByIds(new List<int> {3, 4}); //删除张飞 曹操
                Response.Write(irPerson.GetAll().Count()); //2
                irPerson.DeleteAll(); //全部删除
                Response.Write(irPerson.GetAll().Count()); //0

            }

            return Content("");

        }

        public ActionResult Index2()
        {
            Person p1 = new Person() { Id = 1, Name = "刘备" };
            Person p2 = new Person() { Id = 2, Name = "关羽" };
            Person p3 = new Person() { Id = 3, Name = "张飞" };
            Person p4 = new Person() { Id = 4, Name = "曹操" };
            Person p5 = new Person() { Id = 5, Name = "典韦" };
            Person p6 = new Person() { Id = 6, Name = "郭嘉" };

            List<Person> personList = new List<Person>() {p1, p2, p3, p4, p5, p6 };
            using (var redisClient = RedisManager.GetClient())
            {
                IRedisTypedClient<Person> irPerson = redisClient.As<Person>();
                irPerson.StoreAll(personList);

                //读取所有key
                var keys = irPerson.GetAllKeys();
                foreach (var item in keys)
                {
                    Response.Write(item + "</br>");
                }

                //修改只能通过key进行修改
                Person p7 = new Person {Id = 7,Name = "王大锤"};
                irPerson.SetEntry("urn:person:1", p7);
                Response.Write(irPerson.GetAll().First(x => x.Id == 7).Name);
            }
            return Content("");
        }
    }
}