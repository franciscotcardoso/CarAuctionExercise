# Car Auction Management

![Build and Test Status](https://github.com/franciscotcardoso/CarAuctionExercise/actions/workflows/build_and_test.yml/badge.svg)

## Description

This project is a simple car auction management system. It manages the vehicles inventory and their auction process.

## Project details

### Structure

The project adheres to Clean Architecture principles and follows Domain-Driven Design (DDD), ensuring that the system is modular,
maintainable, and scalable. The architecture divides the application into distinct layers, each with its own responsibility:

- API Layer: Handles incoming HTTP requests, delegates them to the application layer to process business logic,
and returns appropriate responses.
- Application Layer: Contains the application-specific business logic.
- Domain Layer: Encapsulates the core business entities.
- Infrastructure Layer: Handles data persistence and external integrations. In this project, the data is hold in memory,
but in a real-world scenario, this layer would connect to a database or external services.

### Business logic details

There are two main domain entities: Vehicles and Auctions. Each auction can have a vehicle and multiple bids for it.
The domain entities are managed by two main services in the Application layer. The entities are stored in-memory and
managed used the repository and the specification patterns.

The communication between the API layer and Application layer is done by specific DTOs that are mapped to the domain entities.
The actions over the domain entities are mainly validated by rules using `FluentValidation` in the Application layer: 
`AuctionValidator.cs` and `VehicleValidator.cs`. There also some rules directly validated in the Application layer services.

#### Main rules:

- Can't create a vehicle with the same license plate more than once. **License plate is the vehicle unique identifier;**
- Can't be created more than one auction for the same vehicle;
- Can't create an auction with a starting bid with a value less than 1;
- Can't be created an auction for a vehicle that it's not in the inventory;
- Can't start an auction that was not created;
- Can't start an auction already closed;
- Can't close an auction that was not created;
- Can't close an auction that has not yet started;
- Can't place a bid for auction that is not created;
- Can't place a bid for an auction that has not yet started;
- Can't place a bid with value less that the starting bid or previous value;
- Vehicles year can't be greater than the current year;
- Number of doors is mandatory for Hatchbacks and Sedans and the values must be between 3 and 5;
- Number of seats is mandatory for SUVs and the values must be between 5 and 8;
- Load capacity is mandatory for Trucks and the values must be between 10000 and 50000;
- Load capacity isn't allowed for Hatchbacks, Sedans and SUVs;
- Number of seats isn't allowed for Hatchbacks, Sedans and Trucks;
- Number of doors isn't allowed for SUVs and Trucks;

### Tests

The project includes both unit tests and integration tests. The unit tests focus on the application layer services,
which handle the core business logic. The integration tests validate the interaction between the controllers,
the application layer, and the domain layer, ensuring that the components work together as expected.

## Docker

The project can be run using Docker. To build and start the application, simply run `docker-compose up`.

## Build and test pipeline

The GitHub repository has a build and test pipeline that runs every time a new pull request is open to the main branch.

## Insomnia collection / Swagger

There is (at the project root) an Insomnia collection names `CarAucationCollectionInsomnia.json` that has all the available endpoints.
The swagger is also available in the project.

