using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;

namespace inventory.Pages.Admin.Asset
{
    public class EditaModel : PageModel
    {
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




        [BindProperty, Required]
        [Display(Name = "AssetSerialNumber*")]
        public string AssetSerialNumber { get; set; } = "";


        [BindProperty]
        [Display(Name = "Manufacturer")]
        public string? Manufacturer { get; set; } = "";

        


        public string ErrorMessage { get; set; } = "";
        public string SuccessMessage { get; set; } = "";
        public void OnGet()
        {
            string requestId = Request.Query["AssetTag"];
            try
            {
                string connectionString = "Data Source=inventoryserver1.database.windows.net;Initial Catalog=Inventory;Persist Security Info=True;User ID=inventory;Password=pvamuaitc1!";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "SELECT * FROM Asset where AssetTag =@AssetTag";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@AssetTag", requestId);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                AssetTag = reader.GetString(0);
                                AssetName = reader.GetString(1);
                                AssetType = reader.GetString(2);
                                AssetSerialNumber = reader.GetString(3);
                         
                                Manufacturer = reader.GetString(4);
                         

                            }
                            else
                            {
                                Response.Redirect("/Admin/Asset/Indexa");

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Response.Redirect("/Admin/Asset/Indexa");
            }
        }
        public void OnPost()
        {
            if (!ModelState.IsValid)
            {
                ErrorMessage = "Data validation failed";
                return;

            }

            if (Manufacturer == null) Manufacturer = "";
            

            // update information in database
            try
            {
                string connectionString = "Data Source=inventoryserver1.database.windows.net;Initial Catalog=Inventory;Persist Security Info=True;User ID=inventory;Password=pvamuaitc1!";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "UPDATE Asset SET  AssetName=@AssetName, AssetType=@AssetType, " +
                       " AssetSerialNumber= @AssetSerialNumber, Manufacturer=@Manufacturer where AssetTag=@AssetTag;";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {

                        command.Parameters.AddWithValue("@AssetName", AssetName);
                        command.Parameters.AddWithValue("@AssetType", AssetType);
                        command.Parameters.AddWithValue("@AssetTag", AssetTag);
                        command.Parameters.AddWithValue("@AssetSerialNumber", AssetSerialNumber);
                        command.Parameters.AddWithValue("@Manufacturer", Manufacturer);
                        


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
            Response.Redirect("/Admin/Asset/Indexa");
        }

    }
}
