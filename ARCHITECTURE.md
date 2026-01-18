# Renault Smart Center - Enterprise ERP System
## Phase 1: Architecture & Project Structure

---

## 🏗️ SYSTEM ARCHITECTURE OVERVIEW

### Clean Architecture + DDD Principles

```
┌─────────────────────────────────────────────────────────────┐
│                        BLAZOR SERVER UI                       │
│  (Presentation Layer - User Interface & Components)          │
└─────────────────────────────────────────────────────────────┘
                              ↕
┌─────────────────────────────────────────────────────────────┐
│                    ASP.NET CORE WEB API                       │
│  (API Controllers, JWT Authentication, Request/Response)     │
└─────────────────────────────────────────────────────────────┘
                              ↕
┌─────────────────────────────────────────────────────────────┐
│                     APPLICATION LAYER                         │
│  (Use Cases, DTOs, Interfaces, Services, Validation)         │
└─────────────────────────────────────────────────────────────┘
                              ↕
┌─────────────────────────────────────────────────────────────┐
│                       DOMAIN LAYER                            │
│  (Entities, Value Objects, Domain Events, Business Logic)    │
└─────────────────────────────────────────────────────────────┘
                              ↕
┌─────────────────────────────────────────────────────────────┐
│                    INFRASTRUCTURE LAYER                       │
│  (EF Core, Repositories, External Services, File Storage)    │
└─────────────────────────────────────────────────────────────┘
                              ↕
┌─────────────────────────────────────────────────────────────┐
│                      SQL SERVER DATABASE                      │
│  (Code First Migrations, Tables, Indexes, Constraints)       │
└─────────────────────────────────────────────────────────────┘
```

---

## 📁 PROJECT STRUCTURE

### Solution: `RenaultSmartCenter.sln`

```
RenaultSmartCenter/
├── src/
│   ├── RenaultSmartCenter.Domain/              # Domain Layer
│   │   ├── Entities/
│   │   │   ├── Branch.cs
│   │   │   ├── User.cs
│   │   │   ├── Role.cs
│   │   │   ├── Customer.cs
│   │   │   ├── Vehicle.cs
│   │   │   ├── ServiceOrder.cs
│   │   │   ├── ServiceOrderItem.cs
│   │   │   ├── Appointment.cs
│   │   │   ├── SparePart.cs
│   │   │   ├── InventoryTransaction.cs
│   │   │   ├── Supplier.cs
│   │   │   ├── Invoice.cs
│   │   │   ├── InvoiceItem.cs
│   │   │   ├── Payment.cs
│   │   │   └── AuditLog.cs
│   │   ├── ValueObjects/
│   │   │   ├── Address.cs
│   │   │   └── Money.cs
│   │   ├── Enums/
│   │   │   ├── ServiceType.cs
│   │   │   ├── ServiceOrderStatus.cs
│   │   │   ├── AppointmentStatus.cs
│   │   │   ├── PaymentMethod.cs
│   │   │   ├── TransactionType.cs
│   │   │   └── UserRole.cs
│   │   └── Interfaces/
│   │       └── IAuditable.cs
│   │
│   ├── RenaultSmartCenter.Application/         # Application Layer
│   │   ├── Common/
│   │   │   ├── DTOs/
│   │   │   ├── Mappings/
│   │   │   └── Validators/
│   │   ├── Features/
│   │   │   ├── Authentication/
│   │   │   ├── Dashboard/
│   │   │   ├── Customers/
│   │   │   ├── Vehicles/
│   │   │   ├── ServiceOrders/
│   │   │   ├── Appointments/
│   │   │   ├── Inventory/
│   │   │   ├── Billing/
│   │   │   └── Reports/
│   │   ├── Interfaces/
│   │   │   ├── IRepository.cs
│   │   │   ├── IUnitOfWork.cs
│   │   │   └── [Feature]Service interfaces
│   │   └── ApplicationServiceRegistration.cs
│   │
│   ├── RenaultSmartCenter.Infrastructure/      # Infrastructure Layer
│   │   ├── Data/
│   │   │   ├── ApplicationDbContext.cs
│   │   │   ├── Configurations/
│   │   │   │   └── [Entity]Configuration.cs (EF Configurations)
│   │   │   └── SeedData/
│   │   ├── Repositories/
│   │   │   └── GenericRepository.cs
│   │   ├── Identity/
│   │   │   ├── ApplicationUser.cs
│   │   │   └── ApplicationRole.cs
│   │   ├── Services/
│   │   │   ├── JwtService.cs
│   │   │   ├── PdfService.cs
│   │   │   └── NotificationService.cs (Placeholders)
│   │   └── InfrastructureServiceRegistration.cs
│   │
│   ├── RenaultSmartCenter.API/                 # Web API Layer
│   │   ├── Controllers/
│   │   │   ├── AuthController.cs
│   │   │   ├── DashboardController.cs
│   │   │   ├── CustomersController.cs
│   │   │   ├── VehiclesController.cs
│   │   │   ├── ServiceOrdersController.cs
│   │   │   ├── AppointmentsController.cs
│   │   │   ├── InventoryController.cs
│   │   │   ├── BillingController.cs
│   │   │   └── ReportsController.cs
│   │   ├── Middleware/
│   │   │   └── ExceptionHandlingMiddleware.cs
│   │   ├── Program.cs
│   │   └── appsettings.json
│   │
│   └── RenaultSmartCenter.BlazorUI/            # Blazor Server UI
│       ├── Components/
│       │   ├── Layout/
│       │   ├── Common/
│       │   └── Pages/
│       │       ├── Dashboard/
│       │       ├── Customers/
│       │       ├── Vehicles/
│       │       ├── ServiceOrders/
│       │       ├── Appointments/
│       │       ├── Inventory/
│       │       ├── Billing/
│       │       └── Reports/
│       ├── Services/
│       │   ├── ApiClient.cs
│       │   └── AuthenticationService.cs
│       ├── wwwroot/
│       │   ├── css/
│       │   └── js/
│       ├── Program.cs
│       └── App.razor
│
├── tests/
│   ├── RenaultSmartCenter.Domain.Tests/
│   └── RenaultSmartCenter.Application.Tests/
│
├── RenaultSmartCenter.sln
├── README.md
└── ARCHITECTURE.md
```

