// Dark/Light Mode Toggle
const themeToggle = document.getElementById("themeToggle");
const themeIcon = themeToggle.querySelector("i");

// Check for saved theme preference
if (localStorage.getItem("theme") === "dark") {
    document.body.classList.add("dark-mode");
    themeIcon.classList.remove("fa-moon");
    themeIcon.classList.add("fa-sun");
}

themeToggle.addEventListener("click", () => {
    document.body.classList.toggle("dark-mode");

    if (document.body.classList.contains("dark-mode")) {
        localStorage.setItem("theme", "dark");
        themeIcon.classList.remove("fa-moon");
        themeIcon.classList.add("fa-sun");
    } else {
        localStorage.setItem("theme", "light");
        themeIcon.classList.remove("fa-sun");
        themeIcon.classList.add("fa-moon");
    }
});

// Smooth scrolling for navigation links
document.querySelectorAll('a[href^="#"]').forEach(anchor => {
    anchor.addEventListener('click', function (e) {
        e.preventDefault();

        const targetId = this.getAttribute('href');
        if (targetId === '#') return;

        const targetElement = document.querySelector(targetId);
        if (targetElement) {
            window.scrollTo({
                top: targetElement.offsetTop - 70,
                behavior: 'smooth'
            });
        }
    });
});

// Form Submission
const contactForm = document.getElementById('contactForm');
if (contactForm) {
    contactForm.addEventListener('submit', function (e) {
        e.preventDefault();

        // Get form data
        const formData = new FormData(this);
        const name = formData.get('name');
        const email = formData.get('email');
        const subject = formData.get('subject');
        const message = formData.get('message');

        // Send data to server using AJAX
        submitContactForm(name, email, subject, message);
    });
} else {
    console.error("contactForm element not found");
}

// AJAX function to submit contact form
function submitContactForm(name, email, subject, message) {
    // Use fetch API to submit the form
    fetch('Default.aspx/SaveContact', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify({
            name: name,
            email: email,
            subject: subject,
            message: message
        })
    })
        .then(response => response.json())
        .then(data => {
            if (data.d && data.d.success) {
                alert('Thank you for your message! I will get back to you soon.');
                contactForm.reset();
            } else {
                alert('There was an error sending your message. Please try again.');
            }
        })
        .catch(error => {
            console.error('Error:', error);
            alert('There was an error sending your message. Please try again.');
        });
}


// Project Modal functionality
const modal = document.getElementById('projectModal');
const closeModal = document.getElementById('closeModal');
const modalTitle = document.getElementById('modalTitle');
const modalImage = document.getElementById('modalImage');
const modalDescription = document.getElementById('modalDescription');
const modalTechnologies = document.getElementById('modalTechnologies');
const modalLink = document.getElementById('modalLink');

// Check if modal elements exist
if (!modal || !closeModal || !modalTitle || !modalImage || !modalDescription || !modalTechnologies || !modalLink) {
    console.error("One or more modal elements not found");
} else {
    // Open modal when portfolio item is clicked
    document.addEventListener('click', (e) => {
        const portfolioItem = e.target.closest('.portfolio-item');
        if (portfolioItem) {
            const projectId = portfolioItem.getAttribute('data-project-id');

            // Fetch project details from server
            fetchProjectDetails(projectId);
        }
    });

    // Close modal
    closeModal.addEventListener('click', () => {
        modal.style.display = 'none';
        document.body.style.overflow = 'auto';
    });

    // Close modal when clicking outside
    window.addEventListener('click', (e) => {
        if (e.target === modal) {
            modal.style.display = 'none';
            document.body.style.overflow = 'auto';
        }
    });
}

// Function to fetch project details from server
function fetchProjectDetails(projectId) {
    // In a real implementation, you would fetch this from the server
    // For now, we'll use a simple approach
    fetch('Default.aspx/GetProjectDetails', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify({
            projectId: projectId
        })
    })
        .then(response => response.json())
        .then(data => {
            if (data.d) {
                const project = data.d;
                showProjectModal(project);
            } else {
                console.error('Error fetching project details');
            }
        })
        .catch(error => {
            console.error('Error:', error);
        });
}

// Function to show project modal
function showProjectModal(project) {
    modalTitle.textContent = project.Title;
    modalImage.src = project.ImageUrl;
    modalImage.alt = project.Title;
    modalDescription.textContent = project.FullDescription;

    // Clear and add technologies
    modalTechnologies.innerHTML = '<h4>Technologies Used:</h4>';
    const techList = document.createElement('ul');

    if (project.Technologies) {
        const technologies = project.Technologies.split(',');
        technologies.forEach(tech => {
            const listItem = document.createElement('li');
            listItem.textContent = tech.trim();
            techList.appendChild(listItem);
        });
    }

    modalTechnologies.appendChild(techList);

    modalLink.href = project.ProjectUrl || '#';
    modalLink.textContent = project.ProjectUrl ? 'View Project' : 'Project Link Not Available';


    modalLink.onclick = function () {
        //location.href = project.ProjectUrl;
        ////modalLink.setAttribute('target', '_blank');
        window.open(project.ProjectUrl, '_blank');
    }



    modal.style.display = 'block';
    document.body.style.overflow = 'hidden';
}



// Hamburger menu toggle
const hamburger = document.getElementById("hamburger");
const navLinks = document.getElementById("navLinks");

if (hamburger && navLinks) {
    hamburger.addEventListener("click", () => {
        navLinks.classList.toggle("active");

        // Change icon when toggled
        const icon = hamburger.querySelector("i");
        if (navLinks.classList.contains("active")) {
            icon.classList.remove("fa-bars");
            icon.classList.add("fa-times"); // close icon
        } else {
            icon.classList.remove("fa-times");
            icon.classList.add("fa-bars"); // hamburger icon
        }
    });

    // Close menu when link is clicked (for smoother UX)
    navLinks.querySelectorAll("a").forEach(link => {
        link.addEventListener("click", () => {
            navLinks.classList.remove("active");
            const icon = hamburger.querySelector("i");
            icon.classList.remove("fa-times");
            icon.classList.add("fa-bars");
        });
    });
}
