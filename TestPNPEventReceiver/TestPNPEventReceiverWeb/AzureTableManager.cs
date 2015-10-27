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
        //property that holds the apps Tablereference
        public static CloudTable NewsTable { get; set; }

        //Connection to Azure Table
        public static CloudStorageAccount StorageAccount = CloudStorageAccount.Parse(
                CloudConfigurationManager.GetSetting("StorageConnectionString"));
        public static CloudTableClient Client = StorageAccount.CreateCloudTableClient();


        //Create a new Azure Table for this App. workaround from having to wait to delete table with same name
        public static void SelectTable() {
            int id = 0;
            while (true) {
                var tempTable = Client.GetTableReference("NewsTable" + id.ToString()); //check this table
                if (tempTable.Exists())
                {
                    //tempTable.Delete(); //Delete tables manually for now. via serverexplorer in VS
                    id++;
                }
                else {
                    tempTable.Create();
                    NewsTable = tempTable; //use this table
                    break;
                }
            } 
        }

        //save news to azure table from inparameter listItems
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

        //Load all news from Azure Table, return stebras
        public static IEnumerable<StebraEntity> LoadAllNews() {

            //Query all entities where "PartitionKey" is "News"
            var allNewsQuery = new TableQuery<StebraEntity>()
                .Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, "News"));

            //Entities to List
            var stebras = NewsTable.ExecuteQuery(allNewsQuery).ToList();

            //Return List
            return stebras;
           
        }


    }
}