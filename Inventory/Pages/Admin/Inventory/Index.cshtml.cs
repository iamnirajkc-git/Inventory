using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.Design;
using System.Data.SqlClient;

namespace inventory.Pages.Admin.Inventory
{
    public class IndexModel : PageModel
    {
        public List<InventoryInfo> ListInventory = new List<InventoryInfo>(); // we are creating list so that it can store  all records of inventory table
        public string search = "";

        public void OnGet() // inside onget method we will get data from the database
        {
            search = Request.Query["search"];
            if (search == null) search = ""; 
            try
            {
                string connectionString = "Data Source=inventoryserver1.database.windows.net;Initial Catalog=Inventory;Persist Security Info=True;User ID=inventory;Password=pvamuaitc1!"; // declare variable for connection
                using (SqlConnection connection = new SqlConnection(connectionString)) // now make connection to database
                {
                    connection.Open(); // open the connection 
                    string sql = "SELECT * FROM Inventory ";
                    if (search.Length > 0)
                    {
                        sql += " WHERE AssetTag LIKE @search OR CurrentOwner LIKE @search ";
                    }
                    sql += " ORDER BY InventoryID DESC";
                        
                    using (SqlCommand command = new SqlCommand(sql, connection)) // creating sql command to exceute sql query
                    {
                        command.Parameters.AddWithValue("@search", "%" + search + "%"); // replace @search with search parameter either at beginiing or at the end

                        using (SqlDataReader reader = command.ExecuteReader()) // creating sql datareader to read the results
                        {
                            while (reader.Read()) // using while loop to read data of one row
                            {
                                InventoryInfo inventoryInfo = new InventoryInfo(); //creating object of type InventoryInfo
                                inventoryInfo.InventoryID = reader.GetInt32(0); // one will will fill this object 
                                inventoryInfo.assetname = reader.GetString(1); 
                                inventoryInfo.IssuedDepartment = reader.GetString(2);
                                inventoryInfo.AssetTag = reader.GetString(3);
                                inventoryInfo.CurrentOwner = reader.GetString(4);
                                inventoryInfo.PreviousOwner = reader.GetString(5);
                                inventoryInfo.DateIssued  = reader.GetDateTime(6).ToString("MM/dd/yyyy");
                                inventoryInfo.Issued_ID = reader.GetInt32(7);
                                inventoryInfo.Status = reader.GetString(8);
                                inventoryInfo.Notes = reader.GetString(9);
                                inventoryInfo.IssuedRoom = reader.GetString(10);

                                ListInventory.Add(inventoryInfo);   // adding object in the list, now we will go to page(html to display the list)

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
        public class InventoryInfo //we have to create this class so that it will have only one records from the inventory table, to holds all the records we have to create list, which we have created at the top 
        {
            public int InventoryID { get; set; } 
            public string assetname { get; set; } = "";
            public string IssuedDepartment { get; set; } = "";
            public string AssetTag { get; set; } = "";
            public string CurrentOwner { get; set; } = "";
            public string PreviousOwner { get; set; } = ""; 
            public string DateIssued { get; set; } = "";
            public int Issued_ID { get; set; } 
            public string Status { get; set; } = "";
            public string Notes { get; set; } = "";
            public string IssuedRoom { get; set; } = "";


        }

    }
}
