using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace ContractMonthlyClaimSystem
{
    public partial class LecturerForm : Form
    {
        // Database connection string
        private SqlConnection connection;

        public LecturerForm()
        {
            InitializeComponent();
            // Initialize connection string for SQL Server database
            connection = new SqlConnection("Server=labG9AEB3\\MSSQLSERVER01; Database=LecturerClaimsDB; Integrated Security=True;");
        }

        private void LecturerForm_Load(object sender, EventArgs e)
        {
            // Load claims for the lecturer when form loads
            LoadClaims();
        }

        // Load claims related to the lecturer from the database
        private void LoadClaims()
        {
            try
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM Claims WHERE UserRole = 'Lecturer'", connection);
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

        // Submit claim for the lecturer
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

        // Add new claim for lecturer
        private void addClaimButton_Click(object sender, EventArgs e)
        {
            try
            {
                string claimDescription = txtClaimDescription.Text;
                if (string.IsNullOrEmpty(claimDescription))
                {
                    MessageBox.Show("Please enter a valid claim description.");
                    return;
                }

                if (!decimal.TryParse(txtClaimAmount.Text, out decimal claimAmount) || claimAmount <= 0)
                {
                    MessageBox.Show("Please enter a valid claim amount.");
                    return;
                }

                SqlCommand cmd = new SqlCommand("INSERT INTO Claims (Description, Amount, UserRole, Status) VALUES (@description, @amount, 'Lecturer', 'Pending')", connection);
                cmd.Parameters.AddWithValue("@description", claimDescription);
                cmd.Parameters.AddWithValue("@amount", claimAmount);
                connection.Open();
                cmd.ExecuteNonQuery();
                connection.Close();
                MessageBox.Show("Claim added successfully.");
            }
            catch (Exception ex)
            {
                LogError($"Error adding claim: {ex.Message}");
                MessageBox.Show("Error adding claim. Please try again.");
            }
        }

        // Submit grades for lecturer
        private void submitGradesButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(lecturerID) || string.IsNullOrEmpty(grade))
                {
                    MessageBox.Show("Please provide valid lecturer ID and grade.");
                    return;
                }

                SqlCommand cmd = new SqlCommand("INSERT INTO Grades (LecturerID, Grade) VALUES (@LecturerID, @Grade)", connection);
                cmd.Parameters.AddWithValue("@LecturerID", lecturerID);
                cmd.Parameters.AddWithValue("@Grade", grade);
                connection.Open();
                cmd.ExecuteNonQuery();
                connection.Close();
                MessageBox.Show("Grades submitted successfully.");
            }
            catch (Exception ex)
            {
                LogError($"Error submitting grades: {ex.Message}");
                MessageBox.Show("Error submitting grades. Please try again.");
            }
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

        // Logout Button Click Event
        private void logoutButton_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form1 loginForm = new Form1();
            loginForm.Show();
        }
    }
}
