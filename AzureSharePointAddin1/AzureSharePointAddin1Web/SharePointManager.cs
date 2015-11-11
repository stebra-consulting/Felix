using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AzureSharePointAddin1Web
{
    public class SharePointManager
    {
        public static SPlistItemgetListItemsFromListName() {
            return SPListItemCollection;
        }
        ////get listdata from sharepoint
        //List spNewsList = clientContext.Web.Lists.GetByTitle("Nyhetslista");
        //clientContext.Load(spNewsList);
        //clientContext.ExecuteQuery();

        ////get all listitems that has a title-column
        //CamlQuery camlQuery = new CamlQuery();
        //camlQuery.ViewXml = @"
        //                    <View>
        //                        <Query>
        //                            <Where>
        //                                <IsNotNull>
        //                                    <FieldRef Name='Title' />
        //                                </IsNotNull>
        //                            </Where>
        //                        </Query>
        //                    </View>";
        //ListItemCollection items = spNewsList.GetItems(camlQuery);

        //clientContext.Load(items);
        //clientContext.ExecuteQuery();
    }
}