using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MyPortfolio
{
    public partial class Admin : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Check if user is authenticated via session or cookie
            if (!IsAuthenticated())
            {
                Response.Redirect("Login.aspx");
            }

            // Set username from session or cookie
            if (Session["Username"] == null && Request.Cookies["userInfo"] != null)
            {
                Session["Username"] = Request.Cookies["userInfo"]["username"];
            }

            if (!IsPostBack)
            {
                //hfActiveTab.Value = "#skills";

                LoadSkills();
                LoadProjects();
                LoadEducation();
                LoadProfile();
                LoadMessages();
            }
        }

        private bool IsAuthenticated()
        {
            // Check session first
            if (Session["IsAuthenticated"] != null && (bool)Session["IsAuthenticated"])
            {
                return true;
            }

            // Check cookie if session is expired
            if (Request.Cookies["userInfo"] != null)
            {
                string username = Request.Cookies["userInfo"]["username"];

                // Validate the cookie content against database
                if (IsValidUserCookie(username))
                {
                    // Re-establish session
                    Session["IsAuthenticated"] = true;
                    Session["Username"] = username;
                    return true;
                }
            }

            return false;
        }

        private bool IsValidUserCookie(string username)
        {
            string connectionString = WebConfigurationManager.ConnectionStrings["portfolioDB"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("SELECT UserId FROM Users WHERE Username = @Username AND IsActive = 1", conn);
                    cmd.Parameters.AddWithValue("@Username", username);

                    conn.Open();
                    object result = cmd.ExecuteScalar();

                    return result != null;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("Cookie validation error: " + ex.Message);
                    return false;
                }
            }
        }

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            // Clear session
            Session.Clear();
            Session.Abandon();

            // Clear authentication cookie
            if (Request.Cookies["userInfo"] != null)
            {
                HttpCookie cookie = new HttpCookie("userInfo");
                cookie.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(cookie);
            }

            // Clear all other cookies
            string[] cookiesToClear = Request.Cookies.AllKeys;
            foreach (string cookieName in cookiesToClear)
            {
                HttpCookie cookie = new HttpCookie(cookieName);
                cookie.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(cookie);
            }

            Response.Redirect("Login.aspx");
        }

        #region Skills Management
        private void LoadSkills()
        {
            string connectionString = WebConfigurationManager.ConnectionStrings["portfolioDB"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM Skills ORDER BY DisplayOrder", conn);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                gvSkills.DataSource = dt;
                gvSkills.DataBind();
            }
        }

        protected void btnAddSkill_Click(object sender, EventArgs e)
        {
            string connectionString = WebConfigurationManager.ConnectionStrings["portfolioDB"].ConnectionString;
            string imagePath = "";

            // Handle image upload
            if (skillImage.HasFile)
            {
                imagePath = HandleImageUpload(skillImage, "skills");
                if (string.IsNullOrEmpty(imagePath))
                {
                    return;
                }
            }
            else
            {
                ShowAlert("Please select an image for the skill.");
                return;
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("INSERT INTO Skills (Name, Description, IconClass, DisplayOrder, IsActive) VALUES (@Name, @Description, @IconClass, @DisplayOrder, 1)", conn);
                cmd.Parameters.AddWithValue("@Name", txtSkillName.Text.Trim());
                cmd.Parameters.AddWithValue("@Description", txtSkillDescription.Text.Trim());
                cmd.Parameters.AddWithValue("@IconClass", imagePath);
                cmd.Parameters.AddWithValue("@DisplayOrder", string.IsNullOrEmpty(txtSkillOrder.Text) ? 0 : int.Parse(txtSkillOrder.Text));

                conn.Open();
                cmd.ExecuteNonQuery();
            }

            // Clear form and refresh grid
            txtSkillName.Text = "";
            txtSkillDescription.Text = "";
            txtSkillOrder.Text = "";
            LoadSkills();

            ShowAlert("Skill added successfully!", false);
        }

        protected void gvSkills_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvSkills.EditIndex = e.NewEditIndex;
            LoadSkills();
        }

        protected void gvSkills_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            GridViewRow row = gvSkills.Rows[e.RowIndex];
            int skillId = Convert.ToInt32(gvSkills.DataKeys[e.RowIndex].Value);

            string name = ((TextBox)row.FindControl("txtEditName")).Text;
            int order = Convert.ToInt32(((TextBox)row.FindControl("txtEditOrder")).Text);

            string currentImagePath = ((HiddenField)row.FindControl("hfCurrentImage")).Value;
            string newImagePath = currentImagePath;

            // Handle file upload if a new file was selected
            FileUpload fuEditSkillImage = (FileUpload)row.FindControl("fuEditSkillImage");
            if (fuEditSkillImage != null && fuEditSkillImage.HasFile)
            {
                newImagePath = HandleImageUpload(fuEditSkillImage, "skills");
                if (string.IsNullOrEmpty(newImagePath))
                {
                    newImagePath = currentImagePath;
                }
                else
                {
                    // Delete old image file if it's different from the new one
                    if (currentImagePath != newImagePath && !string.IsNullOrEmpty(currentImagePath))
                    {
                        DeleteImageFile(currentImagePath);
                    }
                }
            }

            string connectionString = WebConfigurationManager.ConnectionStrings["portfolioDB"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("UPDATE Skills SET Name = @Name, IconClass = @IconClass, DisplayOrder = @DisplayOrder WHERE SkillId = @SkillId", conn);
                cmd.Parameters.AddWithValue("@Name", name);
                cmd.Parameters.AddWithValue("@IconClass", newImagePath);
                cmd.Parameters.AddWithValue("@DisplayOrder", order);
                cmd.Parameters.AddWithValue("@SkillId", skillId);

                conn.Open();
                cmd.ExecuteNonQuery();
            }

            gvSkills.EditIndex = -1;
            LoadSkills();

            ShowAlert("Skill updated successfully!", false);
        }

        protected void gvSkills_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvSkills.EditIndex = -1;
            LoadSkills();
        }

        protected void gvSkills_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int skillId = Convert.ToInt32(gvSkills.DataKeys[e.RowIndex].Value);

            string connectionString = WebConfigurationManager.ConnectionStrings["portfolioDB"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                // First get the image path to delete the file
                SqlCommand getCmd = new SqlCommand("SELECT IconClass FROM Skills WHERE SkillId = @SkillId", conn);
                getCmd.Parameters.AddWithValue("@SkillId", skillId);

                conn.Open();
                string imagePath = getCmd.ExecuteScalar()?.ToString();

                // Delete the record
                SqlCommand deleteCmd = new SqlCommand("DELETE FROM Skills WHERE SkillId = @SkillId", conn);
                deleteCmd.Parameters.AddWithValue("@SkillId", skillId);
                deleteCmd.ExecuteNonQuery();

                // Delete the image file if it exists
                if (!string.IsNullOrEmpty(imagePath))
                {
                    DeleteImageFile(imagePath);
                }
            }

            LoadSkills();
            ShowAlert("Skill deleted successfully!", false);
        }
        #endregion

        #region Projects Management
        private void LoadProjects()
        {
            string connectionString = WebConfigurationManager.ConnectionStrings["portfolioDB"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM Projects ORDER BY DisplayOrder", conn);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                gvProjects.DataSource = dt;
                gvProjects.DataBind();
            }
        }

        protected void btnAddProject_Click(object sender, EventArgs e)
        {
            string connectionString = WebConfigurationManager.ConnectionStrings["portfolioDB"].ConnectionString;
            string imagePath = "";

            // Handle image upload for projects
            if (projectImageUpload.HasFile)
            {
                imagePath = HandleImageUpload(projectImageUpload, "projects");
                if (string.IsNullOrEmpty(imagePath))
                {
                    return;
                }
            }
            else
            {
                ShowAlert("Please select an image for the project.");
                return;
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("INSERT INTO Projects (Title, ShortDescription, FullDescription, Technologies, ImageUrl, ProjectUrl, DisplayOrder, IsActive) VALUES (@Title, @ShortDescription, @FullDescription, @Technologies, @ImageUrl, @ProjectUrl, @DisplayOrder, 1)", conn);
                cmd.Parameters.AddWithValue("@Title", txtProjectTitle.Text.Trim());
                cmd.Parameters.AddWithValue("@ShortDescription", txtProjectShortDesc.Text.Trim());
                cmd.Parameters.AddWithValue("@FullDescription", txtProjectFullDesc.Text.Trim());
                cmd.Parameters.AddWithValue("@Technologies", txtProjectTech.Text.Trim());
                cmd.Parameters.AddWithValue("@ImageUrl", imagePath);
                cmd.Parameters.AddWithValue("@ProjectUrl", txtProjectUrl.Text.Trim());
                cmd.Parameters.AddWithValue("@DisplayOrder", string.IsNullOrEmpty(txtProjectOrder.Text) ? 0 : int.Parse(txtProjectOrder.Text));

                conn.Open();
                cmd.ExecuteNonQuery();
            }

            ClearProjectForm();
            LoadProjects();
            ShowAlert("Project added successfully!", false);
        }

        protected void gvProjects_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvProjects.EditIndex = e.NewEditIndex;
            LoadProjects();
        }

        protected void gvProjects_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            GridViewRow row = gvProjects.Rows[e.RowIndex];
            int projectId = Convert.ToInt32(gvProjects.DataKeys[e.RowIndex].Value);

            // Title
            string title = ((TextBox)row.FindControl("txtEditTitle"))?.Text ?? "";

            // Order
            int order = 0;
            int.TryParse(((TextBox)row.FindControl("txtEditOrder"))?.Text, out order);

            // Current image
            string currentImagePath = ((HiddenField)row.FindControl("hfCurrentProjectImage"))?.Value ?? "";
            string newImagePath = currentImagePath;

            // File upload
            FileUpload fuEditProjectImage = (FileUpload)row.FindControl("fuEditProjectImage");
            if (fuEditProjectImage != null && fuEditProjectImage.HasFile)
            {
                newImagePath = HandleImageUpload(fuEditProjectImage, "projects");
                if (!string.IsNullOrEmpty(newImagePath) && currentImagePath != newImagePath)
                {
                    DeleteImageFile(currentImagePath);
                }
            }

            // Update database
            string connectionString = WebConfigurationManager.ConnectionStrings["portfolioDB"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("UPDATE Projects SET Title = @Title, ImageUrl = @ImageUrl, DisplayOrder = @DisplayOrder WHERE ProjectId = @ProjectId", conn);
                cmd.Parameters.AddWithValue("@Title", title);
                cmd.Parameters.AddWithValue("@ImageUrl", newImagePath);
                cmd.Parameters.AddWithValue("@DisplayOrder", order);
                cmd.Parameters.AddWithValue("@ProjectId", projectId);

                conn.Open();
                cmd.ExecuteNonQuery();
            }

            gvProjects.EditIndex = -1;
            LoadProjects();
            ShowAlert("Project updated successfully!", false);
        }

        protected void gvProjects_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvProjects.EditIndex = -1;
            LoadProjects();
        }

        protected void gvProjects_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int projectId = Convert.ToInt32(gvProjects.DataKeys[e.RowIndex].Value);

            string connectionString = WebConfigurationManager.ConnectionStrings["portfolioDB"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                // First get the image path to delete the file
                SqlCommand getCmd = new SqlCommand("SELECT ImageUrl FROM Projects WHERE ProjectId = @ProjectId", conn);
                getCmd.Parameters.AddWithValue("@ProjectId", projectId);

                conn.Open();
                string imagePath = getCmd.ExecuteScalar()?.ToString();

                // Delete the record
                SqlCommand deleteCmd = new SqlCommand("DELETE FROM Projects WHERE ProjectId = @ProjectId", conn);
                deleteCmd.Parameters.AddWithValue("@ProjectId", projectId);
                deleteCmd.ExecuteNonQuery();

                // Delete the image file if it exists
                if (!string.IsNullOrEmpty(imagePath))
                {
                    DeleteImageFile(imagePath);
                }
            }

            LoadProjects();
            ShowAlert("Project deleted successfully!", false);
        }
        #endregion

        #region Education Management
        private void LoadEducation()
        {
            string connectionString = WebConfigurationManager.ConnectionStrings["portfolioDB"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM Education ORDER BY DisplayOrder, EndDate DESC", conn);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                gvEducation.DataSource = dt;
                gvEducation.DataBind();
            }
        }

        protected void btnAddEducation_Click(object sender, EventArgs e)
        {
            if (!ValidateEducationDates())
            {
                ShowAlert("End date cannot be earlier than start date.");
                return;
            }

            string connectionString = WebConfigurationManager.ConnectionStrings["portfolioDB"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("INSERT INTO Education (Institution, Degree, FieldOfStudy, StartDate, EndDate, Description, Grade, DisplayOrder, IsActive) VALUES (@Institution, @Degree, @FieldOfStudy, @StartDate, @EndDate, @Description, @Grade, @DisplayOrder, 1)", conn);
                cmd.Parameters.AddWithValue("@Institution", txtInstitution.Text.Trim());
                cmd.Parameters.AddWithValue("@Degree", txtDegree.Text.Trim());
                cmd.Parameters.AddWithValue("@FieldOfStudy", txtFieldOfStudy.Text.Trim());
                cmd.Parameters.AddWithValue("@StartDate", Convert.ToDateTime(txtStartDate.Text));
                cmd.Parameters.AddWithValue("@EndDate", string.IsNullOrEmpty(txtEndDate.Text) ? (object)DBNull.Value : Convert.ToDateTime(txtEndDate.Text));
                cmd.Parameters.AddWithValue("@Description", txtEduDescription.Text.Trim());
                cmd.Parameters.AddWithValue("@Grade", txtGrade.Text.Trim());
                cmd.Parameters.AddWithValue("@DisplayOrder", string.IsNullOrEmpty(txtEduOrder.Text) ? 0 : int.Parse(txtEduOrder.Text));

                conn.Open();
                cmd.ExecuteNonQuery();
            }

            // Clear form and refresh grid
            ClearEducationForm();
            LoadEducation();
            ShowAlert("Education added successfully!", false);
        }

        private bool ValidateEducationDates()
        {
            if (!string.IsNullOrEmpty(txtEndDate.Text))
            {
                DateTime startDate = Convert.ToDateTime(txtStartDate.Text);
                DateTime endDate = Convert.ToDateTime(txtEndDate.Text);
                return endDate >= startDate;
            }
            return true;
        }

        protected void gvEducation_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvEducation.EditIndex = e.NewEditIndex;
            LoadEducation();
        }

        protected void gvEducation_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            GridViewRow row = gvEducation.Rows[e.RowIndex];
            int educationId = Convert.ToInt32(gvEducation.DataKeys[e.RowIndex].Value);

            string institution = ((TextBox)row.FindControl("txtEditInstitution")).Text;
            string degree = ((TextBox)row.FindControl("txtEditDegree")).Text;
            DateTime startDate = Convert.ToDateTime(((TextBox)row.FindControl("txtEditStartDate")).Text);
            string endDateText = ((TextBox)row.FindControl("txtEditEndDate")).Text;
            int order = Convert.ToInt32(((TextBox)row.FindControl("txtEditOrder")).Text);

            string connectionString = WebConfigurationManager.ConnectionStrings["portfolioDB"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("UPDATE Education SET Institution = @Institution, Degree = @Degree, StartDate = @StartDate, EndDate = @EndDate, DisplayOrder = @DisplayOrder WHERE EducationId = @EducationId", conn);
                cmd.Parameters.AddWithValue("@Institution", institution);
                cmd.Parameters.AddWithValue("@Degree", degree);
                cmd.Parameters.AddWithValue("@StartDate", startDate);
                cmd.Parameters.AddWithValue("@EndDate", string.IsNullOrEmpty(endDateText) ? (object)DBNull.Value : Convert.ToDateTime(endDateText));
                cmd.Parameters.AddWithValue("@DisplayOrder", order);
                cmd.Parameters.AddWithValue("@EducationId", educationId);

                conn.Open();
                cmd.ExecuteNonQuery();
            }

            gvEducation.EditIndex = -1;
            LoadEducation();
            ShowAlert("Education updated successfully!", false);
        }

        protected void gvEducation_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvEducation.EditIndex = -1;
            LoadEducation();
        }

        protected void gvEducation_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int educationId = Convert.ToInt32(gvEducation.DataKeys[e.RowIndex].Value);

            string connectionString = WebConfigurationManager.ConnectionStrings["portfolioDB"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("DELETE FROM Education WHERE EducationId = @EducationId", conn);
                cmd.Parameters.AddWithValue("@EducationId", educationId);

                conn.Open();
                cmd.ExecuteNonQuery();
            }

            LoadEducation();
            ShowAlert("Education deleted successfully!", false);
        }

        protected string FormatEducationDate(object startDate, object endDate)
        {
            DateTime start = Convert.ToDateTime(startDate);
            string end = (endDate == DBNull.Value || endDate == null) ? "Present" : Convert.ToDateTime(endDate).ToString("yyyy");

            return start.ToString("yyyy") + " - " + end;
        }
        #endregion

        #region Profile Management
        private void LoadProfile()
        {
            string connectionString = WebConfigurationManager.ConnectionStrings["portfolioDB"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SELECT TOP 1 * FROM Profile", conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    txtFullName.Text = reader["FullName"].ToString();
                    txtTitle.Text = reader["Title"].ToString();
                    txtDescription.Text = reader["Description"].ToString();
                    txtEmail.Text = reader["Email"].ToString();
                    txtPhone.Text = reader["Phone"].ToString();
                    txtLocation.Text = reader["Location"].ToString();
                }

                reader.Close();
            }
        }

        protected void btnUpdateProfile_Click(object sender, EventArgs e)
        {
            string connectionString = WebConfigurationManager.ConnectionStrings["portfolioDB"].ConnectionString;
            string imagePath = "";

            // Handle profile image upload
            if (profileImageUpload.HasFile)
            {
                imagePath = HandleImageUpload(profileImageUpload, "profile");
                if (string.IsNullOrEmpty(imagePath))
                {
                    return;
                }
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                // Check if profile exists
                SqlCommand checkCmd = new SqlCommand("SELECT COUNT(*) FROM Profile", conn);
                conn.Open();
                int count = (int)checkCmd.ExecuteScalar();

                if (count > 0)
                {
                    // Update existing profile
                    SqlCommand cmd;
                    if (!string.IsNullOrEmpty(imagePath))
                    {
                        cmd = new SqlCommand("UPDATE Profile SET FullName = @FullName, Title = @Title, Description = @Description, Email = @Email, Phone = @Phone, Location = @Location, ProfileImage = @ProfileImage, UpdatedDate = GETDATE()", conn);
                        cmd.Parameters.AddWithValue("@ProfileImage", imagePath);
                    }
                    else
                    {
                        cmd = new SqlCommand("UPDATE Profile SET FullName = @FullName, Title = @Title, Description = @Description, Email = @Email, Phone = @Phone, Location = @Location, UpdatedDate = GETDATE()", conn);
                    }

                    cmd.Parameters.AddWithValue("@FullName", txtFullName.Text.Trim());
                    cmd.Parameters.AddWithValue("@Title", txtTitle.Text.Trim());
                    cmd.Parameters.AddWithValue("@Description", txtDescription.Text.Trim());
                    cmd.Parameters.AddWithValue("@Email", txtEmail.Text.Trim());
                    cmd.Parameters.AddWithValue("@Phone", txtPhone.Text.Trim());
                    cmd.Parameters.AddWithValue("@Location", txtLocation.Text.Trim());

                    cmd.ExecuteNonQuery();
                }
                else
                {
                    // Insert new profile
                    SqlCommand cmd = new SqlCommand("INSERT INTO Profile (FullName, Title, Description, Email, Phone, Location, ProfileImage) VALUES (@FullName, @Title, @Description, @Email, @Phone, @Location, @ProfileImage)", conn);
                    cmd.Parameters.AddWithValue("@FullName", txtFullName.Text.Trim());
                    cmd.Parameters.AddWithValue("@Title", txtTitle.Text.Trim());
                    cmd.Parameters.AddWithValue("@Description", txtDescription.Text.Trim());
                    cmd.Parameters.AddWithValue("@Email", txtEmail.Text.Trim());
                    cmd.Parameters.AddWithValue("@Phone", txtPhone.Text.Trim());
                    cmd.Parameters.AddWithValue("@Location", txtLocation.Text.Trim());
                    cmd.Parameters.AddWithValue("@ProfileImage", imagePath);

                    cmd.ExecuteNonQuery();
                }
            }

            lblProfileMessage.Text = "Profile updated successfully!";
            lblProfileMessage.Visible = true;
        }
        #endregion

        #region Messages Management
        private void LoadMessages(string filter = "all")
        {
            string connectionString = WebConfigurationManager.ConnectionStrings["portfolioDB"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT ContactId, Name, Email, Subject, Message, IsRead, CreatedDate FROM Contacts ";

                switch (filter)
                {
                    case "unread":
                        query += "WHERE IsRead = 0 ";
                        break;
                    case "read":
                        query += "WHERE IsRead = 1 ";
                        break;
                }

                query += "ORDER BY CreatedDate DESC";

                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                gvMessages.DataSource = dt;
                gvMessages.DataBind();
            }
        }

        protected string GetMessagePreview(string message)
        {
            if (string.IsNullOrEmpty(message))
                return string.Empty;

            if (message.Length > 100)
                return message.Substring(0, 100) + "...";

            return message;
        }

        protected void ddlMessageFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadMessages(ddlMessageFilter.SelectedValue);
        }

        protected void gvMessages_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                bool isRead = Convert.ToBoolean(DataBinder.Eval(e.Row.DataItem, "IsRead"));
                if (!isRead)
                {
                    e.Row.CssClass = "unread-message";
                }
            }
        }

        protected void gvMessages_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "ViewMessage")
            {
                int contactId = Convert.ToInt32(e.CommandArgument);
                ViewMessageDetails(contactId);
            }
            else if (e.CommandName == "DeleteMessage")
            {
                int contactId = Convert.ToInt32(e.CommandArgument);
                DeleteMessage(contactId);
            }
        }

        private void ViewMessageDetails(int contactId)
        {
            string connectionString = WebConfigurationManager.ConnectionStrings["portfolioDB"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SELECT ContactId, Name, Email, Subject, Message, IsRead, CreatedDate FROM Contacts WHERE ContactId = @ContactId", conn);
                cmd.Parameters.AddWithValue("@ContactId", contactId);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    // Store message details in ViewState for later use
                    ViewState["CurrentMessageId"] = contactId;
                    ViewState["CurrentMessageIsRead"] = reader["IsRead"];

                    // Register JavaScript to show modal with message details
                    string script = $@"
                document.getElementById('modalMessageSubject').innerText = '{EscapeJavaScriptString(reader["Subject"].ToString())}';
                document.getElementById('modalMessageName').innerText = '{EscapeJavaScriptString(reader["Name"].ToString())}';
                document.getElementById('modalMessageEmail').innerText = '{EscapeJavaScriptString(reader["Email"].ToString())}';
                document.getElementById('modalMessageDate').innerText = '{Convert.ToDateTime(reader["CreatedDate"]).ToString("MMM dd, yyyy HH:mm")}';
                document.getElementById('modalMessageContent').innerText = '{EscapeJavaScriptString(reader["Message"].ToString())}';
                
                // Show/hide Mark as Read button
                var markAsReadBtn = document.getElementById('{btnMarkAsRead.ClientID}');
                if (markAsReadBtn) {{
                    markAsReadBtn.style.display = {(Convert.ToBoolean(reader["IsRead"]) ? "'none'" : "'block'")};
                }}
                
                // Show modal
                document.getElementById('messageModal').style.display = 'block';
            ";

                    ClientScript.RegisterStartupScript(this.GetType(), "ShowMessageModal", script, true);

                    // Mark as read if it's unread
                    if (!Convert.ToBoolean(reader["IsRead"]))
                    {
                        MarkMessageAsRead(contactId);
                    }
                }
                reader.Close();
            }
        }

        private string EscapeJavaScriptString(string input)
        {
            return input.Replace("'", "\\'").Replace("\"", "\\\"").Replace("\r", "").Replace("\n", "\\n");
        }

        protected void btnMarkAsRead_Click(object sender, EventArgs e)
        {
            if (ViewState["CurrentMessageId"] != null)
            {
                int contactId = Convert.ToInt32(ViewState["CurrentMessageId"]);
                MarkMessageAsRead(contactId);

                // Refresh the messages grid
                LoadMessages(ddlMessageFilter.SelectedValue);

                // Hide the mark as read button
                string script = @"
            document.getElementById('messageModal').style.display = 'none';
        ";
                ClientScript.RegisterStartupScript(this.GetType(), "CloseModal", script, true);
            }
        }

        private void MarkMessageAsRead(int contactId)
        {
            string connectionString = WebConfigurationManager.ConnectionStrings["portfolioDB"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("UPDATE Contacts SET IsRead = 1 WHERE ContactId = @ContactId", conn);
                cmd.Parameters.AddWithValue("@ContactId", contactId);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        private void DeleteMessage(int contactId)
        {
            string connectionString = WebConfigurationManager.ConnectionStrings["portfolioDB"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("DELETE FROM Contacts WHERE ContactId = @ContactId", conn);
                cmd.Parameters.AddWithValue("@ContactId", contactId);

                conn.Open();
                cmd.ExecuteNonQuery();
            }

            LoadMessages(ddlMessageFilter.SelectedValue);
            ShowAlert("Message deleted successfully!", false);
        }
        #endregion

        #region Helper Methods
        private string HandleImageUpload(FileUpload fileUpload, string subfolder = "")
        {
            try
            {
                string fileName = Path.GetFileName(fileUpload.FileName);
                string extension = Path.GetExtension(fileName).ToLower();

                if (extension == ".jpg" || extension == ".jpeg" || extension == ".png" || extension == ".gif")
                {
                    string folderPath = Server.MapPath("~/Assets/uploads/");
                    if (!string.IsNullOrEmpty(subfolder))
                    {
                        folderPath = Path.Combine(folderPath, subfolder);
                    }

                    if (!Directory.Exists(folderPath))
                    {
                        Directory.CreateDirectory(folderPath);
                    }

                    string uniqueFileName = Guid.NewGuid().ToString() + extension;
                    string fullPath = Path.Combine(folderPath, uniqueFileName);

                    fileUpload.SaveAs(fullPath);

                    // Return relative path without ~
                    return !string.IsNullOrEmpty(subfolder)
                        ? $"Assets/uploads/{subfolder}/{uniqueFileName}"
                        : $"Assets/uploads/{uniqueFileName}";
                }
                else
                {
                    ShowAlert("Only JPG, PNG, and GIF formats are allowed!");
                    return "";
                }
            }
            catch (Exception ex)
            {
                ShowAlert("Error uploading image: " + ex.Message);
                return "";
            }
        }

        private void DeleteImageFile(string imagePath)
        {
            try
            {
                if (!string.IsNullOrEmpty(imagePath) && File.Exists(Server.MapPath("~/" + imagePath)))
                {
                    File.Delete(Server.MapPath("~/" + imagePath));
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error deleting image file: " + ex.Message);
            }
        }

        private void ClearProjectForm()
        {
            txtProjectTitle.Text = "";
            txtProjectShortDesc.Text = "";
            txtProjectFullDesc.Text = "";
            txtProjectTech.Text = "";
            txtProjectUrl.Text = "";
            txtProjectOrder.Text = "";
        }

        private void ClearEducationForm()
        {
            txtInstitution.Text = "";
            txtDegree.Text = "";
            txtFieldOfStudy.Text = "";
            txtStartDate.Text = "";
            txtEndDate.Text = "";
            txtEduDescription.Text = "";
            txtGrade.Text = "";
            txtEduOrder.Text = "";
        }

        private void ShowAlert(string message, bool isError = true)
        {
            string script = $"alert('{message.Replace("'", "\\'")}');";
            ClientScript.RegisterClientScriptBlock(this.GetType(), "alertScript", script, true);
        }
        #endregion

        protected void gvSkills_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Optional: Handle selection if needed
        }
    }
}