# Orders Repository

In this repository, you will find a program based in .NET 9 which provides you with two API for post and get to create an order and fetch an order. 
This is a base project of what can be scaled later to be a full scale inventory-order-management system. 


The project uses .NET 9, along with PostgreSQL for database and entity framework for query handling. MediaAtR is implemented the application layer, while FluentValidation and Xunit are used to Validation and Unit testing respectively. 


The project structure is explained as follows: 

## Architectural approach
- The infrastructure was developed keeping in mind scalability and modularity. The payload provided in the examples gave a glimpse that the application could be used for e-commerce purposes and customers can place orders for items or return to purchase other items. 
Which is why the decision was made to create the following four tables which can then be scaled up in the future if the application grew. 

- Customer -> this stored trivial information such as Customer email, address and credit card number (stored in encrypted format)
- Order -> Stored information about when the order was placed and by which customer the order was placed by. It contains reference to productsordered as well which are stored as a list
- ProductOrdered -> each product ordered refers to the order it belongs to along with the product id and other information such a amount and total price. In the future, this can also be scaled up to store other values such as VAT tax, discounts etc.
- Product -> This is the inventory where product information such as name, price and inventory amount will be stored. This can also be scaled up to include more fields and meta data. 

- For the core implementation, each handler, repository action was made for specific purposes are were tried to be reduced to be as granular as possible. 
- Each handler has a specific purpose and they were responsible for communicating with various players such as validators, mappers, controllers and repositories as to avoid communication between repositories and controllers or respositories and validators and so on. This allowed each module to function on its own while also providing the opportunity to be scalable as each module had a specific task. 

- DTOs and mappers were added so communication between repositories and controllers were done via DTOs as to avoid exposing database naming conventions used. 

## Database schemas (DB)
Any database set up or queries related to schema set up can be found here. 

## Models/Domain Layer
Contains DB context, Base repository and model set up 

## Paessler.Task.Tests/Testing
Testing layer, contains unit tests and integration tests along with custom files to ensure the tests run. 

## Services (Comprising of Application, Infrastructure, Validation layer)
Here is the core logic of the application. Every folder is named with the purpose they serve and files they hold. 
DTOs are defined here to enable communication between components, handler classes are defined to allow communication between API layer and repositories. 
Validators are defined to ensure data is stored in a correct manner 
Mappers are defined to model to DTO mapping and vice versa

## WebAPI
Contains the Controllers. Exposing APIs to the outside world to enable communication with the rest of the backend 

## Running the project 
Simply clone the project via the repo

open in VS code and run the following commands
dotnet build
dotnet run 

## Running the tests

Switch to the test directory
cd Paessler.Task.Tests
dotnet test -> to run all tests 
