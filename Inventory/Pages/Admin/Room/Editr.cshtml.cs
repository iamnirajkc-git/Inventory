using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;

namespace inventory.Pages.Admin.Room
{
    public class EditrModel : PageModel
    {
        [BindProperty]
        [Required(ErrorMessage = "The Room_id is required")]
        [Display(Name = "Room_id*")]
        public string Room_id { get; set; }="";

        [BindProperty]
        [Required(ErrorMessage = "The RoomNumber is required")]
        [Display(Name = "RoomNumber*")]
        public string RoomNumber { get; set; } = "";

        [BindProperty]
        [Display(Name = "BuildingName")]
        public string? BuildingName { get; set; } = "";

        [BindProperty]
        [Display(Name = "FloorNumber")]
        public int? FloorNumber { get; set; } 

               
        public string ErrorMessage { get; set; } = "";
        public string SuccessMessage { get; set; } = "";
        public void OnGet()
        {
            string requestId = Request.Query["Room_id"];
            try
            {
                string connectionString = "Data Source=inventoryserver1.database.windows.net;Initial Catalog=Inventory;Persist Security Info=True;User ID=inventory;Password=pvamuaitc1!";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "SELECT * FROM Room where Room_id =@Room_id";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@Room_id", requestId);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                Room_id = reader.GetString(0);
                                RoomNumber = reader.GetString(1);
                                BuildingName = reader.GetString(2);
                                FloorNumber = reader.GetInt32(3);
                              

                            }
                            else
                            {
                                Response.Redirect("/Admin/Room/Indexr");

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Response.Redirect("/Admin/Room/Indexr");
            }
        }
        public void OnPost()
        {
            if (!ModelState.IsValid)
            {
                ErrorMessage = "Data validation failed";
                return;

            }

            if (BuildingName == null) BuildingName = "";
           

            // update information in database
            try
            {
                string connectionString = "Data Source=inventoryserver1.database.windows.net;Initial Catalog=Inventory;Persist Security Info=True;User ID=inventory;Password=pvamuaitc1!";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "UPDATE Room SET  RoomNumber=@RoomNumber, BuildingName=@BuildingName, " +
                       " FloorNumber= @FloorNumber   WHERE Room_id = @Room_id;";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {

                        command.Parameters.AddWithValue("@Room_id", Room_id);
                        command.Parameters.AddWithValue("@RoomNumber", RoomNumber);
                        command.Parameters.AddWithValue("@BuildingName", BuildingName);
                        command.Parameters.AddWithValue("@FloorNumber", FloorNumber);
                        
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
            Response.Redirect("/Admin/Room/Indexr");
        }

    }
}
