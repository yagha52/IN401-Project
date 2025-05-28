function toggleEditMode() {
    const display = document.getElementById('profile-display');
    const form = document.getElementById('profile-form');
    const editButton = event.target.closest('.edit-button');
    
    if (display.style.display === 'none') {
        display.style.display = 'grid';
        form.style.display = 'none';
        editButton.innerHTML = '<i class="fas fa-edit"></i> Edit';
    } else {
        display.style.display = 'none';
        form.style.display = 'block';
        editButton.innerHTML = '<i class="fas fa-times"></i> Cancel';
    }
}

function cancelEdit() {
    const display = document.getElementById('profile-display');
    const form = document.getElementById('profile-form');
    const editButton = document.querySelector('#profile-tab .edit-button');
    
    display.style.display = 'grid';
    form.style.display = 'none';
    editButton.innerHTML = '<i class="fas fa-edit"></i> Edit';
    
    // Reset form to original values
    form.reset();
}

function toggleAddressEditMode() {
    const display = document.getElementById('address-display');
    const form = document.getElementById('address-form');
    const editButton = event.target.closest('.edit-button');
    
    if (display.style.display === 'none') {
        display.style.display = 'grid';
        form.style.display = 'none';
        editButton.innerHTML = '<i class="fas fa-edit"></i> Edit';
    } else {
        display.style.display = 'none';
        form.style.display = 'block';
        editButton.innerHTML = '<i class="fas fa-times"></i> Cancel';
    }
}

function cancelAddressEdit() {
    const display = document.getElementById('address-display');
    const form = document.getElementById('address-form');
    const editButton = document.querySelector('#address-tab .edit-button');
    
    display.style.display = 'grid';
    form.style.display = 'none';
    editButton.innerHTML = '<i class="fas fa-edit"></i> Edit';
    
    // Reset form to original values
    form.reset();
}

// Enhanced form validation
function validateProfileForm() {
    const form = document.getElementById('profile-form');
    const fullName = form.querySelector('input[name="UserProfile.FullName"]');
    const email = form.querySelector('input[name="UserProfile.Email"]');
    const phone = form.querySelector('input[name="UserProfile.Phone"]');
    
    let isValid = true;
    
    // Clear previous error states
    clearFormErrors(form);
    
    // Validate full name
    if (!fullName.value.trim()) {
        showFieldError(fullName, 'Full name is required');
        isValid = false;
    } else if (fullName.value.trim().length < 2) {
        showFieldError(fullName, 'Full name must be at least 2 characters');
        isValid = false;
    }
    
    // Validate email
    const emailRegex = /^[^\s@@]+@@[^\s@@]+\.[^\s@@]+$/;
    if (!email.value.trim()) {
        showFieldError(email, 'Email is required');
        isValid = false;
    } else if (!emailRegex.test(email.value)) {
        showFieldError(email, 'Please enter a valid email address');
        isValid = false;
    }
    
    // Validate phone (optional but must be valid if provided)
    if (phone.value.trim()) {
        const phoneRegex = /^[\+]?[\d\s\-\(\)]{10,}$/;
        if (!phoneRegex.test(phone.value.trim())) {
            showFieldError(phone, 'Please enter a valid phone number');
            isValid = false;
        }
    }
    
    return isValid;
}

function validateAddressForm() {
    const form = document.getElementById('address-form');
    const streetAddress = form.querySelector('input[name="Address.StreetAddress"]');
    const city = form.querySelector('input[name="Address.City"]');
    const state = form.querySelector('input[name="Address.State"]');
    const zipCode = form.querySelector('input[name="Address.ZipCode"]');
    const country = form.querySelector('input[name="Address.Country"]');
    
    let isValid = true;
    
    // Clear previous error states
    clearFormErrors(form);
    
    // Validate required fields
    const requiredFields = [
        { field: streetAddress, message: 'Street address is required' },
        { field: city, message: 'City is required' },
        { field: state, message: 'State is required' },
        { field: zipCode, message: 'ZIP code is required' },
        { field: country, message: 'Country is required' }
    ];
    
    requiredFields.forEach(function(item) {
        if (!item.field.value.trim()) {
            showFieldError(item.field, item.message);
            isValid = false;
        }
    });
    
    // Validate ZIP code format (basic validation)
    if (zipCode.value.trim()) {
        const zipRegex = /^[\d\-\s]{5,10}$/;
        if (!zipRegex.test(zipCode.value.trim())) {
            showFieldError(zipCode, 'Please enter a valid ZIP code');
            isValid = false;
        }
    }
    
    return isValid;
}

