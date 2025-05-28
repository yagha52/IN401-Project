// Enhanced Cart JavaScript Functions

// Add item to cart function (for product pages)
function addToCart(productId, quantity = 1) {
    const token = getAntiForgeryToken();
    if (!token) {
        console.error('Anti-forgery token not found');
        alert('Security token missing. Please refresh the page.');
        return;
    }

    showLoadingIndicator('Adding to cart...');

    // Disable add to cart button if it exists
    const addButton = document.querySelector('.add-to-cart-btn');
    if (addButton) {
        addButton.disabled = true;
        addButton.innerHTML = '<i class="fas fa-spinner fa-spin"></i> Adding...';
    }

    fetch('/Cart?handler=AddToCart', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'RequestVerificationToken': token
        },
        body: JSON.stringify({ 
            productId: parseInt(productId), 
            quantity: parseInt(quantity)
        })
    })
    .then(response => {
        if (!response.ok) {
            throw new Error(`HTTP error! status: ${response.status}`);
        }
        return response.json();
    })
    .then(data => {
        hideLoadingIndicator();
        if (data.success) {
            showNotification('success', data.message || 'Item added to cart!');
            
            // Update cart count in navigation if it exists
            updateCartCount(data.cartCount);
            
            // Reset button
            if (addButton) {
                addButton.disabled = false;
                addButton.innerHTML = '<i class="fas fa-shopping-cart"></i> Add to Cart';
            }
        } else {
            console.error('Server error:', data.message);
            showNotification('error', data.message || 'Error adding item to cart');
            
            // Reset button
            if (addButton) {
                addButton.disabled = false;
                addButton.innerHTML = '<i class="fas fa-shopping-cart"></i> Add to Cart';
            }
        }
    })
    .catch(error => {
        hideLoadingIndicator();
        console.error('Network error:', error);
        showNotification('error', 'Network error. Please check your connection and try again.');
        
        // Reset button
        if (addButton) {
            addButton.disabled = false;
            addButton.innerHTML = '<i class="fas fa-shopping-cart"></i> Add to Cart';
        }
    });
}

function updateQuantity(itemId, newQuantity) {
    const quantity = parseInt(newQuantity);
    if (isNaN(quantity) || quantity < 1) {
        showNotification('error', 'Please enter a valid quantity (minimum 1)');
        location.reload();
        return;
    }

    const quantityInput = event?.target;
    if (quantityInput) {
        quantityInput.disabled = true;
        quantityInput.style.opacity = '0.5';
    }

    const token = getAntiForgeryToken();
    if (!token) {
        console.error('Anti-forgery token not found');
        showNotification('error', 'Security token missing. Please refresh the page.');
        location.reload();
        return;
    }

    showLoadingIndicator('Updating quantity...');

    fetch('/Cart?handler=UpdateQuantity', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'RequestVerificationToken': token
        },
        body: JSON.stringify({ 
            itemId: parseInt(itemId), 
            quantity: quantity 
        })
    })
    .then(response => {
        if (!response.ok) {
            throw new Error(`HTTP error! status: ${response.status}`);
        }
        return response.json();
    })
    .then(data => {
        hideLoadingIndicator();
        if (data.success) {
            animateItemUpdate(itemId);
            setTimeout(() => {
                location.reload();
            }, 300);
        } else {
            console.error('Server error:', data.message);
            showNotification('error', 'Error updating quantity: ' + (data.message || 'Unknown error'));
            location.reload();
        }
    })
    .catch(error => {
        hideLoadingIndicator();
        console.error('Network error:', error);
        showNotification('error', 'Network error. Please check your connection and try again.');
        location.reload();
    })
    .finally(() => {
        if (quantityInput) {
            quantityInput.disabled = false;
            quantityInput.style.opacity = '1';
        }
    });
}

