using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using static inventory.Pages.Admin.Inventory.IndexModel;

namespace inventory.Pages.Admin.Department
{
    public class IndexdModel : PageModel
    {
        public List<DepartmentInfo> ListDepartment = new List<DepartmentInfo>(); // we are creating list so that it can store  all records of inventory table
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
                    string sql = "SELECT * FROM Department ";
                    if (search.Length > 0)
                    {
                        sql += " WHERE DepartmentID LIKE @search OR DepartmentName LIKE @search ";
                    }
                    sql += " ORDER BY DepartmentID DESC";

                    using (SqlCommand command = new SqlCommand(sql, connection)) // creating sql command to exceute sql query
                    {
                        command.Parameters.AddWithValue("@search", "%" + search + "%"); // replace @search with search parameter either at beginiing or at the end

                        using (SqlDataReader reader = command.ExecuteReader()) // creating sql datareader to read the results
                        {
                            while (reader.Read()) // using while loop to read data of one row
                            {
                                DepartmentInfo inventoryInfo = new DepartmentInfo(); //creating object of type InventoryInfo
                                inventoryInfo.DepartmentID = reader.GetInt32(0); // one will will fill this object 
                                inventoryInfo.DepartmentName = reader.GetString(1);

                                ListDepartment.Add(inventoryInfo);   // adding object in the list, now we will go to page(html to display the list)

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

        public class DepartmentInfo 
        {
            public int DepartmentID { get; set; }
            public string DepartmentName { get; set; } = "";
        }
      
    }
}
