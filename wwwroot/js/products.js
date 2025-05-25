// JavaScript for Filters and Navigation
document.addEventListener('DOMContentLoaded', function () {

    // Back button functionality
    const backButton = document.querySelector('.back-link');
    if (backButton) {
        backButton.addEventListener('click', function (e) {
            e.preventDefault();
            window.location.href = '/';
        });
    }

    // Category filter functionality
    const categoryFilters = document.querySelectorAll('.filter-item[data-category]');
    categoryFilters.forEach(filter => {
        filter.addEventListener('click', function () {
            const category = this.getAttribute('data-category');
            if (category) {
                // Navigate to the selected category
                window.location.href = `/Products/Category/${encodeURIComponent(category)}`;
            }
        });
    });

    // Brand filter functionality
    const brandFilters = document.querySelectorAll('.filter-item[data-brand]');
    const currentUrl = new URL(window.location.href);

    brandFilters.forEach(filter => {
        filter.addEventListener('click', function () {
            const brand = this.getAttribute('data-brand');

            // Toggle brand filter
            if (this.classList.contains('active')) {
                // Remove brand filter
                currentUrl.searchParams.delete('brand');
                this.classList.remove('active');
            } else {
                // Add brand filter
                currentUrl.searchParams.set('brand', brand);
                // Remove active class from other brand filters
                brandFilters.forEach(f => f.classList.remove('active'));
                this.classList.add('active');
            }

            // Navigate with the new filter
            window.location.href = currentUrl.toString();
        });
    });

    // Initialize active states based on current URL parameters
    const urlParams = new URLSearchParams(window.location.search);
    const selectedBrand = urlParams.get('brand');

    if (selectedBrand) {
        const activeBrandFilter = document.querySelector(`[data-brand="${selectedBrand}"]`);
        if (activeBrandFilter) {
            activeBrandFilter.classList.add('active');
        }
    }

    // Set active category based on current page
    const currentPath = window.location.pathname;
    const categoryMatch = currentPath.match(/\/Products\/Category\/(.+)/);
    if (categoryMatch) {
        const currentCategory = decodeURIComponent(categoryMatch[1]);
        const activeCategoryFilter = document.querySelector(`[data-category="${currentCategory}"]`);
        if (activeCategoryFilter) {
            activeCategoryFilter.classList.add('active');
        }
    }

    // Add to cart functionality
    const addToCartButtons = document.querySelectorAll('.add-to-cart');
    addToCartButtons.forEach(button => {
        button.addEventListener('click', function () {
            const productCard = this.closest('.product-card');
            const productId = productCard.getAttribute('data-product-id');
            const productName = productCard.querySelector('.product-name').textContent;

            // Add loading state
            this.innerHTML = '<span class="cart-icon">⏳</span> Adding...';
            this.disabled = true;

            // Simulate adding to cart (replace with actual API call)
            setTimeout(() => {
                // Show success state
                this.innerHTML = '<span class="cart-icon">✓</span> Added!';
                this.style.backgroundColor = '#10b981';

                // Reset after 2 seconds
                setTimeout(() => {
                    this.innerHTML = '<span class="cart-icon">🛒</span> Add to Cart';
                    this.style.backgroundColor = '';
                    this.disabled = false;
                }, 2000);

                console.log(`Added product ${productId} (${productName}) to cart`);
            }, 1000);
        });
    });

    // Search functionality (if you have a search box)
    const searchBox = document.querySelector('input[type="search"]');
    if (searchBox) {
        let searchTimeout;
        searchBox.addEventListener('input', function () {
            clearTimeout(searchTimeout);
            const query = this.value.trim();

            searchTimeout = setTimeout(() => {
                if (query.length > 2) {
                    filterProductsBySearch(query);
                } else if (query.length === 0) {
                    showAllProducts();
                }
            }, 300);
        });
    }

    // Filter products by search query
    function filterProductsBySearch(query) {
        const products = document.querySelectorAll('.product-card');
        const lowerQuery = query.toLowerCase();

        products.forEach(product => {
            const productName = product.querySelector('.product-name').textContent.toLowerCase();
            if (productName.includes(lowerQuery)) {
                product.style.display = 'block';
            } else {
                product.style.display = 'none';
            }
        });
    }

    // Show all products
    function showAllProducts() {
        const products = document.querySelectorAll('.product-card');
        products.forEach(product => {
            product.style.display = 'block';
        });
    }
});