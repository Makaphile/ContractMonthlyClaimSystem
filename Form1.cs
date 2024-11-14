using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Windows.Forms;

namespace ContractMonthlyClaimSystem
{
    public partial class Form1 : Form
    {
        // Database connection string
        private SqlConnection connection;

        public Form1()
        {
            InitializeComponent();
            // Initialize connection string for SQL Server database
            connection = new SqlConnection("Server=labG9AEB3\\MSSQLSERVER01; Database=LecturerClaimsDB; Integrated Security=True;");
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Optional: This method can be used for any initialization on form load.
        }

        // Login Button Click Event
        private void loginButton_Click(object sender, EventArgs e)
        {
            string username = usernameTextBox.Text.Trim();
            string password = passwordTextBox.Text.Trim();

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please enter both username and password.");
                return;
            }

            try
            {
                if (ValidateLogin(username, password))
                {
                    string userRole = GetUserRole(username);
                    OpenRoleForm(userRole);
                }
                else
                {
                    MessageBox.Show("Invalid login credentials.");
                }
            }
            catch (Exception ex)
            {
                LogError($"Login attempt failed for user '{username}': {ex.Message}");
                MessageBox.Show("An error occurred during login. Please try again.");
            }
        }

        // Validate user credentials (username and password)
        private bool ValidateLogin(string username, string password)
        {
            try
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM Users WHERE Username = @Username AND Password = @Password", connection);
                cmd.Parameters.AddWithValue("@Username", username);
                cmd.Parameters.AddWithValue("@Password", password);
                int userCount = (int)cmd.ExecuteScalar();
                connection.Close();

                return userCount > 0;
            }
            catch (Exception ex)
            {
                LogError($"Error validating login for user '{username}': {ex.Message}");
                return false;
            }
        }

        // Get the role of the user (Lecturer, Coordinator, Manager)
        private string GetUserRole(string username)
        {
            try
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand("SELECT UserRole FROM Users WHERE Username = @Username", connection);
                cmd.Parameters.AddWithValue("@Username", username);
                string role = (string)cmd.ExecuteScalar();
                connection.Close();

                return role;
            }
            catch (Exception ex)
            {
                LogError($"Error retrieving user role for '{username}': {ex.Message}");
                return null;
            }
        }

        // Open the appropriate form based on the user role
        private void OpenRoleForm(string role)
        {
            Form roleForm = null;
            switch (role)
            {
                case "Lecturer":
                    roleForm = new LecturerForm();
                    break;
                case "Coordinator":
                    roleForm = new CoordinatorForm();
                    break;
                case "Manager":
                    roleForm = new ManagerForm();
                    break;
                default:
                    MessageBox.Show("Invalid role. Access denied.");
                    return;
            }

            this.Hide();
            roleForm?.Show();
        }

        // Logout Button Click Event
        private void logoutButton_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form1 loginForm = new Form1();
            loginForm.Show();
        }

        // Log errors to a file for debugging
        private void LogError(string errorMessage)
        {
            string logPath = "error_log.txt";
            try
            {
                using (StreamWriter writer = new StreamWriter(logPath, true))
                {
                    writer.WriteLine($"{DateTime.Now}: {errorMessage}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error logging the error: " + ex.Message);
            }
        }
    }
}
