using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Windows.Forms;

namespace ContractMonthlyClaimSystem
{
    public partial class GeneralPanelForm : Form
    {
        private readonly string connectionString = "Server=labG9AEB3\\MSSQLSERVER01;Database=LecturerClaimsDB;Integrated Security=True;";
        private string loggedInUsername = "exampleUsername"; // This should be dynamically set based on the logged-in user

        public GeneralPanelForm()
        {
            InitializeComponent();
        }

        // Form Load event handler
        private void GeneralPanelForm_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(loggedInUsername))
            {
                MessageBox.Show("User is not logged in.");
                return;
            }

            LoadUserDetails(loggedInUsername);
        }

        // Load user details based on the logged-in username
        private void LoadUserDetails(string username)
        {
            string query = "SELECT * FROM Users WHERE Username = @username";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@username", username);

                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        lblName.Text = reader["Name"].ToString();
                        lblRole.Text = reader["Role"].ToString();
                    }
                    else
                    {
                        MessageBox.Show("User not found.");
                    }
                }
                catch (Exception ex)
                {
                    LogError($"Error fetching user details for '{username}': {ex.Message}");
                    MessageBox.Show("Error fetching user details. Please try again later.");
                }
            }
        }

        // Submit Request button click handler
        private void btnSubmitRequest_Click(object sender, EventArgs e)
        {
            if (ValidateRequestInputs())
            {
                try
                {
                    // Additional logic to save or process the request
                    // e.g., Saving request to a database can be added here
                    MessageBox.Show("Request submitted successfully.");
                }
                catch (Exception ex)
                {
                    LogError($"Error submitting request for employee '{txtEmployeeName.Text}': {ex.Message}");
                    MessageBox.Show("Error submitting request. Please try again.");
                }
            }
        }

        // Validate input fields for the request
        private bool ValidateRequestInputs()
        {
            if (string.IsNullOrWhiteSpace(txtEmployeeName.Text))
            {
                MessageBox.Show("Please enter an employee name.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtRequestType.Text) || string.IsNullOrWhiteSpace(txtDetails.Text))
            {
                MessageBox.Show("Please fill out all request details.");
                return false;
            }

            return true;
        }

        // Log errors to a file for debugging purposes
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