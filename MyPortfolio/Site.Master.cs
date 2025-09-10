using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MyPortfolio
{
    public partial class Site : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string currentPage = System.IO.Path.GetFileName(Request.Path).ToLower();

            // Hide nav only on login and admin pages
            if (currentPage == "login.aspx" || currentPage == "admin.aspx")
            {
                navLinks.Visible = false;   // hide navigation
            }
        }
    }
}