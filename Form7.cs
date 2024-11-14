using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Windows.Forms;

namespace ContractMonthlyClaimSystem
{
    public partial class AdminPanelForm : Form
    {
        private string connectionString = "Server=labG9AEB3\\MSSQLSERVER01;Database=LecturerClaimsDB;Integrated Security=True;"; // Connection string to database

        public AdminPanelForm()
        {
            InitializeComponent();
        }

        // Load event for the form (you can add any initialization logic here)
        private void AdminPanelForm_Load(object sender, EventArgs e)
        {
            // Any initializations or loading logic goes here
        }

        // Approve request button click handler
        private void btnApproveRequest_Click(object sender, EventArgs e)
        {
            string requestId = selectedRequestId; // Ensure selectedRequestId is populated elsewhere in your code
            if (string.IsNullOrEmpty(requestId))
            {
                MessageBox.Show("Please select a request to approve.");
                return;
            }

            string query = "UPDATE Requests SET Status = 'Approved' WHERE RequestID = @requestId";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@requestId", requestId);
                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show("Request approved.");
            }
            catch (Exception ex)
            {
                LogError("Error approving request: " + ex.Message); // Log the error
                MessageBox.Show("Error approving request. Please try again later.");
            }
        }

        // Submit button click handler (for validating employee name)
        private void btnSubmit_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtEmployeeName.Text))
            {
                MessageBox.Show("Please enter an employee name.");
                return;
            }

            // Additional submission logic can be added here
            MessageBox.Show("Employee name submitted.");
        }

        // Add user button click handler
        private void btnAddUser_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPassword.Text;
            string role = cbRole.SelectedItem?.ToString();

            // Validate input fields
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password) || string.IsNullOrEmpty(role))
            {
                MessageBox.Show("Please fill out all fields and select a role.");
                return;
            }

            // Check if user already exists
            if (UserExists(username))
            {
                MessageBox.Show("Username already exists. Please choose another.");
                return;
            }

            // Add new user logic (store user data in the database)
            try
            {
                AddNewUser(username, password, role);
                MessageBox.Show("User added successfully.");
            }
            catch (Exception ex)
            {
                LogError("Error adding user: " + ex.Message); // Log the error
                MessageBox.Show("An error occurred while adding the user.");
            }
        }

        // Method to check if the user already exists in the database
        private bool UserExists(string username)
        {
            string query = "SELECT COUNT(*) FROM Users WHERE Username = @username";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@username", username);
                    int count = (int)cmd.ExecuteScalar();
                    return count > 0;
                }
            }
            catch (Exception ex)
            {
                LogError("Error checking user existence: " + ex.Message); // Log the error
                MessageBox.Show("Error checking user existence. Please try again.");
                return false;
            }
        }

        // Method to add a new user to the database
        private void AddNewUser(string username, string password, string role)
        {
            string query = "INSERT INTO Users (Username, Password, Role) VALUES (@username, @password, @role)";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@username", username);
                    cmd.Parameters.AddWithValue("@password", password); // In a real-world app, consider hashing passwords
                    cmd.Parameters.AddWithValue("@role", role);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                LogError("Error adding user to the database: " + ex.Message); // Log the error
                throw; // Rethrow exception to be caught in the calling method
            }
        }

        // Log errors to a text file
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
                MessageBox.Show("Error logging error: " + ex.Message); // Ensure logging issues are shown
            }
        }
    }
}