CREATE DATABASE E_Commerce_master;
USE E_Commerce_master;


-- Create Category Table
CREATE TABLE Category (
    category_id INT IDENTITY(1,1)  PRIMARY KEY,
    name NVARCHAR(255),
    image NVARCHAR(MAX),
    description NVARCHAR(MAX)
);

-- Create Products Table
CREATE TABLE Products (
    product_id INT IDENTITY(1,1)  PRIMARY KEY ,
    name NVARCHAR(255),
    description NVARCHAR(MAX),
    image NVARCHAR(MAX),
    price DECIMAL(10, 2),
    category_id INT,
    brand NVARCHAR(100),
    price_with_discount DECIMAL(10, 2),
    FOREIGN KEY (category_id) REFERENCES Category(category_id)
);

-- Create Users Table
CREATE TABLE Users (
    user_id INT IDENTITY(1,1)  PRIMARY KEY,
    name NVARCHAR(255) NOT NULL,
    email VARCHAR(255)NOT NULL,
    image NVARCHAR(MAX),
    password NVARCHAR(255)NOT NULL,
    passwordHash VARBINARY(MAX),
    passwordSalt VARBINARY(MAX),
    address NVARCHAR(255),
    points INT,
    phone_number VARCHAR(20)
);



-- Create Admin Table
CREATE TABLE Admin (
    admin_id INT IDENTITY(1,1)  PRIMARY KEY,
    name NVARCHAR(255)NOT NULL,
    email VARCHAR(255)NOT NULL,
    img NVARCHAR(MAX),
    password VARCHAR(255)NOT NULL,
    passwordHash VARBINARY(MAX),
    passwordSalt VARBINARY(MAX)
);

-- Create Contact Table
CREATE TABLE Contact (
    contact_id INT IDENTITY(1,1)  PRIMARY KEY,
    name NVARCHAR(255) NOT NULL,
    email VARCHAR(255) NOT NULL,
    subject NVARCHAR(255),
    message NVARCHAR(MAX),
    sent_date DATE,
    admin_response NVARCHAR(MAX),
    response_date DATE,
    status NVARCHAR(255) DEFAULT 'PENDING'
);

-- Create Coupons Table
CREATE TABLE Copons (
    copon_id INT IDENTITY(1,1)  PRIMARY KEY,
    name NVARCHAR(255),
    DiscountAmount  DECIMAL(10, 2),
    ExpiryDate DATETIME  ,
	CreatedAt DATETIME DEFAULT GETDATE(),
	IsUsed BIT DEFAULT 0,
	user_id INT ,
	 FOREIGN KEY (user_id) REFERENCES Users(user_id)

);

-- Create Orders Table
CREATE TABLE Orders (
    order_id INT IDENTITY(1,1)  PRIMARY KEY,
    user_id INT,
    amount DECIMAL(10, 2),
    copon_id INT,
    status INT,
    transaction_id NVARCHAR(MAX),
    FOREIGN KEY (user_id) REFERENCES Users(user_id),
    FOREIGN KEY (copon_id) REFERENCES Copons(copon_id)
);

-- Create Cart_Item Table
CREATE TABLE Cart_Item (
    cart_item_id INT IDENTITY(1,1)  PRIMARY KEY,
    product_id INT,
    user_id INT,
    quantity INT,
    FOREIGN KEY (product_id) REFERENCES Products(product_id),
    FOREIGN KEY (user_id) REFERENCES Users(user_id)
);

-- Create Order_Item Table
CREATE TABLE Order_Item (
    order_item_id INT IDENTITY(1,1)  PRIMARY KEY,
    order_id INT,
    product_id INT,
    quantity INT,
    FOREIGN KEY (order_id) REFERENCES Orders(order_id),
    FOREIGN KEY (product_id) REFERENCES Products(product_id)
);

-- Create Comment Table
CREATE TABLE Comment (
    comment_id INT IDENTITY(1,1)  PRIMARY KEY,
    user_id INT,
    product_id INT,
    comment NVARCHAR(MAX),
    status  NVARCHAR(255) DEFAULT 'PENDING',
    date DATE,
    rating INT,
    FOREIGN KEY (user_id) REFERENCES Users(user_id),
    FOREIGN KEY (product_id) REFERENCES Products(product_id)
);    


CREATE TABLE Payments (
    payment_id INT IDENTITY(1,1) PRIMARY KEY,
    user_id INT NOT NULL,
    amount DECIMAL(10, 2) NOT NULL,
    payment_method NVARCHAR(50),
    transaction_id NVARCHAR(100),
    payment_status NVARCHAR(50),
    payment_date DATETIME DEFAULT GETDATE(), 
    FOREIGN KEY (user_id) REFERENCES Users(user_id) 
); 




CREATE TABLE Messages (
    Id INT PRIMARY KEY IDENTITY(1,1), 
    Sender NVARCHAR(MAX) NULL,         
    Recipient NVARCHAR(MAX) NULL,     
    MessageContent NVARCHAR(MAX) NULL, 
    Timestamp DATETIME NULL            
);


CREATE TABLE ChatMessages (
    Id INT PRIMARY KEY IDENTITY(1,1), 
    Sender NVARCHAR(100) NULL, 
    Recipient NVARCHAR(100) NULL, 
    MessageContent NVARCHAR(MAX) NULL, 
    SentAt DATETIME NULL
);


CREATE TABLE Cart (
    CartId INT PRIMARY KEY IDENTITY(1,1), 
    user_id INT NULL,                      
     FOREIGN KEY (user_id) REFERENCES Users(user_id)
);

CREATE TABLE Chat (
    ChatId INT PRIMARY KEY IDENTITY(1,1), 
    user_id INT NULL,                      
    FOREIGN KEY (user_id) REFERENCES Users(user_id) 
);


CREATE TABLE ChatMessage1 (
    Id INT PRIMARY KEY IDENTITY(1,1),          
    Sender NVARCHAR(MAX) NULL,                
    Recipient NVARCHAR(MAX) NULL,             
    MessageContent NVARCHAR(MAX) NULL,         
    SentAt DATETIME NULL                       
);
CREATE TABLE Voucher (
    Id INT PRIMARY KEY IDENTITY(1,1),           
    Code NVARCHAR(255) NOT NULL,                
    DiscountAmount DECIMAL(18, 2) NOT NULL,   
    ExpiryDate DATETIME NOT NULL,              
    IsUsed BIT NOT NULL,                        
    CreatedAt DATETIME NOT NULL                 
);
