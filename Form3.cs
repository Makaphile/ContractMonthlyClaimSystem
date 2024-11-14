using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Windows.Forms;

namespace ContractMonthlyClaimSystem
{
    public partial class CoordinatorForm : Form
    {
        // Database connection string
        private SqlConnection connection;

        public CoordinatorForm()
        {
            InitializeComponent();
            // Initialize connection string for SQL Server database
            connection = new SqlConnection("Server=labG9AEB3\\MSSQLSERVER01; Database=LecturerClaimsDB; Integrated Security=True;");
        }

        private void CoordinatorForm_Load(object sender, EventArgs e)
        {
            // Load claims data as soon as the form loads.
            LoadClaims();
        }

        // Load the claims for the coordinator
        private void LoadClaims()
        {
            try
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM Claims WHERE UserRole = 'Coordinator'", connection);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridViewClaims.DataSource = dt;
            }
            catch (Exception ex)
            {
                LogError($"Error loading claims: {ex.Message}");
                MessageBox.Show("Error loading claims. Please try again.");
            }
        }

        // Submit claims
        private void submitButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(selectedClaimID))
                {
                    MessageBox.Show("Please select a valid claim.");
                    return;
                }

                SqlCommand cmd = new SqlCommand("UPDATE Claims SET Status = 'Submitted' WHERE ClaimID = @ClaimID", connection);
                cmd.Parameters.AddWithValue("@ClaimID", selectedClaimID);
                connection.Open();
                cmd.ExecuteNonQuery();
                connection.Close();
                MessageBox.Show("Claim submitted successfully.");
            }
            catch (Exception ex)
            {
                LogError($"Error submitting claim {selectedClaimID}: {ex.Message}");
                MessageBox.Show("Error submitting claim. Please try again.");
            }
        }

        // Handle Lecturer Assignments
        private void handleLecturerAssignmentsButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(taskAssignment) || string.IsNullOrEmpty(lecturerID))
                {
                    MessageBox.Show("Please provide valid assignment details.");
                    return;
                }

                SqlCommand cmd = new SqlCommand("UPDATE Lecturers SET AssignedTask = @Task WHERE LecturerID = @LecturerID", connection);
                cmd.Parameters.AddWithValue("@Task", taskAssignment);
                cmd.Parameters.AddWithValue("@LecturerID", lecturerID);
                connection.Open();
                cmd.ExecuteNonQuery();
                connection.Close();
                MessageBox.Show("Lecturer assignments updated successfully.");
            }
            catch (Exception ex)
            {
                LogError($"Error handling lecturer assignment: {ex.Message}");
                MessageBox.Show("Error updating lecturer assignments. Please try again.");
            }
        }

        // Log errors to a file for debugging or auditing
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

        // Logout Button Click Event
        private void logoutButton_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form1 loginForm = new Form1();
            loginForm.Show();
        }
    }
}