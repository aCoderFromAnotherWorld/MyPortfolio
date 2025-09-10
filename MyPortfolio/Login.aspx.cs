//using System;
//using System.Data;
//using System.Data.SqlClient;
//using System.Web;
//using System.Web.Configuration;
//using System.Web.UI;

//namespace MyPortfolio
//{
//    public partial class Login : System.Web.UI.Page
//    {
//        protected void Page_Load(object sender, EventArgs e)
//        {
//            // Redirect to admin if already logged in
//            if (Session["IsAuthenticated"] != null && (bool)Session["IsAuthenticated"])
//            {
//                Response.Redirect("Admin.aspx");
//            }
//        }

//        protected void btnLogin_Click(object sender, EventArgs e)
//        {
//            string username = txtUsername.Text.Trim();
//            string password = txtPassword.Text;

//            if (AuthenticateUser(username, password))
//            {
//                Session["IsAuthenticated"] = true;
//                Session["Username"] = username;
//                Response.Redirect("Admin.aspx");
//            }
//            else
//            {
//                lblMessage.Text = "Invalid username or password";
//                lblMessage.Visible = true;
//            }
//        }

//        private bool AuthenticateUser(string username, string password)
//        {
//            string connectionString = WebConfigurationManager.ConnectionStrings["portfolioDB"].ConnectionString;

//            using (SqlConnection conn = new SqlConnection(connectionString))
//            {
//                try
//                {
//                    // In a real application, you should hash the password and compare hashes
//                    SqlCommand cmd = new SqlCommand("SELECT UserId FROM Users WHERE Username = @Username AND PasswordHash = @Password AND IsActive = 1", conn);
//                    cmd.Parameters.AddWithValue("@Username", username);
//                    cmd.Parameters.AddWithValue("@Password", password); // In real app, use hashed password

//                    conn.Open();
//                    object result = cmd.ExecuteScalar();

//                    return result != null;
//                }
//                catch (Exception ex)
//                {
//                    // Log error
//                    System.Diagnostics.Debug.WriteLine("Authentication error: " + ex.Message);
//                    return false;
//                }
//            }
//        }
//    }
//}



using System;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;

namespace MyPortfolio
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Redirect to admin if already logged in
            if (Session["IsAuthenticated"] != null && (bool)Session["IsAuthenticated"])
            {
                Response.Redirect("Admin.aspx");
            }

            // Check for remember me cookie
            if (!IsPostBack)
            {
                if (Request.Cookies["userInfo"] != null)
                {
                    txtUsername.Text = Request.Cookies["userInfo"]["username"];
                    chkRememberMe.Checked = true;
                }
            }
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text;

            if (AuthenticateUser(username, password))
            {
                Session["IsAuthenticated"] = true;
                Session["Username"] = username;

                // Set remember me cookie if checked
                if (chkRememberMe.Checked)
                {
                    HttpCookie cookie = new HttpCookie("userInfo");
                    cookie["username"] = username;
                    cookie.Expires = DateTime.Now.AddDays(30); // Cookie expires in 30 days
                    Response.Cookies.Add(cookie);
                }
                else
                {
                    // Clear the cookie if remember me is not checked
                    if (Request.Cookies["userInfo"] != null)
                    {
                        HttpCookie cookie = new HttpCookie("userInfo");
                        cookie.Expires = DateTime.Now.AddDays(-1);
                        Response.Cookies.Add(cookie);
                    }
                }

                Response.Redirect("Admin.aspx");
            }
            else
            {
                lblMessage.Text = "Invalid username or password";
                lblMessage.Visible = true;
            }
        }

        private bool AuthenticateUser(string username, string password)
        {
            string connectionString = WebConfigurationManager.ConnectionStrings["portfolioDB"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    // In a real application, you should hash the password and compare hashes
                    SqlCommand cmd = new SqlCommand("SELECT UserId FROM Users WHERE Username = @Username AND PasswordHash = @Password AND IsActive = 1", conn);
                    cmd.Parameters.AddWithValue("@Username", username);
                    cmd.Parameters.AddWithValue("@Password", password); // In real app, use hashed password

                    conn.Open();
                    object result = cmd.ExecuteScalar();

                    return result != null;
                }
                catch (Exception ex)
                {
                    // Log error
                    System.Diagnostics.Debug.WriteLine("Authentication error: " + ex.Message);
                    return false;
                }
            }
        }
    }
}