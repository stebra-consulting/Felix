using Microsoft.SharePoint.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using TestPNPEventReceiverWeb.Models;
using Microsoft.WindowsAzure.Storage.Table;

namespace TestPNPEventReceiverWeb.Controllers
{
    public class HomeController : Controller
    {
        [SharePointContextFilter]
        public ActionResult Index()
        {

            var storageAccount = CloudStorageAccount.Parse(
                CloudConfigurationManager.GetSetting("StorageConnectionString"));

            var client = storageAccount.CreateCloudTableClient();

            var newsTable = client.GetTableReference("NewsTable");

            if (!newsTable.Exists())
            {
                newsTable.Create();
            }


            var spContext = SharePointContextProvider.Current.GetSharePointContext(HttpContext);
            using (var clientContext = spContext.CreateUserClientContextForSPHost())
            {
                if (clientContext != null)
                {
                    //get listdata from sharepoint
                    List spNewsList = clientContext.Web.Lists.GetByTitle("Nyhetslista");
                    clientContext.Load(spNewsList);
                    clientContext.ExecuteQuery();

                    //get all listitems that has a title-column
                    CamlQuery camlQuery = new CamlQuery();
                    camlQuery.ViewXml = @"
                                        <View>
                                            <Query>
                                                <Where>
                                                    <IsNotNull>
                                                        <FieldRef Name='Title' />
                                                    </IsNotNull>
                                                </Where>
                                            </Query>
                                        </View>";
                    ListItemCollection items = spNewsList.GetItems(camlQuery);

                    clientContext.Load(items);
                    clientContext.ExecuteQuery();

                    //Convert and simplify SharePoint ListItemCollection
                    var batchOperation = new TableBatchOperation(); //move outside loop //
                    foreach (ListItem item in items)
                    {

                        var entity = new StebraEntity(
                            "News", 
                            item["Title"].ToString(),
                            item["Article"].ToString(), 
                            "Descriptive text", 
                            item["Datum"].ToString(),
                            item["Body"].ToString());
                        
                        batchOperation.Insert(entity);
                    }
                    newsTable.ExecuteBatch(batchOperation); //move outside loop //

                    return View();
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
