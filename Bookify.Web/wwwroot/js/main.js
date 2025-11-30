/* ============================================
   BOOKIFY - MAIN JAVASCRIPT
   Reusable JavaScript utilities for customer/public pages
   ============================================ */

// ========== Password Toggle Functionality ==========
function initPasswordToggle() {
    const passwordToggles = document.querySelectorAll('.password-toggle');
    
    passwordToggles.forEach(toggle => {
        toggle.addEventListener('click', function() {
            const input = this.previousElementSibling;
            const icon = this.querySelector('i');
            
            if (input && input.type === 'password') {
                input.type = 'text';
                icon.classList.remove('fa-eye');
                icon.classList.add('fa-eye-slash');
            } else if (input) {
                input.type = 'password';
                icon.classList.remove('fa-eye-slash');
                icon.classList.add('fa-eye');
            }
        });
    });
}

// ========== Form Validation Enhancement ==========
function enhanceFormValidation() {
    const forms = document.querySelectorAll('form[data-validate]');
    
    forms.forEach(form => {
        form.addEventListener('submit', function(e) {
            if (!form.checkValidity()) {
                e.preventDefault();
                e.stopPropagation();
            }
            
            form.classList.add('was-validated');
        });
    });
}

// ========== Date Picker Enhancement ==========
function initDatePickers() {
    const dateInputs = document.querySelectorAll('input[type="date"]');
    
    dateInputs.forEach(input => {
        // Set minimum date to today
        const today = new Date().toISOString().split('T')[0];
        if (!input.min || input.min < today) {
            input.min = today;
        }
        
        // Add custom styling
        input.addEventListener('focus', function() {
            this.style.borderColor = 'var(--primary-color)';
        });
        
        input.addEventListener('blur', function() {
            this.style.borderColor = '';
        });
    });
}

// ========== Auto-calculate Nights and Total ==========
function initBookingCalculator() {
    const checkInInput = document.querySelector('[name="CheckInDate"]');
    const checkOutInput = document.querySelector('[name="CheckOutDate"]');
    const nightsInput = document.querySelector('[name="Nights"]');
    const totalInput = document.querySelector('[name="TotalAmount"]');
    
    if (checkInInput && checkOutInput && nightsInput && totalInput) {
        const pricePerNight = parseFloat(totalInput.value) || 0;
        
        function calculateBooking() {
            const checkIn = new Date(checkInInput.value);
            const checkOut = new Date(checkOutInput.value);
            
            if (checkIn && checkOut && checkOut > checkIn) {
                const diffTime = checkOut - checkIn;
                const diffDays = Math.ceil(diffTime / (1000 * 60 * 60 * 24));
                
                nightsInput.value = diffDays;
                
                if (pricePerNight > 0) {
                    totalInput.value = (diffDays * pricePerNight).toFixed(2);
                } else {
                    // Try to get price from data attribute
                    const roomPrice = parseFloat(checkInInput.dataset.price || 0);
                    if (roomPrice > 0) {
                        totalInput.value = (diffDays * roomPrice).toFixed(2);
                    }
                }
            } else {
                nightsInput.value = '';
                totalInput.value = '';
            }
        }
        
        checkInInput.addEventListener('change', calculateBooking);
        checkOutInput.addEventListener('change', calculateBooking);
    }
}

// ========== Loading States ==========
function showLoading(element) {
    if (element) {
        element.classList.add('loading');
        element.disabled = true;
    }
}

function hideLoading(element) {
    if (element) {
        element.classList.remove('loading');
        element.disabled = false;
    }
}

// ========== Toast Notifications ==========
function showToast(message, type = 'info') {
    const toast = document.createElement('div');
    toast.className = `toast toast-${type}`;
    toast.textContent = message;
    
    // Add styles
    Object.assign(toast.style, {
        position: 'fixed',
        top: '20px',
        right: '20px',
        padding: '1rem 1.5rem',
        backgroundColor: type === 'success' ? '#28a745' : type === 'error' ? '#dc3545' : '#17a2b8',
        color: 'white',
        borderRadius: '8px',
        boxShadow: '0 4px 6px rgba(0, 0, 0, 0.1)',
        zIndex: '9999',
        animation: 'slideIn 0.3s ease'
    });
    
    document.body.appendChild(toast);
    
    setTimeout(() => {
        toast.style.animation = 'slideOut 0.3s ease';
        setTimeout(() => toast.remove(), 300);
    }, 3000);
}

// Add CSS animations
const style = document.createElement('style');
style.textContent = `
    @keyframes slideIn {
        from {
            transform: translateX(100%);
            opacity: 0;
        }
        to {
            transform: translateX(0);
            opacity: 1;
        }
    }
    
    @keyframes slideOut {
        from {
            transform: translateX(0);
            opacity: 1;
        }
        to {
            transform: translateX(100%);
            opacity: 0;
        }
    }
`;
document.head.appendChild(style);

// ========== Table Enhancements ==========
function enhanceTables() {
    const tables = document.querySelectorAll('.table');
    
    tables.forEach(table => {
        // Add responsive wrapper if not present
        if (!table.parentElement.classList.contains('table-responsive')) {
            const wrapper = document.createElement('div');
            wrapper.className = 'table-responsive';
            table.parentElement.insertBefore(wrapper, table);
            wrapper.appendChild(table);
        }
    });
}

// ========== Smooth Scrolling ==========
function initSmoothScroll() {
    document.querySelectorAll('a[href^="#"]').forEach(anchor => {
        anchor.addEventListener('click', function(e) {
            const href = this.getAttribute('href');
            if (href !== '#' && href.length > 1) {
                const target = document.querySelector(href);
                if (target) {
                    e.preventDefault();
                    target.scrollIntoView({
                        behavior: 'smooth',
                        block: 'start'
                    });
                }
            }
        });
    });
}

// ========== Initialize on DOM Load ==========
document.addEventListener('DOMContentLoaded', function() {
    initPasswordToggle();
    enhanceFormValidation();
    initDatePickers();
    initBookingCalculator();
    enhanceTables();
    initSmoothScroll();
    
    // Auto-hide alerts after 5 seconds
    const alerts = document.querySelectorAll('.alert');
    alerts.forEach(alert => {
        setTimeout(() => {
            alert.style.transition = 'opacity 0.5s';
            alert.style.opacity = '0';
            setTimeout(() => alert.remove(), 500);
        }, 5000);
    });
});

// ========== Export Functions ==========
window.Bookify = {
    showToast,
    showLoading,
    hideLoading,
    initPasswordToggle,
    initBookingCalculator
};

