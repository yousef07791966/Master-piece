INSERT INTO Products (name, description, image, price, category_id, brand, price_with_discount)
VALUES
-- Electronics (category_id = 1)
('Smartphone X10', 'Latest smartphone with 128GB storage and 5G support.', 'images\hand tools\hand1.jpg', 699.99, 1, 'TechCorp', 649.99),
('Laptop Pro', 'High-performance laptop with 16GB RAM and 512GB SSD.', 'images\hand tools\hand1.jpg', 1199.99, 1, 'CompTech', 1099.99),

-- Home Appliances (category_id = 2)
('Refrigerator XL', 'Spacious refrigerator with energy-saving technology.', 'images\hand tools\hand1.jpg', 899.99, 2, 'HomeCool', 849.99),
('Washing Machine 3000', 'Front-load washing machine with smart features.', 'images\hand tools\hand1.jpg', 499.99, 2, 'CleanTech', 469.99),

-- Fashion (category_id = 3)
('Mens Leather Jacket', 'Stylish leather jacket for men.', 'images\hand tools\hand1.jpg', 149.99, 3, 'FashionBrand', 129.99),
('Women\ Running Shoes', 'Comfortable and lightweight running shoes.', 'images\hand tools\hand1.jpg', 89.99, 3, 'SportyStyle', 79.99),

-- Sports Equipment (category_id = 4)
('Mountain Bike', 'Durable mountain bike suitable for off-road trails.', 'images\hand tools\hand1.jpg', 399.99, 4, 'BikeMaster', 349.99),
('Yoga Mat', 'Non-slip yoga mat for indoor and outdoor use.', 'images\hand tools\hand1.jpg', 29.99, 4, 'FitnessPro', 24.99),

-- Books (category_id = 5)
('The Great Novel', 'A captivating story about adventure and self-discovery.', 'images\hand tools\hand1.jpg', 19.99, 5, 'BookWorm', 14.99),
('Science Encyclopedia', 'Comprehensive encyclopedia covering various scientific fields.', 'images\hand tools\hand1.jpg', 59.99, 5, 'KnowledgePress', 49.99);
