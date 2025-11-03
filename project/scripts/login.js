document.addEventListener('DOMContentLoaded', () => {
  const signupForm = document.getElementById('signupForm');
  const username = document.getElementById('username');
  const email = document.getElementById('email');
  const password = document.getElementById('password');
  const togglePwd = document.getElementById('togglePwd');

  // زر عرض/إخفاء كلمة المرور
  togglePwd.addEventListener('click', function () {
    const type = password.getAttribute('type') === 'password' ? 'text' : 'password';
    password.setAttribute('type', type);
    togglePwd.textContent = type === 'password' ? 'عرض' : 'إخفاء';
    password.focus();
  });

  // تحقق بسيط من النموذج
  signupForm.addEventListener('submit', function (e) {
    e.preventDefault();
    let valid = true;

    // تحقق من الحقول
    if (!username.value.trim() || username.value.trim().length < 3) {
      username.classList.add('is-invalid');
      valid = false;
    } else username.classList.remove('is-invalid');

    if (!email.value.includes('@') || !email.value.includes('.')) {
      email.classList.add('is-invalid');
      valid = false;
    } else email.classList.remove('is-invalid');

    if (!password.value || password.value.length < 6) {
      password.classList.add('is-invalid');
      valid = false;
    } else password.classList.remove('is-invalid');

    if (!valid) return;

    // إرسال البيانات 
    const payload = {
      username: username.value.trim(),
      email: email.value.trim(),
      password: password.value,
    };

    console.log('🟢 بيانات التسجيل:', payload);

    alert('تم إنشاء الحساب بنجاح! ');
    signupForm.reset();
  });
});