---

## 🗄️ DATABASE DESIGN (SQL Server)

### Core Tables:

1. **Branches** - Multi-branch support
2. **AspNetUsers / AspNetRoles** - Identity tables
3. **Customers** - Customer information
4. **Vehicles** - Vehicle details (linked to customers)
5. **ServiceOrders** - Work orders
6. **ServiceOrderItems** - Service items within orders
7. **Appointments** - Scheduled appointments
8. **SpareParts** - Inventory catalog
9. **InventoryTransactions** - Stock movements
10. **Suppliers** - Supplier management
11. **Invoices** - Billing invoices
12. **InvoiceItems** - Invoice line items
13. **Payments** - Payment tracking
14. **AuditLogs** - System audit trail

### Key Design Decisions:

- **Soft Deletes**: All entities implement `IsDeleted` flag
- **Audit Fields**: `CreatedAt`, `UpdatedAt`, `CreatedBy`, `UpdatedBy`
- **Branch Isolation**: Foreign key to `BranchId` for multi-tenancy
- **Indexes**: On frequently queried fields (Status, Dates, CustomerId, etc.)
- **Enums**: Stored as int in database, mapped in application

---

## 🔐 AUTHENTICATION & AUTHORIZATION

### Technology Stack:
- **ASP.NET Core Identity** for user management
- **JWT Bearer Tokens** for API authentication
- **Role-Based Access Control (RBAC)**
- **Policy-Based Authorization** for fine-grained control

### Roles:
1. **SuperAdmin** - Full system access, all branches
2. **BranchManager** - Branch-level management
3. **Reception** - Customer & appointment management
4. **Mechanic** - Service order execution
5. **Accountant** - Billing & financial access
6. **InventoryManager** - Inventory & supplier management

### Security Features:
- Password hashing (Identity default)
- JWT token expiration & refresh
- Branch-based data isolation
- Audit logging for sensitive operations

---

## 🎨 UI ARCHITECTURE (Blazor Server)

### Component Structure:

```
BlazorUI/
├── Layout/
│   ├── MainLayout.razor          # Main app layout
│   ├── NavMenu.razor             # Sidebar navigation
│   └── TopBar.razor              # Header with user info
├── Pages/
│   ├── Dashboard.razor           # Executive dashboard
│   ├── Customers/                # Customer management
│   ├── Vehicles/                 # Vehicle management
│   ├── ServiceOrders/            # Service order workflow
│   ├── Appointments/             # Appointment calendar
│   ├── Inventory/                # Inventory management
│   ├── Billing/                  # Invoicing & payments
│   └── Reports/                  # Report generation
└── Components/
    ├── DataTable.razor           # Reusable data table
    ├── Modal.razor               # Modal dialog
    └── Charts/                   # Chart components
```

### UI Framework:
- **Blazor Server** with SignalR
- **Bootstrap 5** for styling
- **Chart.js** for charts (via Blazor wrapper)
- **Dark Theme** with Renault yellow accents (#FFD700 / #FFB700)

---

## 📊 BUSINESS LOGIC FLOW

### Service Order Workflow:

```
Created → In Progress → Waiting for Parts → Quality Check → Completed → Delivered
```

### Invoice Generation:
- Auto-generated when Service Order status = "Completed"
- Includes: Labor costs + Parts used
- VAT calculation support
- Discount application
- PDF export

### Inventory Management:
- Stock In: Purchase from suppliers
- Stock Out: Used in service orders
- Reorder alerts when stock < minimum threshold
- Branch-specific inventory

---

## 🔄 DATA FLOW EXAMPLE

### Creating a Service Order:

1. **Blazor UI** → User fills form
2. **API Controller** → Receives request
3. **Application Service** → Validates, applies business rules
4. **Repository** → Persists to database via EF Core
5. **Database** → Stores data
6. **Response** → Returns to UI via SignalR

---

## 📦 TECHNOLOGY STACK SUMMARY

| Layer | Technology |
|-------|-----------|
| **UI** | Blazor Server, Bootstrap 5, Chart.js |
| **API** | ASP.NET Core Web API (.NET 8) |
| **ORM** | Entity Framework Core (Code First) |
| **Database** | SQL Server |
| **Authentication** | ASP.NET Core Identity + JWT |
| **Architecture** | Clean Architecture + DDD |
| **Patterns** | Repository Pattern, Unit of Work |
| **PDF** | QuestPDF or iTextSharp |
| **Validation** | FluentValidation |

---

## 🚀 DEPLOYMENT READINESS

### Azure Compatibility:
- ✅ Connection strings via Configuration
- ✅ Azure SQL Database ready
- ✅ Azure App Service compatible
- ✅ Blob Storage for file uploads (structured)
- ✅ Application Insights ready

### Production Features:
- Logging (Serilog or built-in)
- Exception handling middleware
- Health checks
- CORS configuration
- Environment-based configuration

---

## 📋 IMPLEMENTATION PHASES

### ✅ Phase 1: Architecture (CURRENT)
- Architecture documentation
- Project structure setup
- Solution file creation

### ⏳ Phase 2: Database Schema
- Domain entities
- DbContext configuration
- Initial migration
- Seed data

### ⏳ Phase 3: Backend APIs
- Application layer (services, DTOs)
- Infrastructure (repositories, EF)
- Web API controllers
- Validation & error handling

### ⏳ Phase 4: Authentication
- Identity setup
- JWT configuration
- Role seeding
- User management

### ⏳ Phase 5: Blazor UI
- Layout & navigation
- Dashboard with charts
- All feature modules
- Forms & data tables

### ⏳ Phase 6: Reports & Invoices
- PDF generation service
- Invoice templates
- Report generation
- Print functionality

---

## ✅ READY FOR PHASE 2?

This architecture follows enterprise best practices:
- ✅ Clean Architecture separation
- ✅ DDD principles
- ✅ Repository pattern
- ✅ Multi-tenant (branch isolation)
- ✅ Scalable & maintainable
- ✅ Production-ready structure

**Next Step**: Create Domain Entities, DbContext, and Database Migrations.

---

*Generated for Renault Smart Center ERP System*
