using Microsoft.SharePoint.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StackExchange.Redis;

namespace FelixPHAWeb.Controllers
{
    public class HomeController : Controller
    {
        [SharePointContextFilter]
        public ActionResult Index()
        {
            var spContext = SharePointContextProvider.Current.GetSharePointContext(HttpContext);
            using (var clientContext = spContext.CreateUserClientContextForSPHost())
            {
                if (clientContext != null)
                {
                    ListCollection spLists = clientContext.Web.Lists;
                    clientContext.Load(spLists);
                    clientContext.ExecuteQuery();
                    List<string> listNames = new List<string>();
                    foreach (var list in spLists)
                    {
                        listNames.Add(list.Title);
                    }


                    ConnectionMultiplexer connection = ConnectionMultiplexer.Connect(
                        "memory.redis.cache.windows.net,abortConnect=false,ssl=true,password=nv2w3Oz+20mPCR2vVQVLCwUc3PAYpokJy02LK0JcEHQ=");

                    IDatabase cache = connection.GetDatabase();

                    string concatNames = "";
                    foreach (var listName in listNames)
                    {
                        concatNames += listName +"," ;
                    }

                    // Perform cache operations using the cache object...
                    // Simple put of integral data types into the cache
                    cache.StringSet("keyListNames", concatNames, TimeSpan.FromMinutes(60));
                    cache.StringSet("key1", "hello", TimeSpan.FromMinutes(60));
                    cache.StringSet("key1", "world", TimeSpan.FromMinutes(60));

                    // Simple get of data types from the cache
                    string key1 = cache.StringGet("keyListNames");

                    ViewBag.key1 = key1;
                    ViewBag.Lists = listNames;
                }
            }

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
