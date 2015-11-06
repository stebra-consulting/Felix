using Microsoft.SharePoint.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TrialAndErrorEventReceiverWeb.Models;

namespace TrialAndErrorEventReceiverWeb.Controllers
{
    [SharePointContextFilter]
    public class HomeController : Controller
    {
        
        public ActionResult Index()
        {
            Global.Msg = "Index(), ";
            ViewBag.GlobalMsg = Global.Msg;
            return View();
            
        }

        [SharePointContextFilter]
        public ActionResult About()
        {
            Global.Msg += "About(), ";

            var spContext = SharePointContextProvider.Current.GetSharePointContext(HttpContext);

            Global.Msg += "var spContext Set, ";

            using (var clientContext = spContext.CreateUserClientContextForSPHost())
            {
                if (clientContext != null)
                {
                    new RemoteEventReceiverManager().AssociateRemoteEventsToHostWeb(clientContext);
                }
                else
                {
                    Global.Msg += "clientContext is null, ";
                }
            }

            ViewBag.GlobalMsg = Global.Msg;//Show debug when published azure

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
