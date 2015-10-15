using Microsoft.SharePoint.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StackExchange.Redis;
using FelixPHAWeb.Models;

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

                    // put this into News constructor ??
                    //list that holds each news
                    List<News> MyNews = new List<News>();
                    //make news from listitem-data
                    foreach (ListItem item in items) { 

                        MyNews.Add(
                            new News() {
                                Title = item["Title"].ToString(),
                                Article = item["Article"].ToString(),
                                Body = item["Body"].ToString()}
                            );
                    }
                    // put this into News constructor ??

                    //save cache
                    NewsContext newsContext = new NewsContext();
                    newsContext.saveCache(MyNews);

                    ViewBag.News = MyNews;
                }
            }

            return View();
        }

        public ActionResult About()
        {

            //read cache
            List<News> MyNews = new List<News>();
            NewsContext newsContext = new NewsContext();
            MyNews = newsContext.readCache();

            ViewBag.NewsFromCache = MyNews;
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }


    }
}
