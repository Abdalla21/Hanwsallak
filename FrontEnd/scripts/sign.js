// Signup Page Script
document.addEventListener('DOMContentLoaded', () => {
  const driverBtn = document.getElementById('driverBtn');
  const senderBtn = document.getElementById('senderBtn');
  const signupForm = document.getElementById('signupForm');
  const fullName = document.getElementById('fullName');
  const email = document.getElementById('email');
  const phoneNumber = document.getElementById('phoneNumber');
  const password = document.getElementById('password');
  const confirmPassword = document.getElementById('confirmPassword');

  // Check if user is already logged in (wait for api.js to load)
  function checkAuth() {
    if (typeof getAuthToken === 'function' && getAuthToken()) {
      window.location.href = 'dashboard/Customer-dashboard.html';
    } else if (typeof getAuthToken === 'undefined') {
      setTimeout(checkAuth, 100);
    }
  }
  checkAuth();

  // Check URL parameters for role
  const urlParams = new URLSearchParams(window.location.search);
  const roleParam = urlParams.get('role');
  
  // Auto-select role if provided in URL
  if (roleParam === 'driver' && driverBtn) {
    driverBtn.classList.add('active');
    if (senderBtn) senderBtn.classList.remove('active');
    if (signupForm) {
      signupForm.classList.remove('d-none');
      signupForm.dataset.role = 'driver';
    }
  } else if (roleParam === 'sender' && senderBtn) {
    senderBtn.classList.add('active');
    if (driverBtn) driverBtn.classList.remove('active');
    if (signupForm) {
      signupForm.classList.remove('d-none');
      signupForm.dataset.role = 'sender';
    }
  }

  // Role selection
  if (driverBtn) {
    driverBtn.addEventListener('click', () => {
      driverBtn.classList.add('active');
      if (senderBtn) senderBtn.classList.remove('active');
      if (signupForm) {
        signupForm.classList.remove('d-none');
        signupForm.dataset.role = 'driver';
      }
    });
  }

  if (senderBtn) {
    senderBtn.addEventListener('click', () => {
      senderBtn.classList.add('active');
      if (driverBtn) driverBtn.classList.remove('active');
      if (signupForm) {
        signupForm.classList.remove('d-none');
        signupForm.dataset.role = 'sender';
      }
    });
  }

  // Helper function to show alert message
  function showAlert(message, type = 'danger', errors = []) {
    const alertDiv = document.getElementById('signupAlert');
    const alertMessage = document.getElementById('alertMessage');
    const errorList = document.getElementById('errorList');
    
    if (!alertDiv || !alertMessage) return;
    
    // Remove all alert classes
    alertDiv.classList.remove('alert-success', 'alert-danger', 'alert-warning', 'alert-info', 'd-none');
    // Add the appropriate class
    alertDiv.classList.add(`alert-${type}`);
    
    // Set main message
    alertMessage.textContent = message;
    
    // Clear and populate error list if there are errors
    if (errorList) {
      errorList.innerHTML = '';
      if (errors && errors.length > 0) {
        errors.forEach(error => {
          const li = document.createElement('li');
          li.textContent = error;
          errorList.appendChild(li);
        });
      }
    }
    
    // Scroll to alert
    alertDiv.scrollIntoView({ behavior: 'smooth', block: 'nearest' });
  }

  // Helper function to hide alert
  function hideAlert() {
    const alertDiv = document.getElementById('signupAlert');
    if (alertDiv) {
      alertDiv.classList.add('d-none');
    }
  }

  // Form validation and submission
  signupForm.addEventListener('submit', async (e) => {
    e.preventDefault();

    // Hide previous alerts
    hideAlert();

    // Reset validation
    [fullName, email, password, confirmPassword].forEach(field => {
      if (field) field.classList.remove('is-invalid');
    });

    let valid = true;

    // Validate full name
    if (!fullName.value.trim() || fullName.value.trim().length < 3) {
      fullName.classList.add('is-invalid');
      valid = false;
    }

    // Validate email
    const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    if (!email.value.trim() || !emailRegex.test(email.value.trim())) {
      email.classList.add('is-invalid');
      valid = false;
    }

    // Validate password
    if (!password.value || password.value.length < 6) {
      password.classList.add('is-invalid');
      valid = false;
    }

    // Validate confirm password
    if (!confirmPassword.value || password.value !== confirmPassword.value) {
      confirmPassword.classList.add('is-invalid');
      valid = false;
    }

    if (!valid) {
      showAlert(i18n.t('signup.validationError'), 'warning');
      return;
    }

    // Check if role is selected
    if (!signupForm.dataset.role) {
      showAlert(i18n.t('signup.selectRoleError'), 'warning');
      return;
    }

    try {
      // Prepare registration data
      const registerData = {
        fullName: fullName.value.trim(),
        email: email.value.trim(),
        password: password.value,
        confirmPassword: confirmPassword.value,
        phoneNumber: phoneNumber ? phoneNumber.value.trim() : ''
      };

      // Show loading
      const submitBtn = signupForm.querySelector('button[type="submit"]');
      const originalText = submitBtn.textContent;
      submitBtn.disabled = true;
      submitBtn.innerHTML = `<span class="spinner-border spinner-border-sm me-2"></span>${i18n.t('signup.registering')}`;

      // Call registration API
      const response = await AuthAPI.register(registerData);

      // Show success message
      const roleText = signupForm.dataset.role === 'driver' ? i18n.t('signup.asDriver') : i18n.t('signup.asSender');
      const successMsg = i18n.t('signup.successMessage', { name: fullName.value.trim() });
      showAlert(
        `🎉 ${i18n.t('signup.success')} ${roleText}! ${successMsg}`,
        'success'
      );
      
      // Reset form
      signupForm.reset();
      driverBtn.classList.remove('active');
      senderBtn.classList.remove('active');
      signupForm.classList.add('d-none');

      // Redirect to login after 3 seconds
      setTimeout(() => {
        window.location.href = 'login.html';
      }, 3000);

    } catch (error) {
      console.error('Registration error:', error);
      
      // Extract error message from API response
      let errorMessage = i18n.t('signup.error');
      let errorList = [];
      
      // Try to parse error response
      try {
        // Check if error has data property (from our API wrapper)
        if (error.data) {
          const errorData = error.data;
          if (errorData.message) {
            errorMessage = errorData.message;
          }
          // Check for validation errors array
          if (errorData.errors && Array.isArray(errorData.errors)) {
            errorList = errorData.errors;
          } else if (errorData.errors && typeof errorData.errors === 'object') {
            // Handle object with field-specific errors (e.g., from FluentValidation)
            errorList = Object.values(errorData.errors).flat();
          }
        } 
        // Check if error has a response property (from fetch)
        else if (error.response) {
          try {
            const errorData = await error.response.json();
            if (errorData.message) {
              errorMessage = errorData.message;
            }
            if (errorData.errors && Array.isArray(errorData.errors)) {
              errorList = errorData.errors;
            } else if (errorData.errors && typeof errorData.errors === 'object') {
              errorList = Object.values(errorData.errors).flat();
            }
          } catch (parseError) {
            console.error('Error parsing response:', parseError);
          }
        } 
        // Use error message directly
        else if (error.message) {
          errorMessage = error.message;
        }
      } catch (parseError) {
        console.error('Error parsing error:', parseError);
        // If error message is a string, use it directly
        if (typeof error === 'string') {
          errorMessage = error;
        } else if (error.message) {
          errorMessage = error.message;
        }
      }
      
      // Show error with details
      showAlert(errorMessage, 'danger', errorList);
      
      // Re-enable button
      const submitBtn = signupForm.querySelector('button[type="submit"]');
      submitBtn.disabled = false;
      submitBtn.textContent = i18n.t('signup.registerButton');
    }
  });
});
