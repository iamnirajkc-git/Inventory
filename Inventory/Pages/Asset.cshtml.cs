using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;

namespace inventory.Pages
{
    public class AssetModel : PageModel
    {
        public void OnGet()
        {
        }
        [BindProperty]
        [Required(ErrorMessage = "The AssetTag is required")]
        [Display(Name = "AssetTag*")]
        public string AssetTag { get; set; } = "";

        [BindProperty]
        [Required(ErrorMessage = "The AssetName is required")]
        [Display(Name = "AssetName*")]
        public string AssetName { get; set; } = "";

        [BindProperty]
        [Required(ErrorMessage = "The AssetType is required")]
        [Display(Name = "AssetType*")]
        public string AssetType { get; set; } = "";
        
        [BindProperty]
        [Required(ErrorMessage = "The AssetSerialNumber is required")]
        [Display(Name = "AssetSerialNumber*")]
        public string AssetSerialNumber { get; set; } = "";

        [BindProperty]
        [Display(Name = "Manufacturer")]
        public string? Manufacturer { get; set; } = "";


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
                    string sql = "insert into Asset " +
                        "(AssetTag, AssetName, AssetType, AssetSerialNumber, Manufacturer ) VALUES " + // these are database columns name
                        "(@AssetTag, @AssetName, @AssetType, @AssetSerialNumber, @Manufacturer );"; // these values are used to fill the columns, we will get these values from 
                    // diff properties, and we will replace these values with their paramaetrs in below codes

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@AssetTag", AssetTag);
                        command.Parameters.AddWithValue("@AssetName", AssetTag);
                        command.Parameters.AddWithValue("@AssetType", AssetType);
                        command.Parameters.AddWithValue("@AssetSerialNumber", AssetSerialNumber);
                        command.Parameters.AddWithValue("@Manufacturer", Manufacturer);

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
            AssetTag = "";
            AssetType = "";
            AssetSerialNumber = "";
            Manufacturer = "";

            ModelState.Clear();// without this form will always display older values

        }

    }
}
