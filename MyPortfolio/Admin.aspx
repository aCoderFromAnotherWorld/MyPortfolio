<%@ Page Title="Admin" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Admin.aspx.cs" Inherits="MyPortfolio.Admin" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Admin Dashboard - Portfolio
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="admin-container">
        <div class="admin-header">
            <h2>Admin Dashboard</h2>
            <asp:Button ID="btnLogout" runat="server" Text="Logout" CssClass="btn btn-secondary" OnClick="btnLogout_Click" />
        </div>
        
        <!-- Fixed: Added type="button" to prevent form submission -->
        <div class="admin-tabs">
            <button class="tab-button active" data-tab="skills" type="button">Skills</button>
            <button class="tab-button" data-tab="projects" type="button">Projects</button>
            <button class="tab-button" data-tab="education" type="button">Education</button>
            <button class="tab-button" data-tab="profile" type="button">Profile</button>
        </div>
        
        <div class="tab-content">
            <!-- Skills Tab -->
            <div id="skills-tab" class="tab-pane active">
                <h3>Manage Skills</h3>
                <div class="form-panel">
                    <h4>Add New Skill</h4>
                    <div class="form-group">
                        <label>Skill Name</label>
                        <asp:TextBox ID="txtSkillName" runat="server" CssClass="form-control"></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <label>Description</label>
                        <asp:TextBox ID="txtSkillDescription" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="3"></asp:TextBox>
                    </div>
                    <!-- Fixed: Changed from icon input to file upload -->
                    <div class="form-group">
                        <label>Skill Image</label>
                        <asp:FileUpload ID="skillImage" runat="server" CssClass="form-control" accept="image/*" />
                        <small class="form-text">Upload JPG, PNG, or GIF images only</small>
                    </div>
                    <div class="form-group">
                        <label>Display Order</label>
                        <asp:TextBox ID="txtSkillOrder" runat="server" CssClass="form-control" TextMode="Number"></asp:TextBox>
                    </div>
                    <asp:Button ID="btnAddSkill" runat="server" Text="Add Skill" CssClass="btn btn-primary" OnClick="btnAddSkill_Click" />
                </div>
                
                <div class="data-panel">
                    <h4>Existing Skills</h4>
                    <asp:GridView ID="gvSkills" runat="server" AutoGenerateColumns="False" DataKeyNames="SkillId"
                        OnRowEditing="gvSkills_RowEditing" OnRowUpdating="gvSkills_RowUpdating" OnRowCancelingEdit="gvSkills_RowCancelingEdit"
                        OnRowDeleting="gvSkills_RowDeleting" CssClass="admin-table">
                        <Columns>
                            <asp:BoundField DataField="SkillId" HeaderText="ID" ReadOnly="True" />
                            <asp:TemplateField HeaderText="Name">
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtEditName" runat="server" Text='<%# Bind("Name") %>' CssClass="form-control"></asp:TextBox>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblName" runat="server" Text='<%# Bind("Name") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            
                            <asp:TemplateField HeaderText="Image">
                                <EditItemTemplate>
                                    <div style="display:flex; flex-direction:column; align-items:center; justify-content:center; height:100%; text-align:center;">
                                    <img src='<%# Eval("IconClass") %>' alt="Current" style="width: 50px; height: 50px; object-fit: cover;" 
                                         onerror="this.src='Assets/images/icons/default-skill.png'" />
                                    <asp:FileUpload ID="fuEditImage" runat="server" CssClass="form-control" accept="image/*" />
                                    <asp:HiddenField ID="hfCurrentImage" runat="server" Value='<%# Bind("IconClass") %>' />
                                    </div>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <img src='<%# Eval("IconClass") %>' alt='<%# Eval("Name") %>' 
                                         style="width: 50px; height: 50px; object-fit: cover; border-radius: 50%;" 
                                         onerror="this.src='Assets/images/icons/default-skill.png'" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Order">
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtEditOrder" runat="server" Text='<%# Bind("DisplayOrder") %>' TextMode="Number" CssClass="form-control"></asp:TextBox>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblOrder" runat="server" Text='<%# Bind("DisplayOrder") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:CheckBoxField DataField="IsActive" HeaderText="Active" />
                            <asp:CommandField ShowEditButton="True" ButtonType="Button" EditText="Edit" UpdateText="Update" CancelText="Cancel"/>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:Button ID="btnDelete" runat="server" Text="Delete" CommandName="Delete" CssClass="btn btn-danger" 
                                        OnClientClick="return confirm('Are you sure you want to delete this skill?');" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
            
            <!-- Projects Tab -->
            <div id="projects-tab" class="tab-pane">
                <h3>Manage Projects</h3>
                <div class="form-panel">
                    <h4>Add New Project</h4>
                    <div class="form-group">
                        <label>Project Title</label>
                        <asp:TextBox ID="txtProjectTitle" runat="server" CssClass="form-control"></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <label>Short Description</label>
                        <asp:TextBox ID="txtProjectShortDesc" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="2"></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <label>Full Description</label>
                        <asp:TextBox ID="txtProjectFullDesc" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="5"></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <label>Technologies (comma separated)</label>
                        <asp:TextBox ID="txtProjectTech" runat="server" CssClass="form-control" placeholder="ASP.NET, C#, SQL Server"></asp:TextBox>
                    </div>
                    <!-- Fixed: Changed from URL input to file upload -->
                    <div class="form-group">
                        <label>Project Image</label>
                        <asp:FileUpload ID="projectImageUpload" runat="server" CssClass="form-control" accept="image/*" />
                        <small class="form-text">Upload JPG, PNG, or GIF images only</small>
                    </div>
                    <div class="form-group">
                        <label>Project URL</label>
                        <asp:TextBox ID="txtProjectUrl" runat="server" CssClass="form-control"></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <label>Display Order</label>
                        <asp:TextBox ID="txtProjectOrder" runat="server" CssClass="form-control" TextMode="Number"></asp:TextBox>
                    </div>
                    <asp:Button ID="btnAddProject" runat="server" Text="Add Project" CssClass="btn btn-primary" OnClick="btnAddProject_Click" />
                </div>
                
                <div class="data-panel">
                    <h4>Existing Projects</h4>
                    <asp:GridView ID="gvProjects" runat="server" AutoGenerateColumns="False" DataKeyNames="ProjectId"
                        OnRowEditing="gvProjects_RowEditing" OnRowUpdating="gvProjects_RowUpdating" OnRowCancelingEdit="gvProjects_RowCancelingEdit"
                        OnRowDeleting="gvProjects_RowDeleting" CssClass="admin-table">
                        <Columns>
                            <asp:BoundField DataField="ProjectId" HeaderText="ID" ReadOnly="True" />
                            <asp:TemplateField HeaderText="Title">
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtEditTitle" runat="server" Text='<%# Bind("Title") %>' CssClass="form-control"></asp:TextBox>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblTitle" runat="server" Text='<%# Bind("Title") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Image">
                                <EditItemTemplate>
                                    <div style="display:flex; flex-direction:column; align-items:center; justify-content:center; height:100%; text-align:center;">
                                    <img src='<%# Eval("ImageUrl") %>' alt="Current" style="width: 80px; height: 50px; object-fit: cover;" 
                                         onerror="this.src='Assets/images/default-project.png'" />
                                    <asp:FileUpload ID="fuEditProjectImage" runat="server" CssClass="form-control" accept="image/*" />
                                    </div>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Image ID="imgProject" runat="server" ImageUrl='<%# Eval("ImageUrl") %>' 
                                               Height="50" Width="80" style="object-fit: cover;" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Order">
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtEditOrder" runat="server" Text='<%# Bind("DisplayOrder") %>' TextMode="Number" CssClass="form-control"></asp:TextBox>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblOrder" runat="server" Text='<%# Bind("DisplayOrder") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:CheckBoxField DataField="IsActive" HeaderText="Active" />
                            <asp:CommandField ShowEditButton="True" ButtonType="Button" EditText="Edit" UpdateText="Update" CancelText="Cancel" />
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:Button ID="btnDelete" runat="server" Text="Delete" CommandName="Delete" CssClass="btn btn-danger" 
                                        OnClientClick="return confirm('Are you sure you want to delete this project?');" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
            
            <!-- Education Tab -->
            <div id="education-tab" class="tab-pane">
                <h3>Manage Education</h3>
                <div class="form-panel">
                    <h4>Add New Education</h4>
                    <div class="form-group">
                        <label>Institution</label>
                        <asp:TextBox ID="txtInstitution" runat="server" CssClass="form-control"></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <label>Degree</label>
                        <asp:TextBox ID="txtDegree" runat="server" CssClass="form-control"></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <label>Field of Study</label>
                        <asp:TextBox ID="txtFieldOfStudy" runat="server" CssClass="form-control"></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <label>Start Date</label>
                        <asp:TextBox ID="txtStartDate" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <label>End Date (leave empty if current)</label>
                        <asp:TextBox ID="txtEndDate" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <label>Description</label>
                        <asp:TextBox ID="txtEduDescription" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="3"></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <label>Grade</label>
                        <asp:TextBox ID="txtGrade" runat="server" CssClass="form-control"></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <label>Display Order</label>
                        <asp:TextBox ID="txtEduOrder" runat="server" CssClass="form-control" TextMode="Number"></asp:TextBox>
                    </div>
                    <asp:Button ID="btnAddEducation" runat="server" Text="Add Education" CssClass="btn btn-primary" OnClick="btnAddEducation_Click" />
                </div>
    
                <div class="data-panel">
                    <h4>Existing Education</h4>
                    <asp:GridView ID="gvEducation" runat="server" AutoGenerateColumns="False" DataKeyNames="EducationId"
                        OnRowEditing="gvEducation_RowEditing" OnRowUpdating="gvEducation_RowUpdating" OnRowCancelingEdit="gvEducation_RowCancelingEdit"
                        OnRowDeleting="gvEducation_RowDeleting" CssClass="admin-table">
                        <Columns>
                            <asp:BoundField DataField="EducationId" HeaderText="ID" ReadOnly="True" />
                            <asp:TemplateField HeaderText="Institution">
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtEditInstitution" runat="server" Text='<%# Bind("Institution") %>' CssClass="form-control"></asp:TextBox>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblInstitution" runat="server" Text='<%# Bind("Institution") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Degree">
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtEditDegree" runat="server" Text='<%# Bind("Degree") %>' CssClass="form-control"></asp:TextBox>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblDegree" runat="server" Text='<%# Bind("Degree") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Period">
                                <EditItemTemplate>
                                    <div class="form-group">
                                        <label>Start Date</label>
                                        <asp:TextBox ID="txtEditStartDate" runat="server" Text='<%# Bind("StartDate", "{0:yyyy-MM-dd}") %>' TextMode="Date" CssClass="form-control"></asp:TextBox>
                                    </div>
                                    <div class="form-group">
                                        <label>End Date</label>
                                        <asp:TextBox ID="txtEditEndDate" runat="server" Text='<%# Eval("EndDate") != DBNull.Value ? Convert.ToDateTime(Eval("EndDate")).ToString("yyyy-MM-dd") : "" %>' TextMode="Date" CssClass="form-control"></asp:TextBox>
                                    </div>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <%# FormatEducationDate(Eval("StartDate"), Eval("EndDate")) %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Order">
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtEditOrder" runat="server" Text='<%# Bind("DisplayOrder") %>' TextMode="Number" CssClass="form-control"></asp:TextBox>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblOrder" runat="server" Text='<%# Bind("DisplayOrder") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:CheckBoxField DataField="IsActive" HeaderText="Active" />
                            <asp:CommandField ShowEditButton="True" ButtonType="Button" EditText="Edit" UpdateText="Update" CancelText="Cancel" />
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:Button ID="btnDelete" runat="server" Text="Delete" CommandName="Delete" CssClass="btn btn-danger" 
                                        OnClientClick="return confirm('Are you sure you want to delete this education entry?');" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
            
            <!-- Profile Tab -->
            <div id="profile-tab" class="tab-pane">
                <h3>Manage Profile</h3>
                <div class="form-panel">
                    <div class="form-group">
                        <label>Full Name</label>
                        <asp:TextBox ID="txtFullName" runat="server" CssClass="form-control"></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <label>Title</label>
                        <asp:TextBox ID="txtTitle" runat="server" CssClass="form-control"></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <label>Description</label>
                        <asp:TextBox ID="txtDescription" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="5"></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <label>Email</label>
                        <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" TextMode="Email"></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <label>Phone</label>
                        <asp:TextBox ID="txtPhone" runat="server" CssClass="form-control"></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <label>Location</label>
                        <asp:TextBox ID="txtLocation" runat="server" CssClass="form-control"></asp:TextBox>
                    </div>
                    <!-- Fixed: Changed from URL input to file upload -->
                    <div class="form-group">
                        <label>Profile Image</label>
                        <asp:FileUpload ID="profileImageUpload" runat="server" CssClass="form-control" accept="image/*" />
                        <small class="form-text">Upload JPG, PNG, or GIF images only</small>
                        <br />
                        <asp:Image ID="imgCurrentProfile" runat="server" Width="100px" Height="100px" 
                                   style="object-fit: cover; border-radius: 50%; margin-top: 10px;" Visible="false" />
                    </div>
                    <asp:Button ID="btnUpdateProfile" runat="server" Text="Update Profile" CssClass="btn btn-primary" OnClick="btnUpdateProfile_Click" />
                    <asp:Label ID="lblProfileMessage" runat="server" CssClass="success-message" Visible="false"></asp:Label>
                </div>
            </div>
        </div>
    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="Scripts" runat="server">
    <!-- Fixed: Added preventDefault() to prevent form submission on tab clicks -->
    <script>
        // Tab functionality
        document.addEventListener('DOMContentLoaded', function () {
            const tabButtons = document.querySelectorAll('.tab-button');
            const tabPanes = document.querySelectorAll('.tab-pane');

            tabButtons.forEach(button => {
                button.addEventListener('click', (event) => {
                    event.preventDefault(); // Prevent form submission
                    const tabId = button.getAttribute('data-tab');

                    // Remove active class from all buttons and panes
                    tabButtons.forEach(btn => btn.classList.remove('active'));
                    tabPanes.forEach(pane => pane.classList.remove('active'));

                    // Add active class to clicked button and corresponding pane
                    button.classList.add('active');
                    document.getElementById(`${tabId}-tab`).classList.add('active');
                });
            });
        });
    </script>
    <style>
    .admin-container {
        padding: 100px 0 50px;
        min-height: 100vh;
        margin: 0px 50px 0px 50px;
    }
    
    .admin-header {
        display: flex;
        justify-content: space-between;
        align-items: center;
        margin-bottom: 30px;
        padding-bottom: 15px;
        border-bottom: 1px solid #ddd;
    }
    
    .admin-tabs {
        display: flex;
        margin-bottom: 20px;
        border-bottom: 1px solid #ddd;
    }
    
    .tab-button {
        padding: 10px 20px;
        background: none;
        border: none;
        cursor: pointer;
        font-weight: 600;
        color: var(--text-color);
        border-bottom: 3px solid transparent;
    }
    
    .tab-button.active {
        border-bottom-color: var(--primary-color);
        color: var(--primary-color);
    }
    
    .tab-button:hover {
        color: var(--primary-color);
    }
    
    .tab-pane {
        display: none;
    }
    
    .tab-pane.active {
        display: block;
    }
    
    .form-panel {
        background: var(--card-bg);
        padding: 20px;
        border-radius: 10px;
        box-shadow: var(--shadow);
        margin-bottom: 30px;
    }
    
    .data-panel {
        background: var(--card-bg);
        padding: 20px;
        border-radius: 10px;
        box-shadow: var(--shadow);
        overflow-x: auto;
    }
    
    .admin-table {
        width: 100%;
        border-collapse: collapse;
    }
    
    .admin-table th, .admin-table td {
        padding: 12px;
        text-align: left;
        border-bottom: 1px solid #ddd;
    }
    
    .admin-table th {
        background-color: #f8f9fa;
        font-weight: 600;
    }
    
    .admin-table tr:hover {
        background-color: #f8f9fa;
    }
    
    .success-message {
        display: block;
        color: var(--secondary-color);
        margin-top: 20px;
        text-align: center;
    }
    
    .btn-danger {
        background-color: #e74c3c;
        border-color: #e74c3c;
    }
    
    .btn-danger:hover {
        background-color: #c0392b;
        border-color: #c0392b;
    }
    
    .btn-secondary {
        background-color: #95a5a6;
        border-color: #95a5a6;
    }
    
    .btn-secondary:hover {
        background-color: #7f8c8d;
        border-color: #7f8c8d;
    }
    
    .form-text {
        font-size: 0.875em;
        color: #6c757d;
        margin-top: 0.25rem;
    }
    
    /* Dark mode adjustments */
    .dark-mode .admin-table th {
        background-color: #2c3e50;
    }
    
    .dark-mode .admin-table tr:hover {
        background-color: #34495e;
    }
</style>
</asp:Content>