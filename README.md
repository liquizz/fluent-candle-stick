# FluentCandleStick

A candlestick chart visualization application created as a test task for a company interview.

## Overview

FluentCandleStick is a full-stack application for displaying financial market data in candlestick chart format. It features a clean, modern UI built with Angular and Fluent UI components, backed by a .NET Core API server.

## Architecture

The project follows a clean architecture pattern with the following components:

- **FluentCandleStick.Server**: ASP.NET Core Web API
- **FluentCandleStick.Application**: Application business logic layer
- **FluentCandleStick.Domain**: Domain entities and business rules
- **FluentCandleStick.Database**: Data access layer
- **fluentcandlestick.client**: Angular frontend application

## Features

- Import market data from CSV files
- Display financial data in interactive candlestick charts
- Modern UI with Fluent UI components
- Clean, layered architecture

## Technologies

### Backend
- ASP.NET Core
- Entity Framework Core
- MediatR for CQRS pattern
- SQLite database

### Frontend
- Angular 17
- Fluent UI Web Components
- Lightweight Charts library for candlestick visualization
- RxJS for reactive programming

## Getting Started

### Prerequisites
- .NET 8.0 SDK
- Node.js and npm
- Angular CLI

### Running the Application

1. Clone the repository
2. Build and run the backend and frontend:
   ```
   dotnet run --project=FluentCandleStick.Server
   ```
3. Navigate to `https://localhost:4200` in your browser

## Test Data

A sample market data file `MarketDataTest.csv` is included in the repository for testing the import functionality. 