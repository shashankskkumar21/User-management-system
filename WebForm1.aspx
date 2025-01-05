<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="WebForm1.aspx.cs" Inherits="user_management_system1.WebForm1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <!doctype html>
     
     <link href="StyleSheet(dashboad).css" rel="stylesheet" />

    <title>Dashboard</title>
  
            <script>
                function showCategory(content) {
                    document.getElementById("content").innerHTML = `<h3>${content}</h3><p>Details about ${content} will be displayed here.</p>`;
        }
               
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

   

    <div class="container mt-5">
        <h2>Welcome, <%= Session["Username"] %></h2>
        <a href="Login.aspx" class="btn btn-danger mb-3">Logout</a>
       
        <ul ID="ltCategories" runat="server" class="list-group">
            <!-- Literal control to render the categories dynamically -->
        </ul>

        <div id="content" class="mt-3">
           <!-- <p>Select a category from the list to view details.</p> -->
            <asp:Panel ID="panelGridView" runat="server"></asp:Panel>
            

        </div>
        

    </div>



    
    
</asp:Content>
