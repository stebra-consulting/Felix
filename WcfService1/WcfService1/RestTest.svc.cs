﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace WcfService1
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "RestTest" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select RestTest.svc or RestTest.svc.cs at the Solution Explorer and start debugging.
    public class RestTest : IRestTest
    {
        #region IRestTest Members
        public string XMLData(string id)
        {
            return "You requested product " + id;
        }
        public string JSONData(string id)
        {
            return "You requested product " + id;
        }
        #endregion
    }
}


