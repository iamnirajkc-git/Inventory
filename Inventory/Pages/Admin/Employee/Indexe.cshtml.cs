using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using static inventory.Pages.Admin.Employee.IndexeModel;

namespace inventory.Pages.Admin.Employee
{
    public class IndexeModel : PageModel
    {
        public List<EmployeeInfo> ListEmployee = new List<EmployeeInfo>(); // we are creating list so that it can store  all records of inventory table
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
                    string sql = "SELECT * FROM Employee ";
                    if (search.Length > 0)
                    {
                        sql += " WHERE EmployeeID LIKE @search OR FirstName LIKE @search ";
                    }
                    sql += " ORDER BY EmployeeID DESC";

                    using (SqlCommand command = new SqlCommand(sql, connection)) // creating sql command to exceute sql query
                    {
                        command.Parameters.AddWithValue("@search", "%" + search + "%"); // replace @search with search parameter either at beginiing or at the end

                        using (SqlDataReader reader = command.ExecuteReader()) // creating sql datareader to read the results
                        {
                            while (reader.Read()) // using while loop to read data of one row
                            {
                                EmployeeInfo employeeInfo = new EmployeeInfo(); //creating object of type InventoryInfo

                                employeeInfo.EmployeeID = reader.GetInt32(0); // one will will fill this object 
                                employeeInfo.FirstName = reader.GetString(1);
                                employeeInfo.LastName = reader.GetString(2);
                                employeeInfo.DepartmentID = reader.GetInt32(3);
                                employeeInfo.EMPType = reader.GetString(4);
                                employeeInfo.Room_id = reader.GetString(5);
                                employeeInfo.Email = reader.GetString(6);
                                employeeInfo.PhoneNumber = reader.GetString(7);
                              
                                ListEmployee.Add(employeeInfo);   // adding object in the list, now we will go to page(html to display the list)

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
        public class EmployeeInfo //we have to create this class so that it will have only one records from the inventory table, to holds all the records we have to create list, which we have created at the top 
        {
            public int EmployeeID { get; set; }
            public string FirstName { get; set; } = "";
            public string LastName { get; set; } = "";
            public int DepartmentID { get; set; } 
            public string EMPType { get; set; } = "";
            public string Room_id { get; set; } = "";
            public string Email { get; set; } = "";
            public string PhoneNumber { get; set; } = "";
            
        }
    }
}