function removeItem(itemId) {
    if (!confirm('Are you sure you want to remove this item from your cart?')) {
        return;
    }

    const token = getAntiForgeryToken();
    if (!token) {
        console.error('Anti-forgery token not found');
        showNotification('error', 'Security token missing. Please refresh the page.');
        location.reload();
        return;
    }

    showLoadingIndicator('Removing item...');

    const itemElement = document.querySelector(`[data-item-id="${itemId}"]`);
    if (itemElement) {
        itemElement.style.opacity = '0.5';
        itemElement.style.pointerEvents = 'none';
    }

    fetch('/Cart?handler=RemoveItem', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'RequestVerificationToken': token
        },
        body: JSON.stringify({ 
            itemId: parseInt(itemId) 
        })
    })
    .then(response => {
        if (!response.ok) {
            throw new Error(`HTTP error! status: ${response.status}`);
        }
        return response.json();
    })
    .then(data => {
        hideLoadingIndicator();
        if (data.success) {
            if (itemElement) {
                itemElement.style.transform = 'translateX(-100%)';
                itemElement.style.transition = 'all 0.3s ease';
                setTimeout(() => {
                    location.reload();
                }, 300);
            } else {
                location.reload();
            }
        } else {
            console.error('Server error:', data.message);
            showNotification('error', 'Error removing item: ' + (data.message || 'Unknown error'));
            if (itemElement) {
                itemElement.style.opacity = '1';
                itemElement.style.pointerEvents = 'auto';
            }
        }
    })
    .catch(error => {
        hideLoadingIndicator();
        console.error('Network error:', error);
        showNotification('error', 'Network error. Please check your connection and try again.');
        if (itemElement) {
            itemElement.style.opacity = '1';
            itemElement.style.pointerEvents = 'auto';
        }
    });
}

function clearCart() {
    if (!confirm('Are you sure you want to clear your entire cart? This action cannot be undone.')) {
        return;
    }

    const token = getAntiForgeryToken();
    if (!token) {
        console.error('Anti-forgery token not found');
        showNotification('error', 'Security token missing. Please refresh the page.');
        location.reload();
        return;
    }

    showLoadingIndicator('Clearing cart...');

    const clearButton = document.querySelector('.clear-cart-btn');
    if (clearButton) {
        clearButton.disabled = true;
        clearButton.style.opacity = '0.5';
    }

    fetch('/Cart?handler=ClearCart', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'RequestVerificationToken': token
        },
        body: JSON.stringify({})
    })
    .then(response => {
        if (!response.ok) {
            throw new Error(`HTTP error! status: ${response.status}`);
        }
        return response.json();
    })
    .then(data => {
        hideLoadingIndicator();
        if (data.success) {
            const cartItems = document.querySelector('.cart-items');
            if (cartItems) {
                cartItems.style.opacity = '0';
                cartItems.style.transform = 'scale(0.9)';
                cartItems.style.transition = 'all 0.3s ease';
            }
            setTimeout(() => {
                location.reload();
            }, 300);
        } else {
            console.error('Server error:', data.message);
            showNotification('error', 'Error clearing cart: ' + (data.message || 'Unknown error'));
            if (clearButton) {
                clearButton.disabled = false;
                clearButton.style.opacity = '1';
            }
        }
    })
    .catch(error => {
        hideLoadingIndicator();
        console.error('Network error:', error);
        showNotification('error', 'Network error. Please check your connection and try again.');
        if (clearButton) {
            clearButton.disabled = false;
            clearButton.style.opacity = '1';
        }
    });
}

