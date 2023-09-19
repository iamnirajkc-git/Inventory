using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;

namespace Inventory.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {

        }
        [BindProperty]
        [Required(ErrorMessage = "The DepartmentID is required")]
        [Display(Name = "DepartmentID*")]
        public string DepartmentID { get; set; } = "";

        [BindProperty]
        [Required(ErrorMessage = "The DepartmentName is required")]
        [Display(Name = "DepartmentName*")]
        public string DepartmentName { get; set; } = "";


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
                    string sql = "insert into Department " +
                        "(DepartmentID, DepartmentName) VALUES " + // these are database columns name
                        "(@DepartmentID, @DepartmentName);"; // these values are used to fill the columns, we will get these values from 
                    // diff properties, and we will replace these values with their paramaetrs in below codes

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
                //Error
                ErrorMessage = ex.Message; // display custom error to user 
                return; // to exit the post()

            }


            // send confirmation email to the client
            SuccessMessage = "Your message has been received correctly";
            DepartmentID = "";
            DepartmentName = "";
            ModelState.Clear();// without this form will always display older values

        }
    }
}