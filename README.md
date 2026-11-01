# Product Distribution System

A C# domain model for deciding where product instances should be stored. Distribution behaviour is assembled from small rules that can validate products, route them by category, write logs, or place them into containers.

## What the project models

| Component | Responsibility |
| --- | --- |
| `Sku` and `ProductInstance` | Describe a concrete product, its category, dimensions, weight, and expiration date. |
| `Container` | Defines the maximum weight and volume available for stored products. |
| `ProductGroup` | Describes categories that are allowed to be stored together. |
| `ContainerService` | Enforces container capacity and product-neighbourhood rules. |
| `IDistributionRule` | Provides a common contract for every distribution decision. |

## Available distribution rules

- `CategoryRouterRule` selects a rule according to the product category;
- `ContainerDistributionRule` attempts to place a product into a container;
- `ExpiredDistributionRule` accepts only expired products;
- `NonExpiredDistributionRule` accepts only products that have not expired;
- `PalletDistributionRule` tries several destination rules in sequence;
- `LoggingDistributionRule` records the result without coupling the domain model to a specific logger.

Rules can wrap or delegate to other rules. This makes it possible to build different distribution pipelines without modifying existing rule implementations.

## Design highlights

- dependencies such as repositories, logging, time, and container operations are represented by interfaces;
- domain operations return typed success or failure results;
- value objects encapsulate identifiers, categories, weight, and volume;
- infrastructure can be replaced with test doubles without changing domain logic;
- new routing or validation rules can be added through the common rule abstraction.

## Repository structure

```text
src/ProductDistribution/          Domain model and distribution rules
tests/ProductDistribution.Tests/ Unit and integration-style domain tests
```

## Requirements

- .NET SDK 10.0 or newer

## Build and test

```bash
dotnet restore ProductDistribution.slnx
dotnet build ProductDistribution.slnx --no-restore
dotnet test ProductDistribution.slnx --no-build --no-restore
```

The same commands run automatically in GitHub Actions for every push and pull request.
