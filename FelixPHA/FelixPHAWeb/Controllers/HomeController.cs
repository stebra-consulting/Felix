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
                    //get listdata from sharepoint
                    ListCollection spLists = clientContext.Web.Lists;
                    clientContext.Load(spLists);
                    clientContext.ExecuteQuery();

                    //in this example i will store a string of all listnames in concatNames
                    string concatNames = "";
                    foreach (var list in spLists)
                    {
                        concatNames += "," + list.Title;
                    }

                    ConnectionMultiplexer connection = ConnectionMultiplexer.Connect(
                        "memory.redis.cache.windows.net,abortConnect=false,ssl=true,password=nv2w3Oz+20mPCR2vVQVLCwUc3PAYpokJy02LK0JcEHQ=");

                    IDatabase cache = connection.GetDatabase();

                    // Perform cache operations using the cache object...
                    // Simple put of data into the cache
                    cache.StringSet("keyListNames", concatNames, TimeSpan.FromMinutes(60));

                    ViewBag.Lists = concatNames;
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
