document.addEventListener("DOMContentLoaded", function () {
  const categoryContainer = document.getElementById("category");
  async function fetchCategories() {
    try {
      const response = await fetch(
        "https://localhost:44350/api/Category/GetAllCategory",
        {
          method: "get",
        }
      );

      if (response.ok) {
        const categories = await response.json();

        categories.forEach((category) => {
          const categoryItem = document.createElement("a");
          categoryItem.href = `feature.html?categoryId=${category.categoryId}`;
          categoryItem.className = "dropdown-item";
          categoryItem.textContent = category.name;

          categoryContainer.appendChild(categoryItem);
        });
      } else {
        console.error("Failed response:", response);
      }
    } catch (error) {
      console.error("Error:", error);
    }
  }

  fetchCategories();
});
