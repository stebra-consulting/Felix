using Microsoft.SharePoint.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Table;
using Microsoft.Azure;
using System.Configuration;
using AzureTableTestWeb.Models;

namespace AzureTableTestWeb.Controllers
{
    public class HomeController : Controller
    {
        [SharePointContextFilter]
        public ActionResult Index()
        {
            var storageAccount = CloudStorageAccount.Parse(
               CloudConfigurationManager.GetSetting("StorageConnectionString"));

            var client = storageAccount.CreateCloudTableClient();

            var stebraTable = client.GetTableReference("StebraList");

            var stebraQuery = new TableQuery<StebraEntity>()
               .Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, "WeAreStebra"));

            var stebras = stebraTable.ExecuteQuery(stebraQuery).ToList();


            return View(stebras);
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
