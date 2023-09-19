using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;

namespace inventory.Pages.Admin.Room
{
    public class CreaterModel : PageModel
    {
        public void OnGet()
        {
        }
        [BindProperty]
        [Required(ErrorMessage = "The Room_id is required")]
        [Display(Name = "Room_id*")]
        public string Room_id { get; set; } = "";

        [BindProperty]
        [Required(ErrorMessage = "The RoomNumber is required")]
        [Display(Name = "RoomNumber*")]
        public string RoomNumber { get; set; } = "";

        [BindProperty]
        [Display(Name = "BuildingName*")]
        public string? BuildingName { get; set; } = "";

        [BindProperty]
        [Display(Name = "FloorNumber")]
        public int FloorNumber { get; set; } 


        public string ErrorMessage { get; set; } = "";
        public string SuccessMessage { get; set; } = "";
        public void Onpost()
        {


            //check if any required field is empty
            if (!ModelState.IsValid)
            {
                ErrorMessage = "Please fill the required field";
                return;

            }
            // add message to the database
            try
            {
                string connectionString = "Data Source=inventoryserver1.database.windows.net;Initial Catalog=Inventory;Persist Security Info=True;User ID=inventory;Password=pvamuaitc1!";
                // in try we have to connect to the database
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "insert into Room" +
                        "(Room_id, RoomNumber, BuildingName, FloorNumber) VALUES " + // these are database columns name
                        "(@Room_id,@RoomNumber,@BuildingName,@FloorNumber  );"; // these values are used to fill the columns, we will get these values from 
                    // diff properties, and we will replace these values with their paramaetrs in below codes

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
                //Error
                ErrorMessage = ex.Message; // display custom error to user 
                return; // to exit the post()

            }


            // send confirmation email to the client
            SuccessMessage = "Your message has been received correctly";
            Response.Redirect("/Admin/Room/Indexr"); 

        }

    }
}
