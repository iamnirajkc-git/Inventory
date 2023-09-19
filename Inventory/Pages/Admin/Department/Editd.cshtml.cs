using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;

namespace inventory.Pages.Admin.Department
{
    public class EditdModel : PageModel
    {
        [BindProperty]
        [Required(ErrorMessage = "The DepartmentID is required")]
        [Display(Name = "DepartmentID*")]
        public int DepartmentID { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "The DepartmentName is required")]
        [Display(Name = "DepartmentNamee*")]
        public string DepartmentName { get; set; } = "";
      
        public string ErrorMessage { get; set; } = "";
        public string SuccessMessage { get; set; } = "";
        public void OnGet()
        {
            string requestId = Request.Query["DepartmentID"];
            try
            {
                string connectionString = "Data Source=inventoryserver1.database.windows.net;Initial Catalog=Inventory;Persist Security Info=True;User ID=inventory;Password=pvamuaitc1!";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "SELECT * FROM Department where DepartmentID = @DepartmentID";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@DepartmentID", requestId);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                DepartmentID = reader.GetInt32(0);
                                DepartmentName = reader.GetString(1);
                               


                            }
                            else
                            {
                                Response.Redirect("/Admin/Department/Indexd");

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Response.Redirect("/Admin/Department/Indexd");
            }
        }
        public void OnPost()
        {
            if (!ModelState.IsValid)
            {
                ErrorMessage = "Data validation failed";
                return;

            }

           


            // update information in database
            try
            {
                string connectionString = "Data Source=inventoryserver1.database.windows.net;Initial Catalog=Inventory;Persist Security Info=True;User ID=inventory;Password=pvamuaitc1!";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "UPDATE Department SET   DepartmentName=@DepartmentName where DepartmentID=@DepartmentID;";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {

                        command.Parameters.AddWithValue("@DepartmentID", DepartmentID);
                        command.Parameters.AddWithValue("@DepartmentName", DepartmentName);
                        



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
            Response.Redirect("/Admin/Department/Indexd");
        }
    }
}
