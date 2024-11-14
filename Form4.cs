using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace ContractMonthlyClaimSystem
{
    public partial class ManagerForm : Form
    {
        private SqlConnection connection;

        public ManagerForm()
        {
            InitializeComponent();
            // Initialize connection string for SQL Server database
            connection = new SqlConnection("Server=labG9AEB3\\MSSQLSERVER01; Database=LecturerClaimsDB; Integrated Security=True;");
        }

        private void ManagerForm_Load(object sender, EventArgs e)
        {
            LoadPendingClaims();
        }

        // Load all pending claims for review
        private void LoadPendingClaims()
        {
            try
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM Claims WHERE Status = 'Pending'", connection);
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

        // Approve selected claim
        private void ApproveClaim(int claimID)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("UPDATE Claims SET Status = 'Approved' WHERE ClaimID = @ClaimID", connection);
                cmd.Parameters.AddWithValue("@ClaimID", claimID);
                connection.Open();
                cmd.ExecuteNonQuery();
                connection.Close();
                MessageBox.Show("Claim approved successfully.");
                LoadPendingClaims(); // Refresh claims list
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error approving claim: " + ex.Message);
            }
        }

        // Reject selected claim
        private void RejectClaim(int claimID)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("UPDATE Claims SET Status = 'Rejected' WHERE ClaimID = @ClaimID", connection);
                cmd.Parameters.AddWithValue("@ClaimID", claimID);
                connection.Open();
                cmd.ExecuteNonQuery();
                connection.Close();
                MessageBox.Show("Claim rejected successfully.");
                LoadPendingClaims(); // Refresh claims list
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error rejecting claim: " + ex.Message);
            }
        }

        // Event handlers for approve and reject buttons
        private void btnApprove_Click(object sender, EventArgs e)
        {
            if (dataGridViewClaims.SelectedRows.Count > 0)
            {
                int claimID = Convert.ToInt32(dataGridViewClaims.SelectedRows[0].Cells["ClaimID"].Value);
                ApproveClaim(claimID);
            }
        }

        private void btnReject_Click(object sender, EventArgs e)
        {
            if (dataGridViewClaims.SelectedRows.Count > 0)
            {
                int claimID = Convert.ToInt32(dataGridViewClaims.SelectedRows[0].Cells["ClaimID"].Value);
                RejectClaim(claimID);
            }
        }
    }
}