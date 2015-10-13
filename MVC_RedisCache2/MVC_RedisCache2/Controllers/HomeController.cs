using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StackExchange.Redis;

namespace MVC_RedisCache2.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {

            ConnectionMultiplexer connection = ConnectionMultiplexer.Connect(
                "memory.redis.cache.windows.net,abortConnect=false,ssl=true,password=nv2w3Oz+20mPCR2vVQVLCwUc3PAYpokJy02LK0JcEHQ=");

            IDatabase cache = connection.GetDatabase();

            // Perform cache operations using the cache object...
            // Simple put of integral data types into the cache

            cache.StringSet("key1", "Cached String");
            cache.StringSet("key2", 555);

            // Simple get of data types from the cache
            string key1 = cache.StringGet("key1");
            int key2 = (int)cache.StringGet("key2");

            ViewBag.key1 = key1;
            ViewBag.key2 = key2;

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}