window.addEventListener("scroll", function () {
  const navbar = document.querySelector(".navbar");
  if (window.scrollY > 50) {
    navbar.classList.add("scrolled");
  } else {
    navbar.classList.remove("scrolled");
  }
});
document.addEventListener("DOMContentLoaded", function () {
  const services = document.querySelectorAll(".service-box");

  function showServicesOnScroll() {
    const triggerBottom = window.innerHeight * 0.85;

    services.forEach(box => {
      const boxTop = box.getBoundingClientRect().top;

      if (boxTop < triggerBottom) {
        box.classList.add("show");
      } else {
        box.classList.remove("show");
      }
    });
  }

  window.addEventListener("scroll", showServicesOnScroll);
  showServicesOnScroll();
});