function proceedToCheckout() {
    const token = getAntiForgeryToken();
    if (!token) {
        console.error('Anti-forgery token not found');
        showNotification('error', 'Security token missing. Please refresh the page.');
        return;
    }

    const checkoutBtn = document.querySelector('.checkout-btn');
    if (checkoutBtn) {
        checkoutBtn.disabled = true;
        checkoutBtn.innerHTML = '<i class="fas fa-spinner fa-spin"></i> Processing...';
    }

    showLoadingIndicator('Processing your order...');

    fetch('/Cart?handler=Checkout', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'RequestVerificationToken': token
        },
        body: JSON.stringify({})
    })
    .then(response => {
        if (!response.ok) {
            throw new Error(`HTTP error! status: ${response.status}`);
        }
        return response.json();
    })
    .then(data => {
        hideLoadingIndicator();
        
        if (data.success) {
            // Show success message and redirect to main page
            showNotification('success', data.message, 5000);
            setTimeout(() => {
                window.location.href = '/?orderSuccess=true&orderId=' + (data.orderId || '');
            }, 2000);
        } else if (data.requiresLogin) {
            // User needs to log in
            showNotification('info', data.message, 3000);
            setTimeout(() => {
                window.location.href = '/Account/Login?returnUrl=/Cart';
            }, 2000);
        } else {
            // General error
            console.error('Checkout error:', data.message);
            showNotification('error', data.message || 'Error processing your order. Please try again.');
            
            // Reset button
            if (checkoutBtn) {
                checkoutBtn.disabled = false;
                checkoutBtn.innerHTML = 'Proceed to Checkout';
            }
        }
    })
    .catch(error => {
        hideLoadingIndicator();
        console.error('Network error:', error);
        showNotification('error', 'Network error. Please check your connection and try again.');
        
        // Reset button
        if (checkoutBtn) {
            checkoutBtn.disabled = false;
            checkoutBtn.innerHTML = 'Proceed to Checkout';
        }
    });
}

// Utility functions
function getAntiForgeryToken() {
    const tokenElement = document.querySelector('input[name="__RequestVerificationToken"]');
    return tokenElement ? tokenElement.value : null;
}

function showLoadingIndicator(message = 'Loading...') {
    // Remove existing loading indicator
    hideLoadingIndicator();
    
    const loadingDiv = document.createElement('div');
    loadingDiv.id = 'cart-loading-indicator';
    loadingDiv.className = 'loading-overlay';
    loadingDiv.innerHTML = `
        <div class="loading-content">
            <div class="loading-spinner">
                <i class="fas fa-spinner fa-spin"></i>
            </div>
            <div class="loading-message">${message}</div>
        </div>
    `;
    
    document.body.appendChild(loadingDiv);
    
    // Force a reflow to ensure the element is added before showing
    loadingDiv.offsetHeight;
    loadingDiv.classList.add('show');
}

function hideLoadingIndicator() {
    const loadingDiv = document.getElementById('cart-loading-indicator');
    if (loadingDiv) {
        loadingDiv.classList.remove('show');
        setTimeout(() => {
            if (loadingDiv.parentNode) {
                loadingDiv.parentNode.removeChild(loadingDiv);
            }
        }, 200);
    }
}

function showNotification(type, message, duration = 4000) {
    // Remove existing notifications
    const existingNotifications = document.querySelectorAll('.cart-notification');
    existingNotifications.forEach(notification => notification.remove());
    
    const notification = document.createElement('div');
    notification.className = `cart-notification cart-notification-${type}`;
    
    const iconMap = {
        'success': 'fas fa-check-circle',
        'error': 'fas fa-exclamation-circle',
        'info': 'fas fa-info-circle',
        'warning': 'fas fa-exclamation-triangle'
    };
    
    notification.innerHTML = `
        <div class="notification-content">
            <i class="${iconMap[type] || iconMap.info}"></i>
            <span class="notification-message">${message}</span>
            <button class="notification-close" onclick="this.parentElement.parentElement.remove()">
                <i class="fas fa-times"></i>
            </button>
        </div>
    `;
    
    document.body.appendChild(notification);
    
    // Force a reflow to ensure the element is added before showing
    notification.offsetHeight;
    notification.classList.add('show');
    
    // Auto-hide after specified duration
    setTimeout(() => {
        if (notification.parentNode) {
            notification.classList.remove('show');
            setTimeout(() => {
                if (notification.parentNode) {
                    notification.parentNode.removeChild(notification);
                }
            }, 300);
        }
    }, duration);
}

function updateCartCount(count) {
    const cartCountElements = document.querySelectorAll('.cart-count, .nav-cart-count');
    cartCountElements.forEach(element => {
        element.textContent = count || '0';
        
        // Add a small animation to indicate update
        element.style.transform = 'scale(1.2)';
        element.style.transition = 'transform 0.2s ease';
        setTimeout(() => {
            element.style.transform = 'scale(1)';
        }, 200);
    });
}

