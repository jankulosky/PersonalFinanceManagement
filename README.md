# Personal Finance Management
PFM - Personal Finance Management is a comprehensive web application that empowers users to gain insights into their financial data, categorize transactions and manage budgets and financial goals effectively. 
It offers various activities, such as analyzing income and expense trends, organizing transactions into different categories and subcategories, and automating the categorization of transactions into income, 
expense, and savings & investment categories.

![sss](https://user-images.githubusercontent.com/79231048/182835857-8cf2b51b-402b-4449-97c1-86e6ad0f9ca3.PNG)

## Solution architecture
 
The architecture of the pfm web application follows a client-server model, where the client is the Angular front-end and the server is the .NET Core Web API back-end. 
The server-side architecture is structured using various components and patterns to achieve a scalable and maintainable solution.

### Client-Server Model:
The client-side, built with Angular, interacts with the server-side API to request and manipulate data. The server-side API processes these requests and responds with the required data or performs 
the necessary actions.

### Repository and Unit of Work Pattern:
To handle data access and management, the application uses the repository pattern and the unit of work pattern. The repository pattern provides an abstraction layer between the application and the 
underlying database, allowing for separation of concerns and easy testing. The unit of work pattern manages multiple repositories and ensures that all changes are committed or rolled back as a single 
transaction.

### Dependency Injection:
Dependency injection is used to provide loose coupling between components and enable better testability and maintainability. Dependencies, such as repositories and services, are injected into controllers 
and other components as needed.

### Database and Entities:
The application employs a PostgreSQL database to store and manage data. The entities include Transaction, Category and TransactionSplit.

### DbContext and Entity Framework:
Entity Framework is used as the ORM (Object-Relational Mapping) tool to interact with the database. The application defines a DataContext class that inherits from DbContext and contains DbSet properties for 
each entity. The DataContext class manages the mapping between the entities and the database tables.

![Untitled](https://github.com/jankulosky/PersonalFinanceManagement/assets/75507175/e4941fbd-3b1b-48e7-b66f-e2d0845bd74a)

Application features:
1. Enabling import of transactions and categories from a csv file
2. List view of transactions with filters and pagination
3. List view of categories with filter
4. Categorization of a single transaction
5. Analytics view of transactions
6. Splitting of a transaction
7. Automatically categorize transactions

![import](https://user-images.githubusercontent.com/79231048/182836261-ea42e45f-177e-45ab-9bcb-43d0478db12e.PNG)

## Exposed endpoints in Swagger

![Untitled1](https://github.com/jankulosky/PersonalFinanceManagement/assets/75507175/c8f55aa4-265b-4d6e-9e74-f50a1814eddc)

## Minimal frontend with Angular

The Personal Finance Management app also includes a minimal front-end built with Angular. The front-end comprises a user-friendly sidebar navigation menu that allows easy access to various functionalities.
The main dashboard presents transactions in a table format with pagination, enabling users to navigate through their financial data effortlessly. Additionally, the app offers the convenience of filtering 
transactions based on start and end dates. Each transaction entry is equipped with an action button that allows users to categorize individual transactions with ease, enhancing the overall financial 
management experience.


![Untitled2](https://github.com/jankulosky/PersonalFinanceManagement/assets/75507175/51acbe5c-840c-4d1f-a9d4-7926f2c2780a)