function showFieldError(field, message) {
    field.classList.add('error');
    field.style.borderColor = '#ef4444';
    
    // Remove existing error message
    const existingError = field.parentNode.querySelector('.field-error');
    if (existingError) {
        existingError.remove();
    }
    
    // Add error message
    const errorDiv = document.createElement('div');
    errorDiv.className = 'field-error';
    errorDiv.style.color = '#ef4444';
    errorDiv.style.fontSize = '0.8rem';
    errorDiv.style.marginTop = '0.25rem';
    errorDiv.textContent = message;
    
    field.parentNode.appendChild(errorDiv);
}

function clearFormErrors(form) {
    // Remove error classes and messages
    const errorFields = form.querySelectorAll('.error');
    errorFields.forEach(function(field) {
        field.classList.remove('error');
        field.style.borderColor = '';
    });
    
    const errorMessages = form.querySelectorAll('.field-error');
    errorMessages.forEach(function(error) {
        error.remove();
    });
}

// Form submission handlers
document.addEventListener('DOMContentLoaded', function() {
    // Profile form submission
    const profileForm = document.getElementById('profile-form');
    if (profileForm) {
        profileForm.addEventListener('submit', function(e) {
            if (!validateProfileForm()) {
                e.preventDefault();
                return false;
            }
            
            // Show loading state
            const submitBtn = profileForm.querySelector('button[type="submit"]');
            const originalText = submitBtn.innerHTML;
            submitBtn.innerHTML = '<i class="fas fa-spinner fa-spin"></i> Saving...';
            submitBtn.disabled = true;
            
            // Re-enable button after a delay (in case of server error)
            setTimeout(function() {
                submitBtn.innerHTML = originalText;
                submitBtn.disabled = false;
            }, 5000);
        });
    }
    
    // Address form submission
    const addressForm = document.getElementById('address-form');
    if (addressForm) {
        addressForm.addEventListener('submit', function(e) {
            if (!validateAddressForm()) {
                e.preventDefault();
                return false;
            }
            
            // Show loading state
            const submitBtn = addressForm.querySelector('button[type="submit"]');
            const originalText = submitBtn.innerHTML;
            submitBtn.innerHTML = '<i class="fas fa-spinner fa-spin"></i> Updating...';
            submitBtn.disabled = true;
            
            // Re-enable button after a delay (in case of server error)
            setTimeout(function() {
                submitBtn.innerHTML = originalText;
                submitBtn.disabled = false;
            }, 5000);
        });
    }
    
    // Auto-hide success/error messages
    const alerts = document.querySelectorAll('.alert');
    alerts.forEach(function(alert) {
        setTimeout(function() {
            alert.style.opacity = '0';
            alert.style.transform = 'translateY(-10px)';
            setTimeout(function() {
                alert.style.display = 'none';
            }, 300);
        }, 5000);
    });
    
    // Real-time input validation
    const profileInputs = document.querySelectorAll('#profile-form input');
    profileInputs.forEach(function(input) {
        input.addEventListener('blur', function() {
            // Clear previous errors for this field
            this.classList.remove('error');
            this.style.borderColor = '';
            const errorMsg = this.parentNode.querySelector('.field-error');
            if (errorMsg) errorMsg.remove();
            
            // Validate individual field
            validateIndividualField(this);
        });
    });
    
    const addressInputs = document.querySelectorAll('#address-form input');
    addressInputs.forEach(function(input) {
        input.addEventListener('blur', function() {
            // Clear previous errors for this field
            this.classList.remove('error');
            this.style.borderColor = '';
            const errorMsg = this.parentNode.querySelector('.field-error');
            if (errorMsg) errorMsg.remove();
            
            // Validate individual field
            validateIndividualField(this);
        });
    });
});

