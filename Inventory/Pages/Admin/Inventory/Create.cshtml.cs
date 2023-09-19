using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;

namespace inventory.Pages.Admin.Inventory
{
    public class CreateModel : PageModel
    {
        [BindProperty]
        [Required(ErrorMessage = "The InventoryID is required")]
        [Display(Name = "InventoryID*")]
        public int InventoryID { get; set; } 

        [BindProperty]
        [Required(ErrorMessage = "The AssetName is required")]
        [Display(Name = "AssetName*")]
        public string AssetName { get; set; } = "";

        [BindProperty]
        [Display(Name = "IssuedDepartment")]
        public string? IssuedDepartment { get; set; } = "";

        [BindProperty]
        [Display(Name = "AssetTag")]
        public string? AssetTag { get; set; } = "";

        [BindProperty]
        [Display(Name = "CurrentOwner")]
        public string? CurrentOwner { get; set; } = "";

        [BindProperty]
        [Display(Name = "PreviousOwner")]
        public string? PreviousOwner { get; set; } = "";

        [BindProperty]
        [Display(Name = "DateIssued")]
        public String? DateIssued { get; set; } = "";

        [BindProperty]
        [Display(Name = "IssuedRoom")]
        public string? IssuedRoom { get; set; } = "";

        [BindProperty]
        [MaxLength (100, ErrorMessage="The Notes cannot exceed 100 character")]
        [Display(Name = "Notes")]
        public string? Notes { get; set; } = "";

        [BindProperty]
        [Display(Name = "Status")]
        public string? Status { get; set; } = "";

        [BindProperty]
        [Display(Name = "Issued_ID")]
        public int Issued_ID { get; set; } 

        public string ErrorMessage { get; set; } = "";
        public string SuccessMessage { get; set; } = "";

        public void OnGet()
        {
        }

        public void Onpost()
        {


            //check if any required field is empty for manditory field or if not  validated
            if (!ModelState.IsValid)
            {
                ErrorMessage = "Please fill the required field";
                return;

            }
            if (Notes == null) Notes = "";
            // add message to the database
            try
            {
                string connectionString = "Data Source=inventoryserver1.database.windows.net;Initial Catalog=Inventory;Persist Security Info=True;User ID=inventory;Password=pvamuaitc1!";
                // in try we have to connect to the database
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "insert into Inventory " +
                        "(InventoryID, AssetName, IssuedDepartment, AssetTag, CurrentOwner, PreviousOwner, DateIssued, IssuedRoom, Issued_ID, Notes, Status) VALUES " + // these are database columns name
                        "(@InventoryID, @AssetName, @IssuedDepartment, @AssetTag, @CurrentOwner, @PreviousOwner, @DateIssued, @IssuedRoom, @Issued_ID, @Notes, @Status);"; // these values are used to fill the columns, we will get these values from 
                    // diff properties, and we will replace these values with their paramaetrs in below codes

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@InventoryID", InventoryID);
                        command.Parameters.AddWithValue("@AssetName", AssetName);
                        command.Parameters.AddWithValue("@IssuedDepartment", IssuedDepartment);
                        command.Parameters.AddWithValue("@AssetTag", AssetTag);
                        command.Parameters.AddWithValue("@CurrentOwner", CurrentOwner);
                        command.Parameters.AddWithValue("@PreviousOwner", PreviousOwner);
                        command.Parameters.AddWithValue("@DateIssued", DateIssued);
                        command.Parameters.AddWithValue("@IssuedRoom", IssuedRoom);
                        command.Parameters.AddWithValue("@Issued_ID", Issued_ID);
                        command.Parameters.AddWithValue("@Notes", Notes);
                        command.Parameters.AddWithValue("@Status", Status);


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


            // either sucess msg will be presented or will send back to inventory list after data will be added sucessfully
            
            SuccessMessage = "Inventory data has been saved correctly";
            Response.Redirect("/Admin/Inventory/Index");

            

        }
    }
}
