using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace inventory.Pages.Admin.Room
{
    public class IndexrModel : PageModel
    {
        public List<RoomInfo> ListRoom = new List<RoomInfo>(); // we are creating list so that it can store  all records of inventory table
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
                    string sql = "SELECT * FROM Room ";
                    if (search.Length > 0)
                    {
                        sql += " WHERE Room_id LIKE @search OR BuildingName LIKE @search ";
                    }
                    sql += " ORDER BY Room_id DESC";

                    using (SqlCommand command = new SqlCommand(sql, connection)) // creating sql command to exceute sql query
                    {
                        command.Parameters.AddWithValue("@search", "%" + search + "%"); // replace @search with search parameter either at beginiing or at the end

                        using (SqlDataReader reader = command.ExecuteReader()) // creating sql datareader to read the results
                        {
                            while (reader.Read()) // using while loop to read data of one row
                            {
                                RoomInfo roomInfo = new RoomInfo(); //creating object of type InventoryInfo
                                roomInfo.Room_id = reader.GetString(0); // one will will fill this object 
                                roomInfo.RoomNumber = reader.GetString(1);
                                roomInfo.BuildingName = reader.GetString(2);
                                roomInfo.FloorNumber = reader.GetInt32(3);
                                

                                ListRoom.Add(roomInfo);   // adding object in the list, now we will go to page(html to display the list)

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
        public class RoomInfo //we have to create this class so that it will have only one records from the inventory table, to holds all the records we have to create list, which we have created at the top 
        {
            public string Room_id { get; set; } = "";
            public string RoomNumber { get; set; } = "";
            public string BuildingName { get; set; } = "";
            public int FloorNumber { get; set; } 
           
        }
    }
}
