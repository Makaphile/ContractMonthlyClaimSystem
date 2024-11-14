using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Windows.Forms;

namespace ContractMonthlyClaimSystem
{
    public partial class HRPanelForm : Form
    {
        private string connectionString = "Server=labG9AEB3\\MSSQLSERVER01;Database=LecturerClaimsDB;Integrated Security=True;"; // Connection string to database

        public HRPanelForm()
        {
            InitializeComponent();
        }

        // Form Load event handler (add any initializations if needed)
        private void HRPanelForm_Load(object sender, EventArgs e)
        {
            // Any necessary initializations can be added here.
        }

        // Add Employee button click handler
        private void btnAddEmployee_Click(object sender, EventArgs e)
        {
            string name = txtEmployeeName.Text;
            string department = txtDepartment.Text;
            string position = txtPosition.Text; // Assuming there's a text box for position
            string hireDateText = txtHireDate.Text; // Assuming there's a text box for hire date

            // Validate input fields
            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(position))
            {
                MessageBox.Show("Please enter both employee name and position.");
                return;
            }

            if (string.IsNullOrWhiteSpace(hireDateText) || !DateTime.TryParse(hireDateText, out DateTime hireDate))
            {
                MessageBox.Show("Please enter a valid hire date.");
                return;
            }

            // Insert employee into the database
            string query = "INSERT INTO Employees (Name, Department, Position, HireDate) VALUES (@name, @department, @position, @hireDate)";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@name", name);
                    cmd.Parameters.AddWithValue("@department", department);
                    cmd.Parameters.AddWithValue("@position", position);
                    cmd.Parameters.AddWithValue("@hireDate", hireDate);
                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show("Employee added successfully.");
            }
            catch (Exception ex)
            {
                LogError("Error adding employee: " + ex.Message); // Log the error
                MessageBox.Show("Error adding employee. Please try again later.");
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
