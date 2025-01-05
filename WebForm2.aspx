<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebForm2.aspx.cs" Inherits="user_management_system1.WebForm2" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    
    
 <title>Register</title>
    <link href="StyleSheet2%20register.css" rel="stylesheet" />

</head>

   

<body>
    <form id="form1" runat="server">
       

    <h2>Register</h2>
    
   

    <!-- Username -->
    <label for="username">Username:</label>
    <input runat ="server" type="text" id="username" name="username" required /><br>

    <!-- Name -->
    <label for="name">Name:</label>
    <input runat ="server" type="text" id="name" name="name" required /><br>

    <!-- Password -->
    <label for="password">Password:</label>
    <input runat ="server"  type="password" id="password" name="password" required minlength="6" /><br>

    <!-- Re-type Password -->
    <label for="confirmPassword">Re-type Password:</label>
    <input type="password" id="confirmPassword" name="confirmPassword" required /><br>
    <span id="passwordError" style="color: red; display: none;">Passwords do not match!</span><br>

    <!-- Role -->
       <div>
            
    <label  for="role">Role:</label>
    <select  runat="server" id="role" name="role" required>
        <option value="" disabled="disabled" selected="selected">Select a role</option>
        <option value="View">Guest</option>
        <option value="Edit">Employee</option>
        <option value="Admin">Admin</option>
    </select>

<ul id="ltCategories" runat ="server" class="list-group">

   <!-- Dropdown with checkboxes -->
<div class="dropdown">
    <button class="btn btn-primary dropdown-toggle" type="button" id="categoryDropdown" data-bs-toggle="dropdown" aria-expanded="false">
        Select Categories
    </button>
    <ul class="dropdown-menu" aria-labelledby="categoryDropdown">
        <li>
            <div class="form-check">
                <input class="form-check-input category-checkbox" type="checkbox" id="chk_Category1">
                <label class="form-check-label" for="chk_Category1">Category1</label>
            </div>
            <div class="form-check">
                <input class="form-check-input permission-checkbox" type="checkbox" id="chk_edit_Category1">
                <label class="form-check-label" for="chk_edit_Category1">Edit</label>
            </div>
            <div class="form-check">
                <input class="form-check-input permission-checkbox" type="checkbox" id="chk_view_Category1">
                <label class="form-check-label" for="chk_view_Category1">View</label>
            </div>
            <div class="form-check">
                <input class="form-check-input permission-checkbox" type="checkbox" id="chk_delete_Category1">
                <label class="form-check-label" for="chk_delete_Category1">Delete</label>
            </div>
        </li>
        <li>
            <div class="form-check">
                <input class="form-check-input category-checkbox" type="checkbox" id="chk_Category2">
                <label class="form-check-label" for="chk_Category2">Category2</label>
            </div>
            <div class="form-check">
                <input class="form-check-input permission-checkbox" type="checkbox" id="chk_edit_Category2">
                <label class="form-check-label" for="chk_edit_Category2">Edit</label>
            </div>
            <div class="form-check">
                <input class="form-check-input permission-checkbox" type="checkbox" id="chk_view_Category2">
                <label class="form-check-label" for="chk_view_Category2">View</label>
            </div>
            <div class="form-check">
                <input class="form-check-input permission-checkbox" type="checkbox" id="chk_delete_Category2">
                <label class="form-check-label" for="chk_delete_Category2">Delete</label>
            </div>
        </li>

        <!-- Repeat for more categories -->
    </ul>
    </div>
    </ul>
</div>

<script>
    // Function to toggle permissions when category checkbox is checked
    document.querySelectorAll('.category-checkbox').forEach(function (categoryCheckbox) {
        categoryCheckbox.addEventListener('change', function () {
            let categoryId = this.id.split('_')[1]; // Extract category ID (Category1, Category2, etc.)
            let permissionCheckboxes = document.querySelectorAll(`#chk_edit_${categoryId}, #chk_view_${categoryId}, #chk_delete_${categoryId}`);
            permissionCheckboxes.forEach(function (permissionCheckbox) {
                permissionCheckbox.checked = categoryCheckbox.checked; // Toggle permission checkboxes based on category checkbox state
            });
        });
    });
</script>

    <!-- Submit Button -->
 <asp:Button runat="server" OnClick="Unnamed_Click" Text="Register" />
           

</form>


<script>
    // Client-side validation for password match
    const form = document.querySelector('form');
    const passwordInput = document.getElementById('password');
    const confirmPasswordInput = document.getElementById('confirmPassword');
    const passwordError = document.getElementById('passwordError');

    form.addEventListener('submit', function (event) {
        if (passwordInput.value !== confirmPasswordInput.value) {
            event.preventDefault(); // Prevent form submission
            passwordError.style.display = 'block'; // Show error
        } else {
            passwordError.style.display = 'none'; // Hide error
        }
    });
</script>

</body>
</html>