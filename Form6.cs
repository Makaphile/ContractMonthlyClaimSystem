using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Windows.Forms;

namespace ContractMonthlyClaimSystem
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                string username = txtUsername.Text;
                string password = txtPassword.Text;

                // Check if username or password is empty
                if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
                {
                    MessageBox.Show("Please enter both username and password.");
                    return;
                }

                string role = GetUserRole(username, password);

                if (role == null)
                {
                    MessageBox.Show("Invalid username or password.");
                    LogError($"Failed login attempt for user: {username}"); // Log the failed login attempt
                }
                else
                {
                    MessageBox.Show("Login successful! Role: " + role);
                    UserSession.Role = role; // Set the user role in a session variable
                    OpenMainFormBasedOnRole(role); // Open the appropriate form based on the role
                }
            }
            catch (Exception ex)
            {
                LogError($"Error during login: {ex.Message}");
                MessageBox.Show("An error occurred during login. Please try again.");
            }
        }

        private string GetUserRole(string username, string password)
        {
            string connectionString = "Server=labG9AEB3\\MSSQLSERVER01;Database=LecturerClaimsDB;Integrated Security=True;";
            string role = null;

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT Role FROM Users WHERE Username = @username AND Password = @password";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@username", username);
                    cmd.Parameters.AddWithValue("@password", password);

                    var result = cmd.ExecuteScalar();
                    if (result != null)
                    {
                        role = result.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                LogError($"Error while connecting to the database: {ex.Message}");
                MessageBox.Show("Error while connecting to the database. Please try again later.");
            }

            return role;
        }

        private void OpenMainFormBasedOnRole(string role)
        {
            this.Hide();

            // Open the appropriate form based on the user's role
            if (role == "Lecturer")
            {
                LecturerForm lecturerForm = new LecturerForm();
                lecturerForm.Show();
            }
            else if (role == "Coordinator")
            {
                CoordinatorForm coordinatorForm = new CoordinatorForm();
                coordinatorForm.Show();
            }
            else if (role == "Manager")
            {
                ManagerForm managerForm = new ManagerForm();
                managerForm.Show();
            }
            else if (role == "HR")
            {
                HRForm hrForm = new HRForm();
                hrForm.Show();
            }
            else
            {
                MessageBox.Show("Role not recognized.");
            }
        }

        // Method to log errors
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
                MessageBox.Show("Error logging the error: " + ex.Message); // In case of logging failure
            }
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtEmployeeName.Text))
            {
                MessageBox.Show("Please enter an employee name.");
                return;
            }

            // Handle submission logic if needed
            MessageBox.Show("Employee name submitted.");
        }
    }
}