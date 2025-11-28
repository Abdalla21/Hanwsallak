// Login Page Script
document.addEventListener('DOMContentLoaded', () => {
  const loginForm = document.getElementById('loginForm');
  const email = document.getElementById('email');
  const password = document.getElementById('password');
  const togglePwd = document.getElementById('togglePwd');
  const alertMessage = document.getElementById('alertMessage');
  const loginBtn = document.getElementById('loginBtn');
  const loginBtnText = document.getElementById('loginBtnText');
  const loginBtnSpinner = document.getElementById('loginBtnSpinner');
  const forgotPasswordLink = document.getElementById('forgotPasswordLink');

  // Check if user is already logged in
  if (getAuthToken()) {
    // Redirect to appropriate dashboard based on user role or default dashboard
    window.location.href = 'dashboard/Customer-dashboard.html';
  }

  // Toggle password visibility
  togglePwd.addEventListener('click', function () {
    const type = password.getAttribute('type') === 'password' ? 'text' : 'password';
    password.setAttribute('type', type);
    const icon = togglePwd.querySelector('i');
    if (icon) {
      icon.classList.toggle('fa-eye');
      icon.classList.toggle('fa-eye-slash');
    }
    password.focus();
  });

  // Show alert message
  function showAlert(message, type = 'danger') {
    alertMessage.textContent = message;
    alertMessage.className = `alert alert-${type} d-block`;
    alertMessage.scrollIntoView({ behavior: 'smooth', block: 'nearest' });
    setTimeout(() => {
      alertMessage.classList.add('d-none');
    }, 5000);
  }

  // Set loading state
  function setLoading(loading) {
    if (loading) {
      loginBtn.disabled = true;
      loginBtnText.textContent = i18n.t('login.loggingIn');
      loginBtnSpinner.classList.remove('d-none');
    } else {
      loginBtn.disabled = false;
      loginBtnText.textContent = i18n.t('login.loginButton');
      loginBtnSpinner.classList.add('d-none');
    }
  }

  // Handle form submission
  loginForm.addEventListener('submit', async function (e) {
    e.preventDefault();
    
    // Reset validation
    email.classList.remove('is-invalid');
    password.classList.remove('is-invalid');

    let valid = true;

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

    if (!valid) {
      showAlert('يرجى إدخال بيانات صحيحة', 'danger');
      return;
    }

    setLoading(true);

    try {
      // Call login API
      const response = await AuthAPI.login(email.value.trim(), password.value);
      
      // Store user info if provided
      if (response.user) {
        localStorage.setItem('userInfo', JSON.stringify(response.user));
      }

      showAlert(i18n.t('login.loginSuccess'), 'success');
      
      // Redirect after short delay
      setTimeout(() => {
        // Check user role and redirect accordingly
        const userInfo = response.user || JSON.parse(localStorage.getItem('userInfo') || '{}');
        // Check if user is a Driver/Traveler
        if (userInfo && userInfo.roles && (userInfo.roles.includes('Driver') || userInfo.roles.includes('Traveler'))) {
          window.location.href = 'dashboard/driver-dashboard.html';
        } else {
          // Default to customer dashboard for senders/customers
          window.location.href = 'dashboard/Customer-dashboard.html';
        }
      }, 1500);

    } catch (error) {
      console.error('Login error:', error);
      let errorMessage = error.message || i18n.t('login.loginError');
      // Map common error messages
      if (errorMessage.includes('Invalid') || errorMessage.includes('invalid')) {
        errorMessage = i18n.t('login.invalidCredentials');
      } else if (errorMessage.includes('locked')) {
        errorMessage = i18n.t('login.accountLocked');
      } else if (errorMessage.includes('confirmed') || errorMessage.includes('email')) {
        errorMessage = i18n.t('login.emailNotConfirmed');
      }
      showAlert(errorMessage, 'danger');
      setLoading(false);
    }
  });

  // Handle forgot password
  forgotPasswordLink.addEventListener('click', async function(e) {
    e.preventDefault();
    const emailValue = email.value.trim();
    
    if (!emailValue) {
      showAlert('يرجى إدخال البريد الإلكتروني أولاً', 'warning');
      email.focus();
      return;
    }

    try {
      await AuthAPI.forgotPassword(emailValue);
      showAlert('تم إرسال رابط إعادة تعيين كلمة المرور إلى بريدك الإلكتروني', 'success');
    } catch (error) {
      console.error('Forgot password error:', error);
      showAlert('حدث خطأ. يرجى المحاولة مرة أخرى', 'danger');
    }
  });
});
