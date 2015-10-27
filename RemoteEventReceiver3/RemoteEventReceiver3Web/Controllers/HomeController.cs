using Microsoft.SharePoint.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RemoteEventReceiver3Web.Models;

namespace RemoteEventReceiver3Web.Controllers
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
                    var spWeb = clientContext.Web;
                    var hostListColl = spWeb.Lists;
                    clientContext.Load(spWeb, w => w.Id);
                    clientContext.Load(hostListColl);
                    clientContext.ExecuteQuery();
                    ViewBag.HostLists = hostListColl.Select(l => new SelectListItem() { Text = l.Title, Value = l.Title });
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

        [SharePointContextFilter]
        [HttpPost]
        public ActionResult Subscribe(string listTitle)
        {
            var spContext = SharePointContextProvider.Current.GetSharePointContext(HttpContext);
            using (var clientContext = spContext.CreateUserClientContextForSPHost())
            {
                if (!string.IsNullOrEmpty(listTitle))
                {
                    RERUtility.AddListItemRemoteEventReceiver(
                      clientContext,
                      listTitle,
                      EventReceiverType.ItemAdded,
                      EventReceiverSynchronization.Asynchronous,
                      "RERHostReceiver",
                      "https://serviceberra.servicebus.windows.net/73036694/1885843836/obj/426c50da-11a0-4781-9047-698c594f8bc3/Services/Receiver2.svc",
                          10);
                }
            }
            return RedirectToAction("Index", new { SPHostUrl = spContext.SPHostUrl.ToString() });
        }
    }
}
