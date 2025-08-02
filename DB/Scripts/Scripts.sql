-- Scripts

-- PRODUCT TABLE 
CREATE SEQUENCE product_id_seq START WITH 1 INCREMENT BY 1;

CREATE TABLE product (
	id int PRIMARY KEY DEFAULT nextval('product_id_seq'),
	name text NOT NULL,
    price real NOT NULL,
    inventory_amount int NOT NULL
);

-- CUSTOMER TABLE
CREATE SEQUENCE customer_id_seq START WITH 1 INCREMENT BY 1;

CREATE TABLE customer (
    id int PRIMARY KEY DEFAULT nextval('customer_id_seq'),
    email text NOT NULL,
    address text NOT NULL,
    credit_card_number text NOT NULL
);

-- ORDER TABLE
CREATE SEQUENCE order_id_seq START WITH 1 INCREMENT BY 1;

CREATE TABLE "order" (
    id int PRIMARY KEY DEFAULT nextval('order_id_seq'),
    created_at timestamp with time zone NOT NULL,
    customer_id int NOT NULL,
    CONSTRAINT fk_customer FOREIGN KEY (customer_id) REFERENCES customer(id)
);

-- PRODUCT ORDERED
CREATE SEQUENCE product_ordered_id_seq START WITH 1 INCREMENT BY 1;


CREATE TABLE product_ordered (
    id int PRIMARY KEY DEFAULT nextval('product_ordered_id_seq'),
    order_id int NOT NULL,
    product_id int NOT NULL,
    amount int NOT NULL,
    total_price real NOT NULL,
    CONSTRAINT fk_order FOREIGN KEY (order_id) REFERENCES "order"(id),
    CONSTRAINT fk_product FOREIGN KEY (product_id) REFERENCES product(id)
);
