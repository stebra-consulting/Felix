using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StackExchange.Redis;

namespace MAA_Public.Controllers
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
            //cache.StringSet("key1", "value");
            //cache.StringSet("key2", 25);

            // Simple get of data types from the cache


            string keyListNames = cache.StringGet("keyListNames");

            if (keyListNames == null) {keyListNames = "cache not found";}

            List<string> listNames = new List<string>();

            listNames = keyListNames.Split(',').ToList();

            ViewBag.listNames = listNames;

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