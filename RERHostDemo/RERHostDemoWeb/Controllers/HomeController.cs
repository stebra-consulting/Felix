using Microsoft.SharePoint.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RERHostDemoWeb.Controllers
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
                      "https://servicebosse.servicebus.windows.net/73036694/1885843836/obj/cc07a6a4-472d-4401-bc06-1dd3ef1fc80f/Services/RERHostReceiver.svc",
                          10);
                }
            }
            return RedirectToAction("Index", new { SPHostUrl = spContext.SPHostUrl.ToString() });
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
