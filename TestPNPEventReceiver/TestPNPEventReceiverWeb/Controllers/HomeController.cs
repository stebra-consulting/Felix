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

                    //Create a new Azure Table for this app
                    AzureTableManager.SelectTable();
                        
                    //Save the ListItems to AzureTable as StebraEntities
                    AzureTableManager.SaveNews(items);

                    return View();
                }
            }

            return View();
        }

        public ActionResult About()
        {


            IEnumerable<StebraEntity> news = AzureTableManager.LoadAllNews();

            return View(news);
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
