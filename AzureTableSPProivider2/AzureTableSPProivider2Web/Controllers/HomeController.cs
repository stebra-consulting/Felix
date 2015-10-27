using Microsoft.Azure;
using Microsoft.SharePoint.Client;
using Microsoft.WindowsAzure.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AzureTableSPProivider2Web.Models;
using Microsoft.WindowsAzure.Storage.Table;

namespace AzureTableSPProivider2Web.Controllers
{
    public class HomeController : Controller
    {
        [SharePointContextFilter]
        public ActionResult Index()
        {
            var storageAccount = CloudStorageAccount.Parse(
                CloudConfigurationManager.GetSetting("StorageConnectionString"));

            var client = storageAccount.CreateCloudTableClient();

            var kittehTable = client.GetTableReference("PicturesOfKittehs");
            if (!kittehTable.Exists())
            {
                kittehTable.Create();

                var thrillerKitteh = new KittehEntity("FunnyKittehs", "ThrillerKitteh");
                thrillerKitteh.ImageUrl = "http://cdn.buzznet.com/assets/users16/crizz/default/funny-pictures-thriller-kitten-impresses--large-msg-121404159787.jpg";

                var pumpkinKitteh = new KittehEntity("FunnyKittehs", "PumpkinKitteh");
                pumpkinKitteh.ImageUrl = "http://rubmint.com/wp-content/plugins/wp-o-matic/cache/6cb1b_funny-pictures-colur-blind-kitteh-finded-yew-a-pumikin.jpg";

                var batchOperation = new TableBatchOperation();

                batchOperation.Insert(thrillerKitteh);
                batchOperation.Insert(pumpkinKitteh);
                kittehTable.ExecuteBatch(batchOperation);

            }

            var kittehQuery = new TableQuery<KittehEntity>()
                .Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, "FunnyKittehs"));

            var kittehs = kittehTable.ExecuteQuery(kittehQuery).ToList();

            return View(kittehs);
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
