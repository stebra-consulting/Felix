'use strict';

ExecuteOrDelayUntilScriptLoaded(initializePage, "sp.js");

function initializePage()
{
    //var context = SP.ClientContext.get_current();
    //var user = context.get_web().get_currentUser();

    // This code runs when the DOM is ready and creates a context object which is needed to use the SharePoint object model
    $(document).ready(function () {
        //getUserName();
        
        var xhr = new XMLHttpRequest();

        //https://<account-name>.table.core.windows.net/?restype=service&comp=properties

        var urlPath = "https://stebratables.table.core.windows.net/NewsTable4(PartitionKey='News',RowKey='Felix the tjelix')?$select=Body";

        //var requestUrl = "https://stebratables.table.core.windows.net/?restype=service&comp=properties";

        
        var now = new Date;
        var dateInUTC = Date.UTC(now.getFullYear(), now.getMonth(), now.getDate(),
              now.getHours(), now.getMinutes(), now.getSeconds(), now.getMilliseconds());

        //alert("utc_timestamp " + dateInUTC);

        //var signature = Base64(HMAC - SHA256(UTF8(StringToSign)))

        xhr.open("GET", urlPath, false);

        var accountName = "stebratables";

        //this is not correct
        //var signature = "BuOnRuOPlfVXkLxl2qtp/hUJPGzjl8zdaqqg1mqXGZ8SJN9gTN38ChZXVJaNqsBMh+JXo4KfyMAFeDSnhBROxg==";

        var stringToSign = VERB + "\n" +
                           Content-MD5 + "\n" +
                           Content-Type + "\n" +
                           Date + "\n" +
                           CanonicalizedResource;

        alert("preparing ajax");

        $.ajax({
            url: urlPath,
            type: 'GET',
            success: function (data) {
                //do something to data
                $("#message").html("SUCCESS!");
            },
            beforeSend: function (xhr) {
                xhr.setRequestHeader('Authorization', "SharedKey " + accountName + ":" + signature);
                xhr.setRequestHeader('x-ms-date', dateInUTC);
                xhr.setRequestHeader('x-ms-version', '2014-02-14');
                xhr.setRequestHeader('Accept', 'application/json;odata=nometadata');
                xhr.setRequestHeader('DataServiceVersion', '3.0;NetFx');
                xhr.setRequestHeader('MaxDataServiceVersion', '3.0;NetFx');
            },
            error: function (rcvData) {
                console.log(rcvData);
            }
        });

        alert("will now send xhr");
        xhr.send();

        $("#message").html(xhr.status + "</br>" + xhr.statusText);

    });

    //// This function prepares, loads, and then executes a SharePoint query to get the current users information
    //function getUserName() {
    //    context.load(user);
    //    context.executeQueryAsync(onGetUserNameSuccess, onGetUserNameFail);
    //}

    //// This function is executed if the above call is successful
    //// It replaces the contents of the 'message' element with the user name
    //function onGetUserNameSuccess() {
    //    $('#message').text('Hello ' + user.get_title());
    //}

    //// This function is executed if the above call fails
    //function onGetUserNameFail(sender, args) {
    //    alert('Failed to get user name. Error:' + args.get_message());
    //}
}
