document.getElementById('registerForm').addEventListener('submit', function(e) {
    const btn = document.getElementById('registerBtn');
    const agreeTerms = document.getElementById('agreeTerms');
    
    if (!agreeTerms.checked) {
        e.preventDefault();
        alert('Please agree to the Terms of Service and Privacy Policy to continue.');
        return;
    }
    
    btn.disabled = true;
    btn.textContent = 'Creating Account...';
});

// Password confirmation validation
const passwordInput = document.querySelector('input[asp-for="Input.Password"]');
const confirmPasswordInput = document.querySelector('input[asp-for="Input.ConfirmPassword"]');

if (passwordInput && confirmPasswordInput) {
    confirmPasswordInput.addEventListener('input', function() {
        if (this.value && this.value !== passwordInput.value) {
            this.style.borderColor = '#dc3545';
        } else if (this.value === passwordInput.value) {
            this.style.borderColor = '#28a745';
        } else {
            this.style.borderColor = '#e1e5e9';
        }
    });
}