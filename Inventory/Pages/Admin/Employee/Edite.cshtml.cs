using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;

namespace inventory.Pages.Admin.Employee
{
    public class EditeModel : PageModel
    {
        [BindProperty]
        [Required(ErrorMessage = "The EmployeeID is required")]
        [Display(Name = "EmployeeID*")]
        public int EmployeeID { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "The FirstName is required")]
        [Display(Name = "FirstName*")]
        public string FirstName { get; set; } = "";

        [BindProperty, Required]
        [Display(Name = "LastName")]
        public string LastName { get; set; } = "";

        [BindProperty]
        [Display(Name = "DepartmentID")]
        public int? DepartmentID { get; set; } 

        [BindProperty]
        [Display(Name = "EMPType")]
        public string? EMPType { get; set; } = "";

        [BindProperty]
        [Display(Name = "Room_id")]
        public string? Room_id { get; set; } = "";


        [BindProperty]
        [Display(Name = "Email")]
        public string? Email { get; set; } = "";

        [BindProperty]
        [Display(Name = "PhoneNumber")]
        public string? PhoneNumber { get; set; } = "";
       
        public string ErrorMessage { get; set; } = "";
        public string SuccessMessage { get; set; } = "";
        public void OnGet()
        {
            string requestId = Request.Query["EmployeeID"];
            try
            {
                string connectionString = "Data Source=inventoryserver1.database.windows.net;Initial Catalog=Inventory;Persist Security Info=True;User ID=inventory;Password=pvamuaitc1!";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "SELECT * FROM Employee where EmployeeID =@EmployeeID";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@EmployeeID", requestId);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                EmployeeID = reader.GetInt32(0);
                                FirstName = reader.GetString(1);
                                LastName = reader.GetString(2);
                                DepartmentID = reader.GetInt32(3);
                                EMPType = reader.GetString(4);
                                Room_id = reader.GetString(5);
                                Email = reader.GetString(6);
                                PhoneNumber = reader.GetString(7);

                            }
                            else
                            {
                                Response.Redirect("/Admin/Employee/Indexe");

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Response.Redirect("/Admin/Employee/Indexe");
            }
        }
        public void OnPost()
        {
            if (!ModelState.IsValid)
            {
                ErrorMessage = "Data validation failed";
                return;

            }

            
            if (EMPType == null) EMPType = "";
            if (Room_id == null) Room_id = "";
            if (Email == null) Email = "";
            if (PhoneNumber == null) PhoneNumber = "";

            // update information in database
            try
            {
                string connectionString = "Data Source=inventoryserver1.database.windows.net;Initial Catalog=Inventory;Persist Security Info=True;User ID=inventory;Password=pvamuaitc1!";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "UPDATE Employee SET " +
                       " FirstName= @FirstName, LastName=@LastName, DepartmentID=@DepartmentID, " +
                       " EMPType=@EMPType, Room_id= @Room_id, Email=@Email, PhoneNumber=@PhoneNumber WHERE EmployeeID = @EmployeeID;";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {

                        command.Parameters.AddWithValue("@EmployeeID", EmployeeID);
                        command.Parameters.AddWithValue("@FirstName", FirstName);
                        command.Parameters.AddWithValue("@LastName", LastName);
                        command.Parameters.AddWithValue("@DepartmentID", DepartmentID);
                        command.Parameters.AddWithValue("@EMPType", EMPType);
                        command.Parameters.AddWithValue("@Room_id", Room_id);
                        command.Parameters.AddWithValue("@Email", Email);
                        command.Parameters.AddWithValue("@PhoneNumber", PhoneNumber);
    
                        command.ExecuteNonQuery();

                    }


                }
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                return;
            }

            SuccessMessage = "Data saved correctly";
            Response.Redirect("/Admin/Employee/Indexe");
        }
    }
}
