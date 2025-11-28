# SupplierPortal

Project summary  
Purchase requests sent to suppliers will be priced by the supplier and returned as feedback.

Architecture — DDD + Clean Architecture  
This repository implements Domain-Driven Design (DDD) together with Clean Architecture (onion/hexagonal layering). Responsibilities and dependencies are organized so that inner layers (Domain) contain business rules and outer layers (Infrastructure, Presentation) depend on abstractions defined by the Application layer.

High-level layers
- Domain (core): entities, value objects, domain enums, domain behavior and business rules. No external framework references.
  - Example aggregates: `PurchaseRequest` (root) with `PurchaseRequestItem` children; `Account`.
- Application (use-cases): DTOs, application services, validators, repository interfaces (e.g., `IPurchaseRequestRepository`), mapping profiles.
- Infrastructure: EF Core DbContext, entity configurations (e.g., `PurchaseRequestConfiguration`), repository implementations, SMTP/email service.
- Presentation/API: Web API controllers and Blazor UI. Presentation calls Application services or the API and consumes DTOs.

Why this approach
- Keeps business rules inside domain objects (encapsulation of invariants).
- Makes application logic testable and independent from persistence or frameworks.
- Allows swapping infrastructure implementations without changing domain/use-cases.
- Stabilizes API surface by using DTOs rather than exposing EF entities directly.

Quick start (local)
- Requirements: .NET SDK, SQL Server (or other supported DB).
- Configure connection string in the Web API `appsettings.json`.
- Apply EF migrations (project containing DbContext):
  - `dotnet ef database update`
- Run projects:
  - API: `dotnet run --project src/SupplierPortal.WebApi`
  - Blazor App: `dotnet run --project src/SupplierPortal.Blazor`
- Open the Blazor app in your browser (default: `https://localhost:5001` or `http://localhost:5000`).

Postman examples (replace `{{API_BASE}}`)
- Create supplier:
  - `POST {{API_BASE}}/api/suppliers`
  - Body: `{ "code":"S001","title":"Vendor A","phone":"...","address":"...","username":"user","password":"pass" }`
- Create purchase request:
  - `POST {{API_BASE}}/api/purchase-requests` (header + items)
- Update item price (supplier action):
  - `PUT {{API_BASE}}/api/purchase-requests/{requestId}/items/{itemId}`
  - Body: `{ "price": 123.45, "deliveryDate": "2025-12-01T00:00:00" }`
- Get completed requests:
  - `GET {{API_BASE}}/api/purchase-requests/completed`

Recommendations & next steps
- Keep use-case orchestration in Application services; domain entities expose behavior (e.g., `MarkAsCompleted`, `UpdateItemPrice`) to enforce rules.
- Define repository interfaces in Application and implement them in Infrastructure (EF Core).
- Map EF entities to DTOs before returning from controllers to avoid exposing persistence concerns.
- Add unit tests for domain rules and integration tests for repositories.
- Add a Postman collection and CI pipeline (GitHub Actions) to run builds and tests.

License & contribution
- Add a LICENSE file (for example MIT) before publishing.
- Create feature branches and open pull requests. Follow the repository layering and coding conventions.

If you want, I can generate:
- A sample Postman collection JSON,
- Interface stubs and DI wiring examples,
- A unit test template for a domain method (e.g., `PurchaseRequest.MarkAsCompleted()`).