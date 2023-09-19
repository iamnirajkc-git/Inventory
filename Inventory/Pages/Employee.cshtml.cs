using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;

namespace inventory.Pages
{
    public class EmployeeModel : PageModel
    {
        public void OnGet()
        {
        }

        [BindProperty]
        [Required(ErrorMessage = "The EmployeeID is required")]
        [Display(Name = "EmployeeID*")]
        public string EmployeeID { get; set; } = "";

        [BindProperty]
        [Required(ErrorMessage = "The FirstName is required")]
        [Display(Name = "FirstName*")]
        public string FirstName { get; set; } = "";

        [BindProperty]
        [Required(ErrorMessage = "The LastName is required")]
        [Display(Name = "LastName*")]
        public string LastName { get; set; } = "";
        
        [BindProperty]
        [Display(Name = "DepartmentID")]
        public string? DepartmentID { get; set; } = "";

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
                    string sql = "insert into Employee " +
                        "(EmployeeID, FirstName, LastName, DepartmentID, EMPType, Room_id, Email, PhoneNumber ) VALUES " + // these are database columns name
                        "(@EmployeeID, @FirstName, @LastName, @DepartmentID, @EMPType, @Room_id, @Email, @PhoneNumber )"; // these values are used to fill the columns, we will get these values from 
                    // diff properties, and we will replace these values with their paramaetrs in below codes

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
                //Error
                ErrorMessage = ex.Message; // display custom error to user 
                return; // to exit the post()

            }
            // send confirmation email to the client
            SuccessMessage = "Your message has been received correctly";
            EmployeeID = "";
            FirstName = "";
            LastName = "";
            DepartmentID = "";
            EMPType = "";
            Room_id = "";
            Email = "";
            PhoneNumber = "";

            ModelState.Clear();// without this form will always display older values

        }
    }
}
