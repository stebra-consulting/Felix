'use strict';

ExecuteOrDelayUntilScriptLoaded(initializePage, "sp.js");

function initializePage()
{
    var context = SP.ClientContext.get_current();
    var user = context.get_web().get_currentUser();

    //initializePage()-scope variables
    var spItemId;
    var spListId;

    // This code runs when the DOM is ready and creates a context object which is needed to use the SharePoint object model
    $(document).ready(function () {
        spItemId = GetURLParameter('SPListItemId');

        spListId = GetURLParameter('SPListId');

        $("#info").html(
            "spItem is" + spItemId + "</br>"
          + "spListId is" + spListId + "</br>");

        getLists();

        //exert from ribbon elemets.xml:
        //Default.aspx?{StandardTokens}&amp;SPListItemId={SelectedItemId}&amp;SPListId={SelectedListId}
        //which means that 'SPListItemId' and 'SPListId' are URL-parameters prepared by our Ribbon-button
        //
        //

    });

    function getLists() {

        var lists = context.get_web().get_lists();
        context.load(lists);
        context.executeQueryAsync(onGetListsSuccess(lists), onFail);
    }

    function onGetListsSuccess(lists) {
        $("#listtitle").html("Lists loaded");

        //alert(" line 44");

        // errors because i input string instead of SP.Guid see:
        //https://msdn.microsoft.com/EN-US/library/office/jj247185.aspx
        var list = lists.getById(spListId);

        alert("list is " + list);

        context.load(list);

        context.executeQueryAsync(onGetTheListSuccess, onFail);

    }

    function onGetTheListSuccess() {
        $("#listtitle").html(list.get_title());
    }

    // This function is executed if a execute.context call fails
    function onFail(sender, args) {
        alert('Failed to get user name. Error:' + args.get_message());
    }
}

function GetURLParameter(sParam) { //var tech = GetURLParameter('technology');


    var sPageURL = window.location.search.substring(1);
    var sURLVariables = sPageURL.split('&');
    for (var i = 0; i < sURLVariables.length; i++) {
        var sParameterName = sURLVariables[i].split('=');
        if (sParameterName[0] == sParam) {
            return sParameterName[1];
        }
    }
}