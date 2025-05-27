document.addEventListener('DOMContentLoaded', function () {

    // Back button functionality
    const backButton = document.querySelector('.back-link');
    if (backButton) {
        backButton.addEventListener('click', function (e) {
            e.preventDefault();
            window.location.href = '/';
        });
    }

    // Get current category from URL path, e.g. /Products/Category/CPU
    function getCurrentCategory() {
        const match = window.location.pathname.match(/\/Products\/Category\/([^\/]+)/);
        return match ? decodeURIComponent(match[1]) : null;
    }

    // Build URL with category path and optional brand query param
    function buildUrl(category, brand) {
        const url = new URL(window.location.origin);
        url.pathname = `/Products/Category/${encodeURIComponent(category)}`;
        if (brand) {
            url.searchParams.set('brand', brand);
        }
        return url.toString();
    }

    // Category filter functionality
    const categoryFilters = document.querySelectorAll('.filter-item[data-category]');
    categoryFilters.forEach(filter => {
        filter.addEventListener('click', function () {
            const category = this.getAttribute('data-category');
            if (category) {
                window.location.href = buildUrl(category, null);
            }
        });
    });

    // Brand filter functionality
    const brandFilters = document.querySelectorAll('.filter-item[data-brand]');
    brandFilters.forEach(filter => {
        filter.addEventListener('click', function () {
            const brand = this.getAttribute('data-brand');
            if (!brand) return;

            const currentCategory = getCurrentCategory();
            if (!currentCategory) {
                console.warn('No category in URL - cannot apply brand filter.');
                return;
            }

            const urlParams = new URLSearchParams(window.location.search);
            const currentBrand = urlParams.get('brand');

            let newBrand = null;
            if (this.classList.contains('active')) {
                newBrand = null; // remove filter
            } else {
                newBrand = brand; // apply filter
            }

            window.location.href = buildUrl(currentCategory, newBrand);
        });
    });

    // Highlight current active category
    const currentCategory = getCurrentCategory();
    if (currentCategory) {
        const activeCategoryFilter = document.querySelector(`.filter-item[data-category="${currentCategory}"]`);
        if (activeCategoryFilter) {
            activeCategoryFilter.classList.add('active');
        }
    }

    // Highlight current active brand
    const urlParams = new URLSearchParams(window.location.search);
    const selectedBrand = urlParams.get('brand');
    if (selectedBrand) {
        const activeBrandFilter = document.querySelector(`.filter-item[data-brand="${selectedBrand}"]`);
        if (activeBrandFilter) {
            activeBrandFilter.classList.add('active');
        }
    }

    // Add to cart functionality
    const addToCartButtons = document.querySelectorAll('.add-to-cart');
    addToCartButtons.forEach(button => {
        button.addEventListener('click', function () {
            const productCard = this.closest('.product-card');
            const productId = productCard.getAttribute('data-product-id');
            const productName = productCard.querySelector('.product-name').textContent;

            this.innerHTML = '<span class="cart-icon">⏳</span> Adding...';
            this.disabled = true;

            setTimeout(() => {
                this.innerHTML = '<span class="cart-icon">✓</span> Added!';
                this.style.backgroundColor = '#10b981';

                setTimeout(() => {
                    this.innerHTML = '<span class="cart-icon">🛒</span> Add to Cart';
                    this.style.backgroundColor = '';
                    this.disabled = false;
                }, 2000);

                console.log(`Added product ${productId} (${productName}) to cart`);
            }, 1000);
        });
    });

    // Global search functionality (redirect to search page)
    const searchBox = document.querySelector('input[type="search"]');
    if (searchBox) {
        searchBox.addEventListener('keypress', function (e) {
            if (e.key === 'Enter') {
                const query = this.value.trim();
                if (query.length > 0) {
                    window.location.href = `/Products/Search?query=${encodeURIComponent(query)}`;
                }
            }
        });
    }

});
