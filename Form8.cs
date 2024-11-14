using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.IO;

namespace ContractMonthlyClaimSystem
{
    public partial class ManagerPanelForm : Form
    {
        private string connectionString = "Server=labG9AEB3\\MSSQLSERVER01;Database=LecturerClaimsDB;Integrated Security=True;"; // Connection string to database
        private string selectedRequestId; // Ensure this is set elsewhere in your form, e.g., based on user selection from a list

        public ManagerPanelForm()
        {
            InitializeComponent();
        }

        // Form Load event handler (add any initializations if needed)
        private void ManagerPanelForm_Load(object sender, EventArgs e)
        {
            // Any necessary initializations can be added here, such as loading data into UI controls
        }

        // Approve request button click handler
        private void btnApproveRequest_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(selectedRequestId))
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
                    cmd.Parameters.AddWithValue("@requestId", selectedRequestId); // Ensure it's a valid value
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

            // Additional submission logic (e.g., saving employee data) can go here

            MessageBox.Show("Employee name submitted.");
        }

        // Method to check if the request exists in the database
        private bool RequestExists(int requestId)
        {
            string query = "SELECT COUNT(*) FROM Requests WHERE RequestID = @requestId";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@requestId", requestId);
                    int count = (int)cmd.ExecuteScalar();
                    return count > 0;
                }
            }
            catch (Exception ex)
            {
                LogError("Error checking request existence: " + ex.Message); // Log the error
                MessageBox.Show("Error checking request existence. Please try again later.");
                return false;
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