using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace ContractMonthlyClaimSystem
{
    public partial class HRForm : Form
    {
        private SqlConnection connection;

        public HRForm()
        {
            InitializeComponent();
            // Initialize connection string for SQL Server database
            connection = new SqlConnection("Server=labG9AEB3\\MSSQLSERVER01; Database=LecturerClaimsDB; Integrated Security=True;");
        }

        private void HRForm_Load(object sender, EventArgs e)
        {
            LoadApprovedClaims();
        }

        // Load all approved claims for HR processing
        private void LoadApprovedClaims()
        {
            try
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM Claims WHERE Status = 'Approved'", connection);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridViewClaims.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading claims: " + ex.Message);
            }
        }

        // Finalize selected claim
        private void FinalizeClaim(int claimID)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("UPDATE Claims SET Status = 'Finalized' WHERE ClaimID = @ClaimID", connection);
                cmd.Parameters.AddWithValue("@ClaimID", claimID);
                connection.Open();
                cmd.ExecuteNonQuery();
                connection.Close();
                MessageBox.Show("Claim finalized successfully.");
                LoadApprovedClaims(); // Refresh claims list
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error finalizing claim: " + ex.Message);
            }
        }

        // Event handler for Finalize button
        private void btnFinalize_Click(object sender, EventArgs e)
        {
            if (dataGridViewClaims.SelectedRows.Count > 0)
            {
                int claimID = Convert.ToInt32(dataGridViewClaims.SelectedRows[0].Cells["ClaimID"].Value);
                FinalizeClaim(claimID);
            }
        }
    }
}
