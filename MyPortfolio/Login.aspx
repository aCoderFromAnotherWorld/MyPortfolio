<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="MyPortfolio.Login" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Admin Login - Portfolio
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <section class="login-section">
        <div class="container">
            <div class="login-form">
                <h2 class="section-title">Admin Login</h2>
                <asp:Panel ID="pnlLogin" runat="server" DefaultButton="btnLogin">
                    <div class="form-group">
                        <label for="txtUsername">Username</label>
                        <asp:TextBox ID="txtUsername" runat="server" CssClass="form-control" required="true" />
                    </div>
                    <div class="form-group">
                        <label for="txtPassword">Password</label>
                        <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" CssClass="form-control" required="true" />
                    </div>
                    <div class="form-group">
                        <asp:CheckBox ID="chkRememberMe" runat="server" Text=" Remember me" CssClass="checkbox" />
                    </div>
                    <asp:Button ID="btnLogin" runat="server" Text="Login" CssClass="btn btn-primary" OnClick="btnLogin_Click" />
                    <asp:Label ID="lblMessage" runat="server" CssClass="error-message" Visible="false"></asp:Label>
                </asp:Panel>
            </div>
        </div>
    </section>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="Scripts" runat="server">
    <style>
        .login-section {
            padding: 120px 0;
            min-height: 100vh;
            display: flex;
            align-items: center;
        }
        
        .login-form {
            max-width: 400px;
            margin: 0 auto;
            background: var(--card-bg);
            padding: 40px;
            border-radius: 10px;
            box-shadow: var(--shadow);
        }
        
        .error-message {
            display: block;
            color: #e74c3c;
            margin-top: 20px;
            text-align: center;
        }
    </style>
    <script>

    </script>
</asp:Content>