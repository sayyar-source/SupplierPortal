# Purchase Request Management System

A complete end-to-end **Purchase Request Management System** built with **ASP.NET Core MVC / API**, supporting supplier-based filtering, item updates, authorization (JWT token), and clean API usage.

This README provides a clear guide on how to run the project, understand the architecture, and use all endpoints.

---

## 🚀 Features

* Create purchase requests
* Update purchase request items
* Get purchase requests by supplier
* Secure API using JWT authorization
* Strong separation of concerns (Controller → Service → Repository)
* DTO-based clean API communication
* Supports async operations

---

## 🏗️ ## Architecture — Fullstack (API + Blazor) using DDD & Clean Architecture

This solution is a **fullstack application** composed of:

* **Blazor Frontend** — client UI for suppliers and internal users
* **Web API Backend** — REST endpoints exposing application use‑cases
* **Application Layer** — use‑case orchestration, DTOs, service logic, repository interfaces
* **Domain Layer** — enterprise business rules, aggregates, entities, value objects, domain events
* **Infrastructure Layer** — EF Core, repository implementations, DbContext, external integrations

The architecture follows strict DDD and Clean Architecture rules:

* **Domain is pure C#** with no external dependencies
* **Application depends only on Domain** and defines contracts for Infrastructure
* **Infrastructure implements Application contracts** (repositories, SMTP/email, persistence)
* **Web API + Blazor depend on Application**, never on Infrastructure directly

This structure ensures flexibility, testability, and maintainability across the entire fullstack system. Overview

```
┌───────────────────────────┐
│   ASP.NET Core API Layer  │
└──────────────┬────────────┘
               │
┌──────────────┴────────────┐
│     Application Layer     │
│  (Services, DTO Mapping)  │
└──────────────┬────────────┘
               │
┌──────────────┴────────────┐
│       Repository Layer     │
│   (EF Core, Database Ops)  │
└──────────────┬────────────┘
               │
┌──────────────┴────────────┐
│         SQL Database       │
└────────────────────────────┘
```

---

## ⚙️ How to Run

### 1. Clone the repository

```
git clone https://github.com/yourrepo/purchase-request-api.git
cd purchase-request-api
```

### 2. Update database

```
dotnet ef database update
```

### 3. Run the API

```
dotnet run
```

API will be available at:

```
https://localhost:5001/api
```

---

## 🔐 Authentication (JWT)

All API calls require a valid JWT token.

Example request header:

```
Authorization: Bearer <your_token_here>
```

### How to add token in HttpClient (C#)

```csharp
client.DefaultRequestHeaders.Authorization =
    new AuthenticationHeaderValue("Bearer", token);
```

---

## 📡 API Endpoints

### ✅ Get Requests by Supplier

**GET** `/api/PurchaseRequests/supplier/{supplierId}`

```http
GET /api/PurchaseRequests/supplier/3
Authorization: Bearer <token>
```

**Response:**

```json
[
  {
    "id": 1,
    "supplierId": 3,
    "notes": "Urgent order",
    "items": [
      {
        "id": 10,
        "productName": "Steel Pipe",
        "quantity": 50,
        "unit": "Piece",
        "isPriced": true,
        "price": 120.5
      }
    ]
  }
]
```

---

### 📝 Update Purchase Request Item

**PUT** `/api/PurchaseRequests/{purchaseRequestId}/item`

#### Request Body

```json
{
  "itemId": 12,
  "productName": "Updated Name",
  "quantity": 40,
  "unit": "Piece",
  "isPriced": true,
  "price": 85.75
}
```

#### Controller Method

```csharp
[HttpPut("{purchaseRequestId}/item")]
public async Task<ActionResult> UpdateRequestItem(int purchaseRequestId, [FromBody] UpdatePurchaseRequestItemDTO updateDto)
{
    await _purchaseRequestService.UpdatePurchaseRequestItemAsync(purchaseRequestId, updateDto);
    return NoContent();
}
```

---

## 🧪 Sample C# Client Usage

```csharp
string url = $"api/PurchaseRequests/supplier/{supplierId}";
client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

var response = await client.GetAsync(url);
response.EnsureSuccessStatusCode();

var purchases = await response.Content.ReadFromJsonAsync<IEnumerable<PurchaseRequestDTO>>();
return purchases ?? Enumerable.Empty<PurchaseRequestDTO>();
```

---

## 📄 Sample JSON for Create Purchase Request

```json
{
  "supplierId": 1,
  "notes": "string",
  "items": [
    {
      "productName": "string",
      "quantity": 10,
      "unit": "Box"
    }
  ]
}
```

---

## 🧰 Technologies Used

* ASP.NET Core 8/9 API
* Entity Framework Core
* JWT Authentication
* Serilog Logging
* SQL Server or PostgreSQL
* Clean Architecture principles

---

## 📧 Contact

Created by **Mohammad**

If you need help or want a full project file, feel free to ask!

---

## ✔️ License

MIT License — free to use and modify.
