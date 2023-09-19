using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using static inventory.Pages.Admin.Inventory.IndexModel;

namespace inventory.Pages.Admin.Asset
{
    public class IndexaModel : PageModel
    {
        public List<AssetInfo> ListAsset = new List<AssetInfo>(); // we are creating list so that it can store  all records of inventory table
        public string search = "";
        public void OnGet()
        {
            search = Request.Query["search"];
            if (search == null) search = "";
            try
            {
                string connectionString = "Data Source=inventoryserver1.database.windows.net;Initial Catalog=Inventory;Persist Security Info=True;User ID=inventory;Password=pvamuaitc1!"; // declare variable for connection
                using (SqlConnection connection = new SqlConnection(connectionString)) // now make connection to database
                {
                    connection.Open(); // open the connection 
                    string sql = "SELECT * FROM Asset ";
                    if (search.Length > 0)
                    {
                        sql += " WHERE AssetTag LIKE @search OR AssetType LIKE @search OR AssetSerialNumber LIKE @search ";
                    }
                    sql += " ORDER BY AssetTag DESC";

                    using (SqlCommand command = new SqlCommand(sql, connection)) // creating sql command to exceute sql query
                    {
                        command.Parameters.AddWithValue("@search", "%" + search + "%"); // replace @search with search parameter either at beginiing or at the end

                        using (SqlDataReader reader = command.ExecuteReader()) // creating sql datareader to read the results
                        {
                            while (reader.Read()) // using while loop to read data of one row
                            {
                                AssetInfo assetInfo = new AssetInfo(); //creating object of type InventoryInfo
                                assetInfo.AssetTag = reader.GetString(0);
                                assetInfo.AssetName = reader.GetString(1); // one will will fill this object 
                                assetInfo.AssetType = reader.GetString(2);
                                assetInfo.AssetSerialNumber = reader.GetString(3);
                                assetInfo.Manufacturer = reader.GetString(4);
                                
                                ListAsset.Add(assetInfo);   // adding object in the list, now we will go to page(html to display the list)

                            }
                        }

                    }



                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }
        public class AssetInfo //we have to create this class so that it will have only one records from the inventory table, to holds all the records we have to create list, which we have created at the top 
        {
            public string AssetTag { get; set; } = "";
            public string AssetName { get; set; } = "";
            public string AssetType { get; set; } = "";
            public string AssetSerialNumber { get; set; } = "";
            public string Manufacturer { get; set; } = "";
           
        }
    }
}
