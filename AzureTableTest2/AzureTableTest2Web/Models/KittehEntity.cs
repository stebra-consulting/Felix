using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AzureTableTest2Web.Models
{
    public class KittehEntity : TableEntity
    {
        public KittehEntity(string kittehType, string kittehName)
        {
            this.PartitionKey = kittehType;
            this.RowKey = kittehName;
        }

        public KittehEntity() { }

        public string ImageUrl { get; set; }
    }
}