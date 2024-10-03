let products = [];
$(document).ready(function () {
  products = JSON.parse(localStorage.getItem("products"));

  // Display products in the table
  displayProducts(products);
});

function displayProducts(products) {
  let tableBody = $("#productTable tbody");
  tableBody.empty();

  for (let i = 0; i < products.length; i++) {
    let product = products[i];
    let row = $("<tr>");
    row.append(
      `<td><button class="btn btn-danger btn-sm delete-product" data-id="${product.productId}">Delete</button></td>`
    );
    row.append(`<td><img src="${product.image}" width="100" /></td>`);
    row.append(`<td>${product.name}</td>`);
    row.append(`<td>${product.price}</td>`);
    row.append(
      `<td class="product_quantity"><label>الكمية</label> <input data-id="${
        product.productId
      }" class="quantity" min="1" max="100" value="${
        product.quantity || 1
      }" type="number"></td>`
    );
    row.append(`<td>${product.price * product.quantity}</td>`);
    tableBody.append(row);
  }
}

$(document).on("click", ".delete-product", function (e) {
  e.preventDefault();
  deleteProduct($(e.target).attr("data-id"));
});
$(document).on("change", ".quantity", function (e) {
  e.preventDefault();
  editQuantity($(e.target).attr("data-id"), $(e.target).val());
});

function deleteProduct(id) {
  let index = products.findIndex(
    (product) => product.productId === parseInt(id)
  );
  if (index !== -1) {
    products.splice(index, 1);
    localStorage.setItem("products", JSON.stringify(products));
    displayProducts(products);
  }
}

function editQuantity(id, quantity) {
  let product = products.find((product) => product.productId === parseInt(id));
  if (product) {
    product.quantity = parseInt(quantity);
    localStorage.setItem("products", JSON.stringify(products));
    displayProducts(products);
  }
}
