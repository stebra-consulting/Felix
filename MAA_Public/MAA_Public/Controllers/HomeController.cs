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

            // Simple get of data types from the cache
            string keyListNames = cache.StringGet("keyListNames");

            // Check if cache target is null
            if (keyListNames == null) {keyListNames = "cache not found";}

            // Convert string "listname1, listname2, listname3..." to List<string>
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