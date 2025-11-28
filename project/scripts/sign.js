document.addEventListener('DOMContentLoaded', () => {
  const driverBtn = document.getElementById('driverBtn');
  const senderBtn = document.getElementById('senderBtn');
  const signupForm = document.getElementById('signupForm');

  driverBtn.addEventListener('click', () => {
    driverBtn.classList.add('active');
    senderBtn.classList.remove('active');
    signupForm.classList.remove('d-none');
    signupForm.dataset.role = 'driver';
  });

  senderBtn.addEventListener('click', () => {
    senderBtn.classList.add('active');
    driverBtn.classList.remove('active');
    signupForm.classList.remove('d-none');
    signupForm.dataset.role = 'sender';
  });

  signupForm.addEventListener('submit', (e) => {
    e.preventDefault();

    const username = document.getElementById('username').value.trim();
    const email = document.getElementById('email').value.trim();
    const password = document.getElementById('password').value;
    const gender = document.getElementById('gender').value;
    const age = document.getElementById('age').value;
    const role = signupForm.dataset.role;

    if (!username || !email || !password || !gender || !age) {
      alert(' يرجى ملء جميع الحقول المطلوبة');
      return;
    }

    alert(`🎉 تم إنشاء حساب ${role === 'driver' ? 'كسائق' : 'كصاحب طرد'} بنجاح!\nمرحبًا ${username} 👋`);
    signupForm.reset();
    driverBtn.classList.remove('active');
    senderBtn.classList.remove('active');
    signupForm.classList.add('d-none');
  });
});
