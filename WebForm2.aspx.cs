using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Npgsql;

namespace user_management_system1
{
    public partial class WebForm2 : System.Web.UI.Page
    {
         public List<string> Categories { get; private set; }
        protected void Page_Load(object sender, EventArgs e)
        {

            string filepath = Server.MapPath("TextFile1.txt");
            Categories = new List<string>(File.ReadAllLines(filepath));

            if (IsPostBack)
            {

                // Create the dropdown container div
                HtmlGenericControl dropdownContainer = new HtmlGenericControl("div");
                dropdownContainer.Attributes["class"] = "dropdown";

                // Create the dropdown button
                Button dropdownButton = new Button();
                dropdownButton.Text = "Select Categories";
                dropdownButton.CssClass = "btn btn-primary dropdown-toggle";
                dropdownButton.Attributes.Add("type", "button");
                dropdownButton.Attributes.Add("id", "categoryDropdown");
                dropdownButton.Attributes.Add("data-bs-toggle", "dropdown");
                dropdownButton.Attributes.Add("aria-expanded", "false");
                dropdownContainer.Controls.Add(dropdownButton);

                // Create the dropdown menu
                HtmlGenericControl dropdownMenu = new HtmlGenericControl("ul");
                dropdownMenu.Attributes["class"] = "dropdown-menu";
                dropdownMenu.Attributes["aria-labelledby"] = "categoryDropdown";

                // Loop through each category to add category and permission checkboxes
                foreach (string category in Categories)
                {
                    // Create a list item for this category
                    HtmlGenericControl listItem = new HtmlGenericControl("li");

                    // Create the category checkbox
                    CheckBox categoryCheckBox = new CheckBox();
                    categoryCheckBox.ID = "chk_" + category.Replace(" ", "_");
                    categoryCheckBox.Text = category;
                    categoryCheckBox.CssClass = "category-checkbox";
                    listItem.Controls.Add(categoryCheckBox);

                    // Create a container div to hold permission checkboxes
                    HtmlGenericControl permissionsContainer = new HtmlGenericControl("div");

                    // Add Edit checkbox
                    CheckBox editCheckBox = new CheckBox();
                    editCheckBox.ID = "chk_edit_" + category.Replace(" ", "_");
                    editCheckBox.Text = "Edit";
                    editCheckBox.CssClass = "permission-checkbox";
                    permissionsContainer.Controls.Add(editCheckBox);

                    // Add View checkbox
                    CheckBox viewCheckBox = new CheckBox();
                    viewCheckBox.ID = "chk_view_" + category.Replace(" ", "_");
                    viewCheckBox.Text = "View";
                    viewCheckBox.CssClass = "permission-checkbox";
                    permissionsContainer.Controls.Add(viewCheckBox);

                    // Add Delete checkbox
                    CheckBox deleteCheckBox = new CheckBox();
                    deleteCheckBox.ID = "chk_delete_" + category.Replace(" ", "_");
                    deleteCheckBox.Text = "Delete";
                    deleteCheckBox.CssClass = "permission-checkbox";
                    permissionsContainer.Controls.Add(deleteCheckBox);

                    // Add the permissions container to the list item
                    listItem.Controls.Add(permissionsContainer);

                    // Add the list item to the dropdown menu
                    dropdownMenu.Controls.Add(listItem);
                }

                // Add the dropdown menu to the dropdown container
                dropdownContainer.Controls.Add(dropdownMenu);

                // Add the dropdown to the page or parent control
                ltCategories.Controls.Add(dropdownContainer);


            }
        }

        protected void Unnamed_Click(object sender, EventArgs e)
        {
            string connString = System.Configuration.ConfigurationManager.ConnectionStrings["PostgreSqlConnection"].ConnectionString;

            using (NpgsqlConnection conn = new NpgsqlConnection(connString))
            {
                conn.Open();
                string query = "INSERT INTO userdata.users (username, password, role, name) VALUES (@username, @password, @role, @name)";
                using (NpgsqlCommand cmd = new NpgsqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@username", username.Value);
                    cmd.Parameters.AddWithValue("@password", password.Value);
                    cmd.Parameters.AddWithValue("@role", role.Value);
                    cmd.Parameters.AddWithValue("@name", name.Value);
                    try
            {
                cmd.ExecuteNonQuery();
                Response.Redirect("Login.aspx");
            }
            catch (Exception ex)
            {
                Response.Write($"<script>alert('Error: {ex.Message}');</script>");
            }

        }
            }
        }
    }
}

