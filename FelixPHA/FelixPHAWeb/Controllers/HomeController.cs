using Microsoft.SharePoint.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StackExchange.Redis;
using FelixPHAWeb.Models;
using System.Xml;

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

                    //list that holds each news
                    List<News> MyNews = new List<News>();

                    //make news from listitem-data
                    foreach (ListItem item in items) { 
                   
                        News currentNews = new News();
                        currentNews.Title = item["Title"].ToString();
                        currentNews.Article = item["Article"].ToString();
                        currentNews.Body = item["Body"].ToString();

                        MyNews.Add(currentNews);
                    }

                    ViewBag.News = MyNews;

                    //
                    //  Set Cache Block
                    //
                    ConnectionMultiplexer connection = ConnectionMultiplexer.Connect(
                        "memory.redis.cache.windows.net,abortConnect=false,ssl=true,password=nv2w3Oz+20mPCR2vVQVLCwUc3PAYpokJy02LK0JcEHQ=");

                    IDatabase cache = connection.GetDatabase();

                    //set one cacheKey for every news in newslist
                    for (int i = 0; i < MyNews.Count; i++)
                    {
                        string index = i.ToString();
                        string content = MyNews[i].Title;
                        content += "," + MyNews[i].Article;
                        content += "," + MyNews[i].Body;
                        cache.StringSet(index, content, TimeSpan.FromMinutes(60)); //set cache
                    }
                }
            }

            return View();
        }

        public ActionResult About()
        {

            ConnectionMultiplexer connection = ConnectionMultiplexer.Connect(
                "memory.redis.cache.windows.net,abortConnect=false,ssl=true,password=nv2w3Oz+20mPCR2vVQVLCwUc3PAYpokJy02LK0JcEHQ=");

            IDatabase cache = connection.GetDatabase();

            //// Simple get of data types from the cache

            //make the code more readably
            int title = 0;
            int article = 1;
            int body = 2;

            //make list of news from cache
            List<News> MyNews = new List<News>();
            int key = 0;
            while (true) { //While there is a valid cache, populate list of news
                string cachedContent = cache.StringGet(key.ToString());
                if (cachedContent != null)
                {
                    News currentNews = new News();
                    currentNews.Title = cachedContent.Split(',')[title];
                    currentNews.Article = cachedContent.Split(',')[article];
                    currentNews.Body = cachedContent.Split(',')[body];
                    MyNews.Add(currentNews);
                }
                else {
                    break;
                }
                key++;
            }

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
