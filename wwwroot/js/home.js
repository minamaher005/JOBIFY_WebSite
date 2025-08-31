// Smooth scrolling for anchor links
document.addEventListener("DOMContentLoaded", function () {
  document.querySelectorAll('a[href^="#"]').forEach((anchor) => {
    anchor.addEventListener("click", function (e) {
      e.preventDefault();
      const targetId = this.getAttribute("href");
      if (targetId === "#") return;

      const target = document.querySelector(targetId);
      if (target) {
        window.scrollTo({
          top: target.offsetTop - 70,
          behavior: "smooth",
        });
      }
    });
  });
});

// Featured jobs loading
function loadFeaturedJobs() {
  // This function could be used to dynamically load featured jobs via AJAX
  console.log("Featured jobs loaded");
}

// Category filtering
function filterJobsByCategory(categoryId) {
  console.log("Filtering jobs by category:", categoryId);
  // Actual implementation would filter jobs by category
}

// Initialize all scripts
document.addEventListener("DOMContentLoaded", function () {
  // Initialize any additional functions when the DOM is fully loaded
  loadFeaturedJobs();

  // Add event listeners for category buttons
  document.querySelectorAll("[data-category]").forEach((button) => {
    button.addEventListener("click", function () {
      const categoryId = this.getAttribute("data-category");
      filterJobsByCategory(categoryId);
    });
  });
});
