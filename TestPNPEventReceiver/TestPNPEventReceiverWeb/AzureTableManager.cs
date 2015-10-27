using Microsoft.Azure;
using Microsoft.SharePoint.Client;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TestPNPEventReceiverWeb.Models;

namespace TestPNPEventReceiverWeb
{
    public class AzureTableManager
    {
        public static CloudTable NewsTable { get; set; }
        public static CloudStorageAccount StorageAccount = CloudStorageAccount.Parse(
                CloudConfigurationManager.GetSetting("StorageConnectionString"));

        public static CloudTableClient Client = StorageAccount.CreateCloudTableClient();


        public static void SelectTable() {
            int id = 0;
            while (true) {
                var tempTable = Client.GetTableReference("NewsTable" + id.ToString());
                if (tempTable.Exists())
                {
                    //tempTable.Delete(); //Delete tables manually for now.
                    id++;
                }
                else {
                    tempTable.Create();
                    NewsTable = tempTable;
                    break;
                }
            } 
        }

        public static void SaveNews(ListItemCollection listItems) {

            var batchOperation = new TableBatchOperation(); //make only one call to Azure Table, use Batch.
            foreach (ListItem item in listItems)
            {
                //Convert ListItems to Table-entries(Entity)
                var entity = new StebraEntity(
                    "News",                     //string Stebratype
                    item["Title"].ToString(),   //string newsEntry
                    "Descriptive text",         //string NewsDescription
                    item["Article"].ToString(), //string NewsArticle
                    item["Datum"].ToString(),   //string NewsDate
                    item["Body"].ToString()     //string NewsBody
                    );

                batchOperation.Insert(entity); //Batch this
            }
            NewsTable.ExecuteBatch(batchOperation); //Execute Batch
        }

        public static IEnumerable<StebraEntity> LoadAllNews() {

            
            var allNewsQuery = new TableQuery<StebraEntity>()
                .Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, "News"));

            var stebras = NewsTable.ExecuteQuery(allNewsQuery).ToList();
            return stebras;
           
        }


    }
}