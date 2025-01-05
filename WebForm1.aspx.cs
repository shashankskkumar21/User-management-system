using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Npgsql;

namespace user_management_system1
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        public List<string> Categories { get; private set; }

        protected void Page_Load(object sender, EventArgs e)
        { 
        
            // Read categories from the text file
            string filePath = Server.MapPath("~/TextFile1.txt");
            Categories = new List<string>(File.ReadAllLines(filePath));

            // Define specific categories for each role
            var adminCategories = Categories; // Admin can see all categories
            var editCategories = Categories.Take(6).ToList(); // Edit can see and edit first 6 categories
            var viewCategories = Categories.Take(3).ToList();
            // Get the user role from session
            string userRole = Session["userRole"]?.ToString();





            // Loop through all categories and display the ones the user is allowed to view
            foreach (var category in Categories)
            {
                HtmlGenericControl li = new HtmlGenericControl("li");
                li.ID = category; // Set a unique ID
                li.Attributes["class"] = "list-group-item";

                Button a = new Button();
                a.CssClass = "link-button"; // Apply the CSS class
                a.Click += navOnclick; // Subscribe to the Click event
                a.Text = category;

                bool canView = false;
                bool canEdit = false;
                bool canDelete = false;

                if (userRole == "Admin")
                {
                    // Admin can view, edit, and delete all categories
                    canView = true;
                    canEdit = true;
                    canDelete = true;
                }
                else if (userRole == "Edit" && editCategories.Contains(category))
                {
                    canView = true;
                    canEdit = true;
                }
                else if (userRole == "View" && viewCategories.Contains(category))
                {
                    canView = true;
                }

                // Only create and add the element if the user has permission to view
                if (canView)
                {
                    li.Controls.Add(a); // Add the <a> element to the <li>
                    ltCategories.Controls.Add(li); // Add the <li> element to the parent control
                }
            }


        }

        protected void navOnclick(object sender, EventArgs e)
        {
            string connString = System.Configuration.ConfigurationManager.ConnectionStrings["PostgreSqlConnection"].ConnectionString;

            // SQL Query to fetch the desired column
            string query = "SELECT data FROM userdata.data_table WHERE category = @conditionvalue";

            using (NpgsqlConnection conn = new NpgsqlConnection(connString))
            {
                try
                {
                    conn.Open(); // Open the connection

                    using (NpgsqlCommand cmd = new NpgsqlCommand(query, conn))
                    {
                        Button tempButton = sender as Button;

                        // Add parameters to prevent SQL injection
                        cmd.Parameters.AddWithValue("@conditionvalue", tempButton.Text);

                        // Fill a DataTable with the retrieved data
                        using (NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            adapter.Fill(dt);

                            // Create the GridView dynamically
                            GridView gridView = new GridView();
                            gridView.AutoGenerateColumns = false;
                            gridView.ID = "GridView1";

                            // Add a BoundField for the data column
                            BoundField boundField = new BoundField();
                            boundField.DataField = "data"; // Bind the 'data' column from the DataTable
                            boundField.HeaderText = "Data";
                            gridView.Columns.Add(boundField);

                            // Add View column
                            string userRole = Session["userRole"]?.ToString();

                            // Add View column
                            TemplateField viewField = new TemplateField();
                            viewField.HeaderText = "View";
                            viewField.ItemTemplate = new GridViewCommandTemplate("View", userRole); // Pass userRole
                            gridView.Columns.Add(viewField);

                            // Add Edit column
                            TemplateField editField = new TemplateField();
                            editField.HeaderText = "Edit";
                            editField.ItemTemplate = new GridViewCommandTemplate("Edit", userRole); // Pass userRole
                            gridView.Columns.Add(editField);

                            // Add Delete column (if applicable)
                            TemplateField deleteField = new TemplateField();
                            deleteField.HeaderText = "Delete";
                            deleteField.ItemTemplate = new GridViewCommandTemplate("Delete", userRole); // Pass userRole
                            gridView.Columns.Add(deleteField);

                            // Add Delete column
                    //        TemplateField deleteField = new TemplateField();
                    //        deleteField.HeaderText = "Delete";
                    //        deleteField.ItemTemplate = new GridViewCommandTemplate("Delete");
                    //        gridView.Columns.Add(deleteField);

                            // Bind data to GridView
                            gridView.DataSource = dt;
                            gridView.DataBind();
                            // Find the ContentPlaceHolder that contains the panel
                            ContentPlaceHolder contentPlaceHolder = this.Master.FindControl("ContentPlaceHolder1") as ContentPlaceHolder;

                            if (contentPlaceHolder != null)
                            {
                                Panel panelGridView = contentPlaceHolder.FindControl("panelGridView") as Panel;
                                if (panelGridView != null)
                                {
                                    panelGridView.Controls.Clear(); // Clear any previous controls
                                    panelGridView.Controls.Add(gridView); // Add the newly created GridView
                                }
                                else
                                {
                                    Response.Write("Panel not found.");
                                }
                            }
                            else
                            {
                                Response.Write("ContentPlaceHolder not found.");
                            }

                        }
                    }
                }
                catch (Exception ex)
                {
                    // Handle exceptions
                    Response.Write($"Error: {ex.Message}");
                }
            }
        }


        public class GridViewCommandTemplate : ITemplate
        {
            private string commandName;
            private string userRole;

            public GridViewCommandTemplate(string commandName, string userRole)
            {
                this.commandName = commandName;
                this.userRole = userRole;
            }

            public void InstantiateIn(Control container)
            {
                LinkButton button = new LinkButton();
                button.CommandName = this.commandName;
                button.Text = this.commandName;

                button.DataBinding += (sender, e) =>
                {
                    LinkButton btn = (LinkButton)sender;

                    // Access the parent GridViewRow
                    GridViewRow row = (GridViewRow)btn.NamingContainer;

                    // Retrieve the data field using DataBinder.Eval() to bind to the 'data' column
                    string dataValue = DataBinder.Eval(row.DataItem, "data").ToString();

                    // Set the CommandArgument to the value from the data field
                    btn.CommandArgument = dataValue;

                    // Set visibility based on user role and command
                    btn.Visible = ShouldButtonBeVisible(this.commandName, this.userRole);
                };

                button.Click += new EventHandler(Button_Click);
                container.Controls.Add(button);
            }

            private bool ShouldButtonBeVisible(string command, string role)
            {
                switch (command)
                {
                    case "View":
                        return true; // All roles can view
                    case "Edit":
                        return role == "Admin" || role == "Edit"; // Only Admin and Edit roles can edit
                    case "Delete":
                        return role == "Admin"; // Only Admin can delete
                    default:
                        return false; // Invalid command
                }
            }

            private void Button_Click(object sender, EventArgs e)
            {
                string userRole = HttpContext.Current.Session["userRole"]?.ToString();

                if (userRole == null)
                {
                    HttpContext.Current.Response.Write("Error: User role is not set in the session.");
                    return;
                }

                LinkButton btn = sender as LinkButton;
                if (btn == null)
                {
                    HttpContext.Current.Response.Write("Error: Sender is not a LinkButton.");
                    return;
                }

                string command = btn.CommandName;
                string dataValue = btn.CommandArgument;

                // Role-based action handling
                if (command == "View")
                {
                    HttpContext.Current.Response.Write($"Viewing Data: {dataValue}");
                }
                else if (command == "Edit")
                {
                    if (userRole == "Admin" || userRole == "Edit")
                    {
                        HttpContext.Current.Response.Redirect($"EditPage.aspx?data={dataValue}");
                    }
                    else
                    {
                        HttpContext.Current.Response.Write("You do not have permission to edit this data.");
                    }
                }
                else if (command == "Delete")
                {
                    if (userRole == "Admin")
                    {
                        HttpContext.Current.Response.Write($"Delete clicked for data: {dataValue}");
                    }
                    else
                    {
                        HttpContext.Current.Response.Write("You do not have permission to delete this data.");
                    }
                }
                else
                {
                    HttpContext.Current.Response.Write("Invalid action.");
                }
            }
        }


    }
}
        
        
    


    
 