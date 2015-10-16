using Microsoft.SharePoint.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FelixPHAWeb.Models;

namespace FelixPHAWeb.Controllers
{
    public class NewsController : Controller
    {
        [SharePointContextFilter]
        public ActionResult Save()
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

                    //Convert and simplify SharePoint ListItemCollection to List<News>
                    List<News> allNews = new List<News>();
                    foreach (ListItem item in items)
                    {
                        allNews.Add(
                            new News()
                            {
                                Title = item["Title"].ToString(),
                                Article = item["Article"].ToString(),
                                Body = item["Body"].ToString()
                            });
                    }

                    //save cache
                    NewsContext newsContext = new NewsContext();
                    newsContext.saveCache(allNews);

                    return View(allNews);
                }
            }

            return View();
        }
        public ActionResult Load()
        {

            //read cache
            List<News> MyNews = new List<News>();
            NewsContext newsContext = new NewsContext();
            MyNews = newsContext.readCache();

            return View(MyNews);
        }
    }
}