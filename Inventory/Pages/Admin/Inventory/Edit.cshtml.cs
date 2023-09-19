using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;

namespace inventory.Pages.Admin.Inventory
{
    public class EditModel : PageModel
    {
        [BindProperty]
        [Required(ErrorMessage = "The InventoryID is required")]
        [Display(Name = "InventoryID*")]
        public int InventoryID { get; set; } 

        [BindProperty]
        [Required(ErrorMessage = "The AssetName is required")]
        [Display(Name = "assetname*")]
        public string assetname { get; set; } = "";

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
        [Display(Name = "Issued_ID")]
        public int? Issued_ID { get; set; }

        [BindProperty]
        [Display(Name = "Status")]
        public string? Status { get; set; } = "";

        [BindProperty]
        [MaxLength(100, ErrorMessage = "The Notes cannot exceed 100 character")]
        [Display(Name = "Notes")]
        public string? Notes { get; set; } = "";

        [BindProperty]
        [Display(Name = "IssuedRoom")]
        public string? IssuedRoom { get; set; } = "";




        public string ErrorMessage { get; set; } = "";
        public string SuccessMessage { get; set; } = "";
        public void OnGet()
        {
            string requestId = Request.Query["InventoryID"];
            try
            {
                string connectionString = "Data Source=inventoryserver1.database.windows.net;Initial Catalog=Inventory;Persist Security Info=True;User ID=inventory;Password=pvamuaitc1!";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "SELECT * FROM Inventory where InventoryID =@InventoryID";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@InventoryID", requestId);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                InventoryID = reader.GetInt32(0);
                                assetname = reader.GetString(1);
                                IssuedDepartment = reader.GetString(2);
                                AssetTag = reader.GetString(3);
                                CurrentOwner = reader.GetString(4);
                                PreviousOwner = reader.GetString(5);
                                DateIssued = reader.GetDateTime(6).ToString("MM/dd/yyyy");
                                Issued_ID = reader.GetInt32(7);
                                Status = reader.GetString(8);
                                Notes = reader.GetString(9);
                                IssuedRoom = reader.GetString(10);


                            }
                            else
                            {
                                Response.Redirect("/Admin/Inventory/Index");

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Response.Redirect("/Admin/Inventory/Index");
            }
        }
        public void OnPost()
        {
            if (!ModelState.IsValid)
            {
                ErrorMessage = "Data validation failed";
                return;

            }
            
            if (Notes == null) Notes = "";
            if (IssuedDepartment == null) IssuedDepartment = "";
            if (AssetTag == null) AssetTag = "";
            if (Status == null) Status = "";
            if (IssuedRoom == null) IssuedRoom = "";
            if (CurrentOwner == null) CurrentOwner = "";
            if (PreviousOwner == null) PreviousOwner = "";

            // update information in database
            try
            {
                string connectionString = "Data Source=inventoryserver1.database.windows.net;Initial Catalog=Inventory;Persist Security Info=True;User ID=inventory;Password=pvamuaitc1!";
                using (SqlConnection connection = new SqlConnection(connectionString)) 
                {
                    connection.Open();
                    string sql = "UPDATE Inventory SET  assetname=@assetname, IssuedDepartment=@IssuedDepartment, " +
                       " AssetTag= @AssetTag, CurrentOwner=@CurrentOwner, PreviousOwner=@PreviousOwner, DateIssued=@DateIssued, " +
                       " IssuedRoom=@IssuedRoom, Status= @Status, Notes=@Notes, Issued_ID=@Issued_ID WHERE InventoryID = @InventoryID;";
                    using (SqlCommand command = new SqlCommand(sql, connection)) 
                    {
                       
                        command.Parameters.AddWithValue("@assetname", assetname);
                        command.Parameters.AddWithValue("@IssuedDepartment", IssuedDepartment);
                        command.Parameters.AddWithValue("@AssetTag", AssetTag);
                        command.Parameters.AddWithValue("@CurrentOwner", CurrentOwner);
                        command.Parameters.AddWithValue("@PreviousOwner", PreviousOwner);
                        command.Parameters.AddWithValue("@DateIssued", DateIssued);
                        command.Parameters.AddWithValue("@IssuedRoom", IssuedRoom);
                        command.Parameters.AddWithValue("@Issued_ID", Issued_ID);
                        command.Parameters.AddWithValue("@Notes", Notes);
                        command.Parameters.AddWithValue("@Status", Status);
                        command.Parameters.AddWithValue("@InventoryID", InventoryID);


                        command.ExecuteNonQuery();


                    }


                }
            }
            catch (Exception ex) 
            {
                ErrorMessage= ex.Message;
                return;
            }

            SuccessMessage = "Data saved correctly";
            Response.Redirect("/Admin/Inventory/Index");
        }
    }
}
