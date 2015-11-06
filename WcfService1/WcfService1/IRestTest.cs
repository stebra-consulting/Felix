using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace WcfService1
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IRestTest" in both code and config file together.
    [ServiceContract]
    public interface IRestTest
    {
        [OperationContract]
        [WebInvoke(Method = "GET",
            ResponseFormat = WebMessageFormat.Xml,
            BodyStyle = WebMessageBodyStyle.Wrapped,
            UriTemplate = "xml/{id}")]
        string XMLData(string id);

        [OperationContract]
        [WebInvoke(Method = "GET",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped,
            UriTemplate = "json/{id}")]
        string JSONData(string id);

    }
}
