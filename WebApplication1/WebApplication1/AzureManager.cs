﻿using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication1.Models;
using Microsoft.WindowsAzure.Storage.Blob;

namespace WebApplication1
{
    public class AzureManager
    {
        //Config
        const string containerName = "photos";
        public const string tableName = "stebraNyhetslist";

        //Connection to Azure Storage
        private static CloudStorageAccount StorageAccount = CloudStorageAccount.Parse(
                CloudConfigurationManager.GetSetting("StorageConnectionString"));


        //BLOB SPECIFIC=========================================================================
        //get Blob -client
        public static CloudBlobClient blobClient = StorageAccount.CreateCloudBlobClient();

        //get Container
        public static CloudBlobContainer container = blobClient.GetContainerReference(containerName);

        public static string ExportImage(System.IO.Stream stream, string blobName)
        {

            CloudBlockBlob blockBlob = container.GetBlockBlobReference(blobName);
            blockBlob.UploadFromStream(stream);

            string url = blockBlob.StorageUri.PrimaryUri.AbsoluteUri;

            return url;

        }

        //TABLE SPECIFIC=========================================================================
        //get Table -client  
        public static CloudTableClient tableClient = StorageAccount.CreateCloudTableClient();

        //public static void CreateTable(List<ListItem> stebraList)
        //{
        //    CloudTable table = SelectTable();//make sure table is a clean slate

        //    var batchOperation = new TableBatchOperation(); //make only one call to Azure Table, use Batch.
        //    foreach (ListItem item in stebraList)
        //    {
        //        //Convert ListItems to Table-entries(Entity)
        //        var entity = new StebraEntity(
        //            "Nyhet",                    //string Stebratype this is partitionKey
        //            item["Title"].ToString(),   //string newsEntry this will be used as rowKey
        //            "Descriptive text",         //string NewsDescription
        //            item["Article"].ToString(), //string NewsArticle
        //            item["Datum"].ToString(),   //string NewsDate
        //            item["Body"].ToString()     //string NewsBody
        //            );

        //        batchOperation.Insert(entity); //Config Batch
        //    }
        //    table.ExecuteBatch(batchOperation); //make only one call to Azure Table, use Batch. this is that one call.
        //}

        public static CloudTable SelectTable()
        {
            CloudTable tempTable = null;
            //int id = 0;

            //delete bord tills du hittar en tom slot
            int emptyId = 0;

            for (int id = 0; id < 10; id++)
            {
                tempTable = tableClient.GetTableReference(tableName + id.ToString()); //fetch table

                if (!tempTable.Exists())
                {
                    emptyId = id;
                    break;
                }
                else
                {
                    tempTable.Delete();
                }
            }

            //skapa ett bord på det tomma slottet
            tempTable.Create();

            //returnera det bordet
            return tempTable;

        }

    }

}

//while (id <= 2)
//{
//    tempTable = tableClient.GetTableReference(tableName + id.ToString()); //check this table

//    if (!tempTable.Exists())
//    {
//        tempTable.Create();

//        break;
//    }
//    else if (tempTable.Exists())
//    {
//        tempTable.DeleteIfExists(); //delete busy table

//    }
//    id++;
//}
//return tempTable;

//        public static CloudTable SelectValidTable()
//{
//    CloudTable tempTable = null;

//    for (int id = 0; id< 2; id++)
//    {
//        tempTable = tableClient.GetTableReference(tableName + id.ToString());
//        if (tempTable.Exists()) break;
//    }
//    //check this tables

//    return tempTable;
//}