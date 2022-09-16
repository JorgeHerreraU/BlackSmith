# BlackSmith - Medical Center Application
#### Demonstration project application built in [WPF UI](https://github.com/lepoco/wpfui)  to schedule medical appointments

![image](https://user-images.githubusercontent.com/34349731/188518116-ba5b72f7-8830-4552-ab8b-a3894eb12007.png)

![GitHub repo size](https://img.shields.io/github/repo-size/JorgeHerreraU/ProductsApi?style=for-the-badge)
![GitHub language count](https://img.shields.io/github/languages/count/JorgeHerreraU/ProductsApi?style=for-the-badge)
![Github open issues](https://img.shields.io/github/issues/JorgeHerreraU/ProductsApi?style=for-the-badge)

## 💽 Download
Go to releases or click [here](https://github.com/JorgeHerreraU/BlackSmith/releases/download/blacksmith%2Fv1.0.0/BlackSmith.Presentation.exe)

## 📐 Infrastructure
- **Presentation Layer**
  - This is the "Desktop Client Application"
  - Based on [WPF](https://docs.microsoft.com/en-us/visualstudio/designers/getting-started-with-wpf?view=vs-2022) Framework & [AdonisUI](https://benruehl.github.io/adonis-ui/) toolkit 
  - Designed with MVVM (Model-View-ViewModel) Pattern
  - Consumes Application Services Layer


- **Service Layer** 
  - Contains application services
  - Exposes the business logic through interfaces
  - Bridge between Presentation Layer and Domain Layer
  - Has Dto definitions to be able to return data without exposing the domain entities
  - Dto soft validations using C# built-in Data Annotations
  - Register injection dependencies


- **Business Layer**
  - Centralizes business rules ensuring data consistency and validity
  - [FluentValidation](https://docs.fluentvalidation.net/en/latest/) library is used for strongly-typed validations rules
  - Complex validations implemented against the database
  - Serves as an intermediary for data exchange between the Presentation Layer and the Data Access Layer


- **Domain Layer**
  - Domain entities 
  - Declares the IRepository interface bridging the data layer 


- **Data Access Layer**
  - Database infrastructure (mapping)
  - Implementation of the Repository interface
  - Holds database migrations
  - Separate the EF code needed for generating database tables at design-time from EF code used by your application at runtime


- **Core Layer**
  - Exposes Helpers, Extension Methods, Data Structures shared across layers
  - Common functionality (crosscutting concerns)


## 💻 Requirements

Before you begin:
* [.NET 6 Desktop Runtime](https://dotnet.microsoft.com/en-us/download/dotnet/6.0)

## 🚀 Build

Building back-end
```
dotnet build
```
