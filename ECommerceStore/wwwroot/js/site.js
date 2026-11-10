document.addEventListener("DOMContentLoaded", function () {

    // Add product to cart
    document.querySelectorAll(".add-to-cart").forEach(button => {
        button.addEventListener("click", function () {

            const productId = this.dataset.id;
            const productName = this.dataset.name;
            const productPrice = parseFloat(this.dataset.price);
            const productImage = this.dataset.image;

            let cart = JSON.parse(localStorage.getItem("cart")) || [];

            const existingProduct = cart.find(item => item.id == productId);

            if (existingProduct) {
                existingProduct.quantity++;
            } else {
                cart.push({
                    id: productId,
                    name: productName,
                    price: productPrice,
                    image: productImage,
                    quantity: 1
                });
            }

            localStorage.setItem("cart", JSON.stringify(cart));

            updateCartCount();

            alert(`${productName} added to cart!`);
        });
    });

    // Update cart count
    function updateCartCount() {
        const cart = JSON.parse(localStorage.getItem("cart")) || [];

        const count = cart.reduce((total, item) => {
            return total + item.quantity;
        }, 0);

        const cartCount = document.getElementById("cart-count");

        if (cartCount) {
            cartCount.textContent = count;
        }
    }

    // Display cart items
    function displayCart() {

        const cart = JSON.parse(localStorage.getItem("cart")) || [];
        const cartContainer = document.getElementById("cart-items");

        if (!cartContainer) return;

        cartContainer.innerHTML = "";

        if (cart.length === 0) {
            cartContainer.innerHTML = `
                <div class="text-center py-5">
                    <h4>Your cart is empty</h4>
                    <a href="/Product" class="btn btn-primary mt-3">
                        Continue Shopping
                    </a>
                </div>
            `;

            updateCartTotal();
            return;
        }

        cart.forEach(item => {

            cartContainer.innerHTML += `
                <div class="cart-item d-flex align-items-center 
                            justify-content-between border-bottom py-3">

                    <div class="d-flex align-items-center">

                        <img src="${item.image}"
                             alt="${item.name}"
                             width="80"
                             height="80"
                             class="rounded me-3"
                             style="object-fit: cover;">

                        <div>
                            <h5>${item.name}</h5>
                            <p class="mb-0">
                                ₹${item.price.toFixed(2)}
                            </p>
                        </div>

                    </div>

                    <div class="d-flex align-items-center gap-2">

                        <button class="btn btn-sm btn-outline-secondary"
                                onclick="changeQuantity('${item.id}', -1)">
                            −
                        </button>

                        <span>${item.quantity}</span>

                        <button class="btn btn-sm btn-outline-secondary"
                                onclick="changeQuantity('${item.id}', 1)">
                            +
                        </button>

                        <button class="btn btn-sm btn-danger"
                                onclick="removeFromCart('${item.id}')">
                            Remove
                        </button>

                    </div>

                </div>
            `;
        });

        updateCartTotal();
    }

    // Change quantity
    window.changeQuantity = function (productId, change) {

        let cart = JSON.parse(localStorage.getItem("cart")) || [];

        const product = cart.find(item => item.id == productId);

        if (!product) return;

        product.quantity += change;

        if (product.quantity <= 0) {
            cart = cart.filter(item => item.id != productId);
        }

        localStorage.setItem("cart", JSON.stringify(cart));

        displayCart();
        updateCartCount();
    };

    // Remove product
    window.removeFromCart = function (productId) {

        let cart = JSON.parse(localStorage.getItem("cart")) || [];

        cart = cart.filter(item => item.id != productId);

        localStorage.setItem("cart", JSON.stringify(cart));

        displayCart();
        updateCartCount();
    };

    // Calculate total
    function updateCartTotal() {

        const cart = JSON.parse(localStorage.getItem("cart")) || [];

        const total = cart.reduce((sum, item) => {
            return sum + (item.price * item.quantity);
        }, 0);

        const cartTotal = document.getElementById("cart-total");

        if (cartTotal) {
            cartTotal.textContent = `₹${total.toFixed(2)}`;
        }
    }

    // Initial loading
    updateCartCount();
    displayCart();

});
const SuccessMsg = document.getElementById("SuccessMesage");
setTimeout(function () {
    if (SuccessMsg) {
        SuccessMsg.style.display = "None";
    }
}, 5000);

