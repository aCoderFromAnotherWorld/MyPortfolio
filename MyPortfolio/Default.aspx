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
                <p>Full Stack Developer & UI/UX Designer</p>
                <p>I create beautiful, functional websites and applications that help businesses grow.</p>
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
                    <p>I'm a passionate web developer and designer with over 5 years of experience creating digital solutions for businesses of all sizes. My journey in tech started when I built my first website at the age of 15, and I've been hooked ever since.</p>
                    <p>I specialize in creating responsive, user-friendly websites and applications using modern technologies like HTML5, CSS3, JavaScript, and ASP.NET. I believe in clean code, intuitive design, and delivering projects that exceed client expectations.</p>
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
                <%--<asp:Repeater ID="rptSkills" runat="server">
                    <ItemTemplate>
                        <div class="skill-item">
                            <div class="skill-icon">
                                <i class="fas <%# Eval("IconClass") %>"></i>
                            </div>
                            <h3><%# Eval("Name") %></h3>
                            <p><%# Eval("Description") %></p>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>--%>

                <!-- Update the Skills section in Default.aspx -->
                <asp:Repeater ID="rptSkills" runat="server">
                    <ItemTemplate>
                        <div class="skill-item">
                            <div class="skill-icon">
                                <!-- Replace font-awesome icon with image -->
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
        <div class="container">
            <h2 class="section-title">Get In Touch</h2>
            <div class="contact-form">
                <form id="contactForm">
                    <div class="form-group">
                        <label for="name">Name</label>
                        <input type="text" id="name" name="name" required />
                    </div>
                    <div class="form-group">
                        <label for="email">Email</label>
                        <input type="email" id="email" name="email" required />
                    </div>
                    <div class="form-group">
                        <label for="subject">Subject</label>
                        <input type="text" id="subject" name="subject" required />
                    </div>
                    <div class="form-group">
                        <label for="message">Message</label>
                        <textarea id="message" name="message" required></textarea>
                    </div>
                    <button type="submit" class="btn">Send Message</button>
                    <%--<asp:Button ID="btn" runat="server" Text="Send Message" OnClick="" />--%>
                </form>
            </div>
        </div>
    </section>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Scripts" runat="server">
    <script src="Assets/js/scripts.js"></script>
</asp:Content>