
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Table;

namespace AzureTableTestWeb.Models
{
    public class StebraEntity : TableEntity
    {
        public StebraEntity(string StebraType, string StebraName)
        {
            this.PartitionKey = StebraType;
            this.RowKey = StebraName;
        }

        public StebraEntity() { }

        public string ImageUrl { get; set; }

    }
}