// Internationalization (i18n) System for Hanwsallak
// Supports Arabic and English

const i18n = {
  currentLanguage: localStorage.getItem('language') || 'ar',
  translations: {},

  // Initialize translations
  async init() {
    try {
      const response = await fetch(`scripts/locales/${this.currentLanguage}.json`);
      if (!response.ok) {
        throw new Error(`Failed to load translations: ${response.status}`);
      }
      this.translations = await response.json();
      this.applyLanguage();
    } catch (error) {
      console.error('Error loading translations:', error);
      // Fallback to English if Arabic fails
      if (this.currentLanguage === 'ar') {
        this.currentLanguage = 'en';
        localStorage.setItem('language', 'en');
        return this.init();
      }
      // If English also fails, use empty translations
      this.translations = {};
    }
  },

  // Get translation by key
  t(key, params = {}) {
    const keys = key.split('.');
    let value = this.translations;
    
    for (const k of keys) {
      if (value && typeof value === 'object') {
        value = value[k];
      } else {
        console.warn(`Translation key not found: ${key}`);
        return key;
      }
    }

    if (typeof value !== 'string') {
      console.warn(`Translation value is not a string for key: ${key}`);
      return key;
    }

    // Replace parameters in translation
    return value.replace(/\{\{(\w+)\}\}/g, (match, paramKey) => {
      return params[paramKey] !== undefined ? params[paramKey] : match;
    });
  },

  // Change language
  async setLanguage(lang) {
    if (lang === this.currentLanguage) {
      return Promise.resolve();
    }
    
    this.currentLanguage = lang;
    localStorage.setItem('language', lang);
    
    try {
      const response = await fetch(`scripts/locales/${lang}.json`);
      if (!response.ok) {
        throw new Error(`Failed to load translations: ${response.status}`);
      }
      this.translations = await response.json();
      this.applyLanguage();
      
      // Trigger custom event for language change
      window.dispatchEvent(new CustomEvent('languageChanged', { detail: { language: lang } }));
      return Promise.resolve();
    } catch (error) {
      console.error('Error loading translations:', error);
      return Promise.reject(error);
    }
  },

  // Apply language to page
  applyLanguage() {
    // Set document direction
    document.documentElement.dir = this.currentLanguage === 'ar' ? 'rtl' : 'ltr';
    document.documentElement.lang = this.currentLanguage;

    // Update all elements with data-i18n attribute
    document.querySelectorAll('[data-i18n]').forEach(element => {
      const key = element.getAttribute('data-i18n');
      const translation = this.t(key);
      
      if (element.tagName === 'INPUT' || element.tagName === 'TEXTAREA') {
        if (element.type === 'submit' || element.type === 'button') {
          element.value = translation;
        } else {
          element.placeholder = translation;
        }
      } else {
        element.textContent = translation;
      }
    });

    // Update all elements with data-i18n-html attribute (for HTML content)
    document.querySelectorAll('[data-i18n-html]').forEach(element => {
      const key = element.getAttribute('data-i18n-html');
      element.innerHTML = this.t(key);
    });

    // Update all elements with data-i18n-placeholder attribute
    document.querySelectorAll('[data-i18n-placeholder]').forEach(element => {
      const key = element.getAttribute('data-i18n-placeholder');
      element.placeholder = this.t(key);
    });

    // Update all elements with data-i18n-title attribute
    document.querySelectorAll('[data-i18n-title]').forEach(element => {
      const key = element.getAttribute('data-i18n-title');
      element.title = this.t(key);
    });

    // Update page title
    const titleKey = document.querySelector('[data-i18n-title-page]');
    if (titleKey) {
      document.title = this.t(titleKey.getAttribute('data-i18n-title-page'));
    }
  },

  // Get current language
  getLanguage() {
    return this.currentLanguage;
  }
};

// Initialize on page load
function initializeI18n() {
  i18n.init().then(() => {
    // Re-apply language after a short delay to ensure all elements are loaded
    setTimeout(() => {
      i18n.applyLanguage();
      // Re-apply after header/footer load
      setTimeout(() => i18n.applyLanguage(), 500);
    }, 100);
  });
}

if (document.readyState === 'loading') {
  document.addEventListener('DOMContentLoaded', initializeI18n);
} else {
  initializeI18n();
}

// Export for use in other scripts
window.i18n = i18n;

