using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using StackExchange.Redis;

namespace FelixPHAWeb.Models
{
    public class NewsContext
    {
        public List<News> ListOfNews { get; set; }
        private IDatabase Cache { get; set; }

        public NewsContext() //constructor
        {
            //establish connection redisCache
            ConnectionMultiplexer connection = ConnectionMultiplexer.Connect(
                "memory.redis.cache.windows.net,abortConnect=false,ssl=true,password=nv2w3Oz+20mPCR2vVQVLCwUc3PAYpokJy02LK0JcEHQ=");

            this.Cache = connection.GetDatabase();
        }

        public void saveCache(List<News> listOfNews)
        {
            //set one cacheKey for every news in newslist
            for (int i = 0; i < listOfNews.Count; i++)
            {
                string index = i.ToString();
                string content = listOfNews[i].Title;
                content += "," + listOfNews[i].Article;
                content += "," + listOfNews[i].Body;
                this.Cache.StringSet(index, content, TimeSpan.FromMinutes(60)); //save to cache
            }
        }

        public List<News> readCache()
        {
            List<News> ListOfNews = new List<News>();

            int key = 0;//which cachekey to read
            while (true) {
                string cachedContent = this.Cache.StringGet(key.ToString()); //read from cache
                if (cachedContent != null)  //valid cache, populate list of news
                {
                    News currentNews = new News();
                    currentNews.Title = cachedContent.Split(',')[0];    //title
                    currentNews.Article = cachedContent.Split(',')[1];  //article
                    currentNews.Body = cachedContent.Split(',')[2];     //body
                    ListOfNews.Add(currentNews);

                }
                else //invalid cache, cancel loop
                {
                    break;
                }
                key++;
            }
            return ListOfNews;
        }

     }

}