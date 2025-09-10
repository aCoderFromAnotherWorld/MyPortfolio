<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="MyPortfolio.Default1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    My Portfolio
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Hero Section -->
    <section id="home" class="hero">
        <div class="container">
            <div class="hero-content">
                <h1>MD. Abu Hasanat Soykot</h1>
                <p id="typewriter"></p> <br /><br />
                <p>Exploring technology through academic projects and real-world applications, from building responsive web solutions to researching vulnerabilities and system security.</p>
                <a href="Assets/cv/CV.pdf" class="btn" target="_blank">Download CV</a>
            </div>
        </div>
    </section>

    <!-- About Section -->
    <section id="about" class="about">
        <div class="container">
            <h2 class="section-title">About Me</h2>
            <div class="about-content">
                <div class="about-text">
                <p>I'm an undergraduate student in Computer Science and Engineering at KUET with a deep passion for web development, system design, and cybersecurity. My academic journey has given me the opportunity to work on diverse projects such as chat applications, database-driven systems, and digital circuit simulations.</p>
                <p>I enjoy building responsive, user-friendly web applications using modern technologies like <strong>ASP.NET, SQL Server, JavaScript, and React</strong>. Alongside web development, I am also exploring <strong>cybersecurity</strong>, focusing on vulnerability research and exploit development.</p>
                <p>My goal is to combine software development with security expertise to create reliable, secure, and innovative digital solutions.</p>
                    <a href="#contact" class="btn">Contact Me</a>
                </div>
                <div class="about-image">
                    <%--<img src="https://placehold.co/400x500/3498db/ffffff?text=MD.+Abu+Hasanat+Soykot" alt="MD. Abu Hasanat Soykot" />--%>
                    <%--<asp:Repeater ID="rptProfile" runat="server">
                        <ItemTemplate>
                            <img src="<%# Eval("ProfilePic") %>" alt="MD. Abu Hasanat Soykot"/>
                        </ItemTemplate>
                    </asp:Repeater>--%>

                    <img src="Assets/images/profile/profileImage.jpg" alt="MD. Abu Hasanat Soykot"/>
                </div>
            </div>
        </div>
    </section>

    <!-- Skills Section -->
    <section id="skills" class="skills">
        <div class="container">
            <h2 class="section-title">My Skills</h2>
            <div class="skills-grid">
                
                <asp:Repeater ID="rptSkills" runat="server">
                    <ItemTemplate>
                        <div class="skill-item">
                            <div class="skill-icon">
                                
                                <img src="<%# Eval("IconClass") %>" alt="<%# Eval("Name") %>" 
                                     style="width: 60px; height: 60px; object-fit: cover; border-radius: 50%;" 
                                     onerror="this.src='Assets/images/icons/default-skill.png'" />
                            </div>
                            <h3><%# Eval("Name") %></h3>
                            <p><%# Eval("Description") %></p>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
            </div>
        </div>
    </section>
    <!-- Education Section -->
    <section id="education" class="education">
        <div class="container">
            <h2 class="section-title">My Education</h2>
            <div class="education-timeline">
                <asp:Repeater ID="rptEducation" runat="server">
                    <ItemTemplate>
                        <div class="education-item">
                            <div class="education-icon">
                                <i class="fas fa-graduation-cap"></i>
                            </div>
                            <div class="education-content">
                                <h3><%# Eval("Degree") %></h3>
                                <h4><%# Eval("Institution") %></h4>
                                <span class="education-date">
                                    <%# FormatDate(Eval("StartDate"), Eval("EndDate")) %>
                                </span>
                                <p><%# Eval("Description") %></p>
                                <%# !string.IsNullOrEmpty(Eval("Grade").ToString()) ? "<span class='education-grade'>Grade: " + Eval("Grade") + "</span>" : "" %>
                            </div>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
            </div>
        </div>
    </section>

    <!-- Portfolio Section -->
    <section id="portfolio" class="portfolio">
        <div class="container">
            <h2 class="section-title">My Projects</h2>
            <div class="portfolio-grid">
                <asp:Repeater ID="rptProjects" runat="server">
                    <ItemTemplate>
                        <div class="portfolio-item" data-project-id="<%# Eval("ProjectId") %>">
                            <div class="portfolio-image">
                                <img src="<%# Eval("ImageUrl", "{0}") %>" alt="<%# Eval("Title") %>" onerror="this.src='https://placehold.co/600x400/3498db/ffffff?text=Project+Image'" />
                            </div>
                            <div class="portfolio-info">
                                <h3><%# Eval("Title") %></h3>
                                <p><%# GetShortDescription(Eval("ShortDescription"), Eval("FullDescription")) %></p>
                            </div>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
            </div>
        </div>
    </section>

    <!-- Project Modal -->
    <div class="modal" id="projectModal">
        <div class="modal-content">
            <span class="close-modal" id="closeModal">&times;</span>
            <div class="modal-header">
                <h2 id="modalTitle">Project Title</h2>
            </div>
            <div class="modal-body">
                <div class="modal-image">
                    <img id="modalImage" src="" alt="Project Image" />
                </div>
                <div class="modal-details">
                    <p id="modalDescription">Project description will be loaded here...</p>
                    <div id="modalTechnologies"></div>
                    <a id="modalLink" href="#" class="btn" target="_blank">View Project</a>
                </div>
            </div>
        </div>
    </div>

    <!-- Contact Section -->
    
    <section id="contact" class="contact">
        <div class="contact-form">
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <asp:Label ID="lblContactMessage" runat="server" CssClass="alert" Visible="false"></asp:Label>
            
                    <div class="form-group">
                        <label for="txtName">Name</label>
                        <asp:TextBox ID="txtName" runat="server" CssClass="form-control" required="true"></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <label for="txtEmail">Email</label>
                        <asp:TextBox ID="txtEmail" runat="server" TextMode="Email" CssClass="form-control" required="true"></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <label for="txtSubject">Subject</label>
                        <asp:TextBox ID="txtSubject" runat="server" CssClass="form-control" required="true"></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <label for="txtMessage">Message</label>
                        <asp:TextBox ID="txtMessage" runat="server" TextMode="MultiLine" Rows="5" CssClass="form-control" required="true"></asp:TextBox>
                    </div>
                    <asp:Button ID="btnSubmit" runat="server" Text="Send Message" CssClass="btn" OnClick="btnSubmit_Click" />
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </section>


</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Scripts" runat="server">
    <script src="Assets/js/scripts.js"></script>
</asp:Content>