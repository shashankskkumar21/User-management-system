using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Npgsql;

namespace user_management_system1
{
    public partial class login : System.Web.UI.Page
    {
        private object lblError;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
            {
                string username = Request.Form["username"];
                string password = Request.Form["password"];

                if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
                {

                    return;
                }

                string connString = System.Configuration.ConfigurationManager.ConnectionStrings["PostgreSqlConnection"].ConnectionString;

               
            }
        }
        protected void Login_Click(object sender, EventArgs e)
        {
            string connString = System.Configuration.ConfigurationManager.ConnectionStrings["PostgreSqlConnection"].ConnectionString;

            using (NpgsqlConnection conn = new NpgsqlConnection(connString))
            {
                conn.Open();
                string query = "SELECT role from userdata.users where username=@username and password=@password";
                using (NpgsqlCommand cmd = new NpgsqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@username", txtUsername.Text);
                    cmd.Parameters.AddWithValue("@password", txtPassword.Text);
                   
                    try
                    {
                        NpgsqlDataAdapter AD = new NpgsqlDataAdapter(cmd);
                        DataTable DA = new DataTable();
                        AD.Fill(DA);
                        if(DA.Rows.Count>0)
                        {
                            Session.Add("userRole", DA.Rows[0][0].ToString());
                            Response.Redirect("WebForm1.aspx");
                        }
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