function validateIndividualField(field) {
    const fieldName = field.name;
    const value = field.value.trim();
    
    switch (fieldName) {
        case 'UserProfile.FullName':
            if (!value) {
                showFieldError(field, 'Full name is required');
            } else if (value.length < 2) {
                showFieldError(field, 'Full name must be at least 2 characters');
            }
            break;
            
        case 'UserProfile.Email':
            const emailRegex = /^[^\s@@]+@@[^\s@@]+\.[^\s@@]+$/;
            if (!value) {
                showFieldError(field, 'Email is required');
            } else if (!emailRegex.test(value)) {
                showFieldError(field, 'Please enter a valid email address');
            }
            break;
            
        case 'UserProfile.Phone':
            if (value) {
                const phoneRegex = /^[\+]?[\d\s\-\(\)]{8,}$/;
                if (!phoneRegex.test(value)) {
                    showFieldError(field, 'Please enter a valid phone number');
                }
            }
            break;
            
        case 'Address.StreetAddress':
        case 'Address.City':
        case 'Address.State':
        case 'Address.Country':
            if (!value) {
                const fieldLabel = field.previousElementSibling.textContent;
                showFieldError(field, fieldLabel + ' is required');
            }
            break;
            
        case 'Address.ZipCode':
            if (!value) {
                showFieldError(field, 'ZIP code is required');
            } else {
                const zipRegex = /^[\d\-\s]{4,10}$/;
                if (!zipRegex.test(value)) {
                    showFieldError(field, 'Please enter a valid ZIP code');
                }
            }
            break;
    }
}

// Order actions
function viewOrderDetails(orderId) {
    // This would typically navigate to an order details page
    console.log('Viewing details for order:', orderId);
    // window.location.href = '/orders/' + orderId;
}

function reorderItems(orderId) {
    // This would typically add items to cart and redirect to checkout
    console.log('Reordering items from order:', orderId);
    
    // Show confirmation
    if (confirm('Add all items from this order to your cart?')) {
        // Simulate API call
        const btn = event.target;
        const originalText = btn.innerHTML;
        btn.innerHTML = '<i class="fas fa-spinner fa-spin"></i> Adding to Cart...';
        btn.disabled = true;
        
        setTimeout(function() {
            btn.innerHTML = originalText;
            btn.disabled = false;
            
            // Show success message
            showNotification('Items added to cart successfully!', 'success');
        }, 2000);
    }
}

function showNotification(message, type) {
    type = type || 'info';
    const notification = document.createElement('div');
    notification.className = 'notification notification-' + type;
    notification.innerHTML = 
        '<i class="fas fa-' + (type === 'success' ? 'check-circle' : 'info-circle') + '"></i>' +
        '<span>' + message + '</span>';
    
    // Add notification styles
    notification.style.cssText = 
        'position: fixed;' +
        'top: 20px;' +
        'right: 20px;' +
        'background: ' + (type === 'success' ? 'rgba(16, 185, 129, 0.1)' : 'rgba(74, 158, 255, 0.1)') + ';' +
        'border: 1px solid ' + (type === 'success' ? 'rgba(16, 185, 129, 0.3)' : 'rgba(74, 158, 255, 0.3)') + ';' +
        'color: ' + (type === 'success' ? '#10b981' : '#4a9eff') + ';' +
        'padding: 1rem 1.5rem;' +
        'border-radius: 8px;' +
        'display: flex;' +
        'align-items: center;' +
        'gap: 0.5rem;' +
        'font-size: 0.9rem;' +
        'font-weight: 500;' +
        'z-index: 1000;' +
        'animation: slideInRight 0.3s ease-out;';
    
    document.body.appendChild(notification);
    
    // Auto remove after 3 seconds
    setTimeout(function() {
        notification.style.animation = 'slideOutRight 0.3s ease-in';
        setTimeout(function() {
            document.body.removeChild(notification);
        }, 300);
    }, 3000);
}