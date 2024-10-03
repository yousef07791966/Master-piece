let productDetails = {};
document.addEventListener("DOMContentLoaded", function () {
  const productcontainer = document.getElementById("product-container");

  const urlParams = new URLSearchParams(window.location.search);
  const productId = urlParams.get("productId");

  async function fetchCategories() {
    try {
      const response = await fetch(
        `https://localhost:44350/api/Products/GetProductByID/${productId}`,
        {
          method: "GET",
        }
      );

      console.log("Response status:", response); // Log the response status

      if (response.ok) {
        const product = await response.json();
        productDetails = product;
        console.log("Fetched product:", product); // Log the fetched product

        // Create and append the product card as before
        const productCard = document.createElement("div");
        productCard.classList.add("col-lg-4", "wow", "fadeInUp");
        productCard.setAttribute("data-wow-delay", "0.2s");

        productCard.innerHTML = `
                <div class="col-md-5">
                    <div class="main-img">
                        <img class="img-fluid" src="product/1.jpg" alt="ProductS">
                        <div class="row my-3 previews"></div>
                    </div>
                </div>
                <div class="col-md-7">
                    <div class="main-description px-2">
                        <div class="category text-bold">${product.name}</div>
                        <div class="price-area my-4">
                            <p class="old-price mb-1"><del>JOD ${product.price}</del> <span class="old-price-discount text-danger">${product.priceWithDiscount}</span></p>
                            <p class="new-price text-bold mb-1">JOD ${product.price}</p>
                        </div>
                        <div class="buttons d-flex my-5">
                            <div class="block">
                                <a href="payment.html" class="shadow btn custom-btn">الدفع</a>
                            </div>
                            <div class="block">
                                <a href="##" id="addToCart" onclick="addToCart(${product})"><button class="shadow btn custom-btn">أضافة إلى السلة</button></a>
                            </div>
                            <div class="block quantity">
                                <input type="number" class="form-control" id="cart_quantity" value="1" max="" min="0" name="cart_quantity">
                            </div>
                        </div>
                    </div>
                    <div class="product-details my-4">
                        <p class="details-title text-color mb-1">معلومات عن المنتج</p>
                        <p class="description">${product.description}</p>
                    </div>
                </div>
            `;

        productcontainer.appendChild(productCard);
      } else {
        console.error("Failed response:", response);
      }
    } catch (error) {
      console.error("Error:", error);
    }
  }

  fetchCategories();
});
