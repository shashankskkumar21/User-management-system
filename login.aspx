<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="login.aspx.cs" Inherits="user_management_system1.login" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>login</title>
    <link href="StyleSheet1.css" rel="stylesheet" />
</head>
<body>
     <div class="container mt-5">
        <h2>Login</h2>
  <form id="form1" runat="server">
            <div class="mb-3">
                <label for="username" class="form-label">Username</label>
                <asp:TextBox ID="txtUsername" runat="server" CssClass="form-control" placeholder="Enter your username" required="true"></asp:TextBox>
            </div>
            <div class="mb-3">
                <label for="password" class="form-label">Password</label>
                <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" CssClass="form-control" placeholder="Enter your password" required="true"></asp:TextBox>
            </div>
            <!-- Button for login -->
            <asp:Button  runat="server" OnClick="Login_Click" Text="Login" />
            <!-- Label for error messages -->
            <asp:Label ID="lblError" runat="server" CssClass="text-danger mt-3"></asp:Label>
        </form>
         </div>
</body>
</html>















