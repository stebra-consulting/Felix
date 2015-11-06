using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Shared.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TestConfigureAzTableCors.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {


            //code here
            var storageAccount = CloudStorageAccount.Parse(
                        CloudConfigurationManager.GetSetting("StorageConnectionString"));

            var client = storageAccount.CreateCloudTableClient();


            var corsRule = new CorsRule()
            {
                AllowedHeaders = new List<string> { "*" },
                AllowedMethods = CorsHttpMethods.Connect | CorsHttpMethods.Get,
                //Since we'll only be calling Query Tables, let's just allow GET verb
                AllowedOrigins = new List<string> { "*" }, //This is the URL of our application.
                ExposedHeaders = new List<string> { "*" },
                MaxAgeInSeconds = 1 * 60 * 60, //Let the browswer cache it for an hour
            };

            ServiceProperties serviceProperties = client.GetServiceProperties();
            CorsProperties corsSettings = serviceProperties.Cors;

            //add the rule
            corsSettings.CorsRules.Add(corsRule);

            //remove all rules - remember to save the settings to the client
            //corsSettings.CorsRules.Clear();

            //Save the rule
            client.SetServiceProperties(serviceProperties);


            //break here and inspect corsSettings


            //After #4, there should already be cors rule connected to an account name.
            //In order to double check what cors rules are there for that account name, we can use:
            //ServiceProperties serviceProperties = client.GetServiceProperties();
            //            CorsProperties corsSettings = serviceProperties.Cors;

            //code here
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}