function animateItemUpdate(itemId) {
    const itemElement = document.querySelector(`[data-item-id="${itemId}"]`);
    if (itemElement) {
        itemElement.style.transform = 'scale(1.02)';
        itemElement.style.transition = 'transform 0.2s ease';
        itemElement.style.boxShadow = '0 4px 12px rgba(0,123,255,0.3)';
        
        setTimeout(() => {
            itemElement.style.transform = 'scale(1)';
            itemElement.style.boxShadow = '';
        }, 200);
    }
}

// Initialize cart page functionality
document.addEventListener('DOMContentLoaded', function() {
    // Add keyboard event listeners for quantity inputs
    const quantityInputs = document.querySelectorAll('.quantity-input');
    quantityInputs.forEach(input => {
        input.addEventListener('keydown', function(e) {
            // Allow: backspace, delete, tab, escape, enter
            if ([46, 8, 9, 27, 13].indexOf(e.keyCode) !== -1 ||
                // Allow: Ctrl+A, Ctrl+C, Ctrl+V, Ctrl+X
                (e.keyCode === 65 && e.ctrlKey === true) ||
                (e.keyCode === 67 && e.ctrlKey === true) ||
                (e.keyCode === 86 && e.ctrlKey === true) ||
                (e.keyCode === 88 && e.ctrlKey === true) ||
                // Allow: home, end, left, right, down, up
                (e.keyCode >= 35 && e.keyCode <= 40)) {
                return;
            }
            // Ensure that it is a number and stop the keypress
            if ((e.shiftKey || (e.keyCode < 48 || e.keyCode > 57)) && (e.keyCode < 96 || e.keyCode > 105)) {
                e.preventDefault();
            }
        });
        
        // Add blur event to validate minimum quantity
        input.addEventListener('blur', function() {
            const value = parseInt(this.value);
            if (isNaN(value) || value < 1) {
                this.value = 1;
                showNotification('warning', 'Quantity cannot be less than 1');
            }
        });
    });
    
    // Check for order success message in URL
    const urlParams = new URLSearchParams(window.location.search);
    if (urlParams.get('orderSuccess') === 'true') {
        const orderId = urlParams.get('orderId');
        const message = orderId ? 
            `Order #${orderId} has been placed successfully! Thank you for your purchase.` : 
            'Your order has been placed successfully! Thank you for your purchase.';
        showNotification('success', message, 6000);
        
        // Clean up URL parameters
        window.history.replaceState({}, document.title, window.location.pathname);
    }
    
    // Add smooth scroll behavior for better UX
    document.documentElement.style.scrollBehavior = 'smooth';
    
    // Add focus management for accessibility
    const removeButtons = document.querySelectorAll('.remove-btn');
    removeButtons.forEach(button => {
        button.addEventListener('focus', function() {
            this.style.outline = '2px solid #007bff';
            this.style.outlineOffset = '2px';
        });
        
        button.addEventListener('blur', function() {
            this.style.outline = '';
            this.style.outlineOffset = '';
        });
    });
    
    // Add cart item hover effects
    const cartItems = document.querySelectorAll('.cart-item');
    cartItems.forEach(item => {
        item.addEventListener('mouseenter', function() {
            this.style.transition = 'transform 0.2s ease, box-shadow 0.2s ease';
            this.style.transform = 'translateY(-2px)';
            this.style.boxShadow = '0 4px 12px rgba(0,0,0,0.1)';
        });
        
        item.addEventListener('mouseleave', function() {
            this.style.transform = 'translateY(0)';
            this.style.boxShadow = '';
        });
    });
});

// Error handling for global errors
window.addEventListener('error', function(event) {
    console.error('Global error:', event.error);
    showNotification('error', 'An unexpected error occurred. Please refresh the page and try again.');
});

// Handle beforeunload for unsaved changes warning (optional)
window.addEventListener('beforeunload', function(event) {
    const loadingIndicator = document.getElementById('cart-loading-indicator');
    if (loadingIndicator && loadingIndicator.classList.contains('show')) {
        event.preventDefault();
        event.returnValue = 'A cart operation is in progress. Are you sure you want to leave?';
        return event.returnValue;
    }
});