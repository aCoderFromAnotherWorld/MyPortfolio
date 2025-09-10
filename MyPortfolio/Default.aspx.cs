using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using static System.ActivationContext;

namespace MyPortfolio
{
    public partial class Default1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Load skills from database
                LoadSkills();
                // Load educations frm database
                LoadEducation();
                // Load projects from database
                LoadProjects();
            }
        }

        //private void LoadProfile()
        //{
        //    string connectionString = ConfigurationManager.ConnectionStrings["portfolioDB"].ConnectionString;
        //    using (SqlConnection conn = new SqlConnection(connectionString))
        //    {
        //        try
        //        {
        //            conn.Open();
        //            SqlCommand cmd = new SqlCommand("SELECT ProfileImage FROM Profile WHERE ProfileId = 1", conn);
        //            SqlDataAdapter da = new SqlDataAdapter(cmd);
        //            DataTable dt = new DataTable();
        //            da.Fill(dt);

        //            // Bind skills to repeater
        //            rptProfile.DataSource = dt;
        //            rptProfile.DataBind();
        //            conn.Close();
        //        }
        //        catch (Exception ex)
        //        {
        //            // Log error
        //            System.Diagnostics.Debug.WriteLine("Error loading skills: " + ex.Message);
        //        }
        //    }
        //}

        private void LoadSkills()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["portfolioDB"].ConnectionString;
            using(SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("SELECT Name, IconClass, Description FROM Skills WHERE IsActive = 1 ORDER BY DisplayOrder", conn);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    // Bind skills to repeater
                    rptSkills.DataSource = dt;
                    rptSkills.DataBind();
                    conn.Close();
                }
                catch (Exception ex)
                {
                    // Log error
                    System.Diagnostics.Debug.WriteLine("Error loading skills: " + ex.Message);
                }
            }
        }

        private void LoadEducation()
        {
            string connectionString = WebConfigurationManager.ConnectionStrings["portfolioDB"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("SELECT Institution, Degree, FieldOfStudy, StartDate, EndDate, Description, Grade FROM Education WHERE IsActive = 1 ORDER BY DisplayOrder, EndDate DESC", conn);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    rptEducation.DataSource = dt;
                    rptEducation.DataBind();
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("Error loading education: " + ex.Message);
                }
            }
        }

        protected string FormatDate(object startDate, object endDate)
        {
            DateTime start = Convert.ToDateTime(startDate);
            string end = (endDate == DBNull.Value || endDate == null) ? "Present" : Convert.ToDateTime(endDate).ToString("yyyy");

            return start.ToString("yyyy") + " - " + end;
        }

        private void LoadProjects()
        {
            string connectionString = WebConfigurationManager.ConnectionStrings["portfolioDB"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("SELECT ProjectId, Title, ImageUrl, ShortDescription, FullDescription, Technologies, ProjectUrl FROM Projects WHERE IsActive = 1 ORDER BY DisplayOrder", conn);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    // Bind projects to repeater
                    rptProjects.DataSource = dt;
                    rptProjects.DataBind();
                }
                catch (Exception ex)
                {
                    // Log error
                    System.Diagnostics.Debug.WriteLine("Error loading projects: " + ex.Message);
                }
            }
        }

        [System.Web.Services.WebMethod]
        public static object SaveContact(string name, string email, string subject, string message)
        {
            try
            {
                string connectionString = WebConfigurationManager.ConnectionStrings["portfolioDB"].ConnectionString;

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand("INSERT INTO Contacts (Name, Email, Subject, Message) VALUES (@Name, @Email, @Subject, @Message)", conn);
                    cmd.Parameters.AddWithValue("@Name", name);
                    cmd.Parameters.AddWithValue("@Email", email);
                    cmd.Parameters.AddWithValue("@Subject", subject);
                    cmd.Parameters.AddWithValue("@Message", message);

                    conn.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();

                    return new { success = rowsAffected > 0 };
                }
            }
            catch (Exception ex)
            {
                // Log error
                System.Diagnostics.Debug.WriteLine("Error saving contact: " + ex.Message);
                return new { success = false, error = ex.Message };
            }
        }
        
        // Helper method to get short description
        protected string GetShortDescription(object shortDesc, object fullDesc)
        {
            string description = shortDesc as string;
            if (string.IsNullOrEmpty(description))
            {
                description = fullDesc as string;
            }

            if (!string.IsNullOrEmpty(description) && description.Length > 100)
            {
                return description.Substring(0, 100) + "...";
            }

            return description;
        }

        [System.Web.Services.WebMethod]
        public static object GetProjectDetails(int projectId)
        {
            try
            {
                string connectionString = WebConfigurationManager.ConnectionStrings["portfolioDB"].ConnectionString;

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand("SELECT Title, ImageUrl, FullDescription, Technologies, ProjectUrl FROM Projects WHERE ProjectId = @ProjectId", conn);
                    cmd.Parameters.AddWithValue("@ProjectId", projectId);

                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        return new
                        {
                            Title = reader["Title"].ToString(),
                            ImageUrl = reader["ImageUrl"].ToString(),
                            FullDescription = reader["FullDescription"].ToString(),
                            Technologies = reader["Technologies"].ToString(),
                            ProjectUrl = reader["ProjectUrl"].ToString()
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error getting project details: " + ex.Message);
            }

            return null;
        }



        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                string name = txtName.Text.Trim();
                string email = txtEmail.Text.Trim();
                string subject = txtSubject.Text.Trim();
                string message = txtMessage.Text.Trim();

                // Validate and save to database (same logic as WebMethod)
                string connectionString = WebConfigurationManager.ConnectionStrings["portfolioDB"].ConnectionString;

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand("INSERT INTO Contacts (Name, Email, Subject, Message) VALUES (@Name, @Email, @Subject, @Message)", conn);
                    cmd.Parameters.AddWithValue("@Name", name);
                    cmd.Parameters.AddWithValue("@Email", email);
                    cmd.Parameters.AddWithValue("@Subject", subject);
                    cmd.Parameters.AddWithValue("@Message", message);

                    conn.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        lblContactMessage.Text = "Message sent successfully!";
                        lblContactMessage.CssClass = "alert alert-success";
                        lblContactMessage.Visible = true;

                        // Clear form
                        txtName.Text = "";
                        txtEmail.Text = "";
                        txtSubject.Text = "";
                        txtMessage.Text = "";
                    }
                }
            }
            catch (Exception ex)
            {
                lblContactMessage.Text = "Error sending message: " + ex.Message;
                lblContactMessage.CssClass = "alert alert-error";
                lblContactMessage.Visible = true;
            }
        }
    }
}