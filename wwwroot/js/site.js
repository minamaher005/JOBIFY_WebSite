// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Job Portal Application - Main JavaScript
"use strict";

// Define a namespace for our app
const JobPortal = {
  // Initialize the application
  init: function () {
    this.initTooltips();
    this.initNavbarEffects();
    this.initDropdownBehavior();
    this.initFormValidation();
    this.initAccessibility();
  },

  // Initialize Bootstrap tooltips
  initTooltips: function () {
    const tooltipTriggerList = [].slice.call(
      document.querySelectorAll('[data-bs-toggle="tooltip"]')
    );
    tooltipTriggerList.map(function (tooltipTriggerEl) {
      return new bootstrap.Tooltip(tooltipTriggerEl);
    });
  },

  // Handle navbar scroll effects
  initNavbarEffects: function () {
    const navbar = document.querySelector(".navbar");
    if (!navbar) return;

    window.addEventListener("scroll", function () {
      if (window.scrollY > 10) {
        navbar.classList.add("shadow-sm");
      } else {
        navbar.classList.remove("shadow-sm");
      }
    });
  },

  // Improve dropdown menu behavior
  initDropdownBehavior: function () {
    document.addEventListener("click", function (e) {
      const dropdownMenus = document.querySelectorAll(".dropdown-menu.show");
      const dropdownToggles = document.querySelectorAll(".dropdown-toggle");

      // Check if the click was outside of any dropdown
      let clickedInside = false;

      dropdownMenus.forEach((menu) => {
        if (menu.contains(e.target)) clickedInside = true;
      });

      dropdownToggles.forEach((toggle) => {
        if (toggle.contains(e.target)) clickedInside = true;
      });

      // If click was outside, close all dropdowns
      if (!clickedInside) {
        dropdownMenus.forEach((menu) => {
          menu.classList.remove("show");
        });
      }
    });
  },

  // Enhance form validation
  initFormValidation: function () {
    const forms = document.querySelectorAll(".needs-validation");

    if (forms.length === 0) return;

    Array.from(forms).forEach((form) => {
      form.addEventListener(
        "submit",
        (event) => {
          if (!form.checkValidity()) {
            event.preventDefault();
            event.stopPropagation();
          }

          form.classList.add("was-validated");
        },
        false
      );
    });
  },

  // Improve accessibility
  initAccessibility: function () {
    // Make skiplinks visible on focus
    const skipLink = document.querySelector(".skip-link");
    if (skipLink) {
      skipLink.addEventListener("focus", function () {
        this.style.transform = "translateY(0)";
      });

      skipLink.addEventListener("blur", function () {
        this.style.transform = "translateY(-100%)";
      });
    }

    // Ensure proper focus styles
    document
      .querySelectorAll(
        'a, button, input, select, textarea, [tabindex]:not([tabindex="-1"])'
      )
      .forEach((el) => {
        el.classList.add("focus-visible");
      });
  },
};

// Initialize the application when the DOM is ready
document.addEventListener("DOMContentLoaded", function () {
  JobPortal.init();
});
