# Database Setup & Migration Guide

## 📊 Phase 2: Database Schema Complete

### ✅ What's Been Created:

1. **Domain Entities** (16 entities)
   - Branch, User, Customer, Vehicle
   - ServiceOrder, ServiceOrderItem
   - Appointment
   - SparePart, InventoryTransaction, Supplier
   - Invoice, InvoiceItem, Payment
   - AuditLog

2. **Enums** (6 enums)
   - ServiceType
   - ServiceOrderStatus
   - AppointmentStatus
   - PaymentMethod
   - TransactionType
   - UserRole

3. **EF Core Configuration**
   - ApplicationDbContext
   - 14 Entity Configurations
   - Global query filters for soft deletes
   - Automatic audit field population

4. **Database Features**
   - Primary & Foreign Keys
   - Indexes on frequently queried fields
   - Unique constraints (OrderNumber, InvoiceNumber, etc.)
   - Soft delete support (IsDeleted flag)
   - Audit fields (CreatedAt, UpdatedAt, CreatedBy, UpdatedBy)

---

## 🗄️ Database Schema Overview

### Core Tables:

```
Branches
├── Id (PK, Guid)
├── Name
├── Address, City, Phone, Email
├── IsActive
└── Audit Fields

Customers
├── Id (PK, Guid)
├── BranchId (FK)
├── FirstName, LastName
├── Email, Phone, Address
└── Audit Fields

Vehicles
├── Id (PK, Guid)
├── CustomerId (FK)
├── Make, Model, Year
├── VIN, PlateNumber
├── CurrentMileage
└── Audit Fields

ServiceOrders
├── Id (PK, Guid)
├── BranchId, CustomerId, VehicleId (FKs)
├── OrderNumber (Unique)
├── ServiceType, Status (Enums)
├── LaborCost, PartsCost, Discount, VAT, TotalAmount
└── Audit Fields

ServiceOrderItems
├── Id (PK, Guid)
├── ServiceOrderId, SparePartId (FKs)
├── Description, Quantity
├── UnitPrice, TotalPrice
└── Audit Fields

Appointments
├── Id (PK, Guid)
├── BranchId, CustomerId, VehicleId, AssignedMechanicId (FKs)
├── AppointmentDate, StartTime, EndTime
├── Status (Enum)
└── Audit Fields

SpareParts
├── Id (PK, Guid)
├── BranchId, SupplierId (FKs)
├── PartNumber, Name, OEMNumber
├── UnitPrice, StockQuantity, MinimumStock
└── Audit Fields

InventoryTransactions
├── Id (PK, Guid)
├── SparePartId, BranchId, SupplierId, ServiceOrderId (FKs)
├── TransactionType (Enum)
├── Quantity, UnitPrice, TotalAmount
└── Audit Fields

Suppliers
├── Id (PK, Guid)
├── BranchId (FK)
├── Name, ContactPerson
├── Email, Phone, Address
└── Audit Fields

Invoices
├── Id (PK, Guid)
├── BranchId, ServiceOrderId (FKs)
├── InvoiceNumber (Unique)
├── SubTotal, Discount, VAT, TotalAmount
├── IsPaid, PaidDate
└── Audit Fields

InvoiceItems
├── Id (PK, Guid)
├── InvoiceId (FK)
├── Description, Quantity
├── UnitPrice, TotalPrice
└── Audit Fields

Payments
├── Id (PK, Guid)
├── InvoiceId (FK)
├── Amount, PaymentMethod (Enum)
├── PaymentDate
└── Audit Fields

Users
├── Id (PK, Guid)
├── IdentityUserId (Links to ASP.NET Identity)
├── BranchId (FK)
├── FirstName, LastName
└── Audit Fields

AuditLogs
├── Id (PK, Guid)
├── UserId, UserName
├── Action, EntityType, EntityId
├── OldValues, NewValues (JSON)
└── Timestamp
```

---

## 🚀 Creating Database Migrations

### Prerequisites:
- SQL Server installed and running
- .NET 8 SDK installed
- Connection string configured

### Step 1: Add Infrastructure Project Reference to API/BlazorUI
*(Will be done in Phase 3)*

### Step 2: Configure Connection String
Add to `appsettings.json` (in API project):
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=RenaultSmartCenter;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=true"
  }
}
```

### Step 3: Install EF Core Tools (if not already installed)
```bash
dotnet tool install --global dotnet-ef
```

### Step 4: Create Initial Migration
```bash
cd src/RenaultSmartCenter.Infrastructure
dotnet ef migrations add InitialCreate --startup-project ../RenaultSmartCenter.API --context ApplicationDbContext
```

### Step 5: Update Database
```bash
dotnet ef database update --startup-project ../RenaultSmartCenter.API --context ApplicationDbContext
```

Or apply migration programmatically (recommended for production):
```csharp
// In Program.cs (Phase 3)
context.Database.Migrate();
await DatabaseSeeder.SeedAsync(context, userManager, roleManager);
```

---

## 📋 Seed Data Included

The `DatabaseSeeder` class will create:

### Roles:
- SuperAdmin
- BranchManager
- Reception
- Mechanic
- Accountant
- InventoryManager

### Branches:
- Main Branch - Downtown (Beirut)
- Branch - North (Tripoli)

### Test Users:
| Username | Password | Role | Branch |
|----------|----------|------|--------|
| admin@renault.com | Admin@123 | SuperAdmin | Main Branch |
| manager@renault.com | Manager@123 | BranchManager | Main Branch |
| mechanic@renault.com | Mechanic@123 | Mechanic | Main Branch |
| reception@renault.com | Reception@123 | Reception | Main Branch |

---

## 🔍 Key Database Features

### 1. Soft Deletes
All entities implement `IAuditable` with `IsDeleted` flag. Global query filters automatically exclude deleted records.

### 2. Audit Trail
- `CreatedAt`, `UpdatedAt` - Automatically set
- `CreatedBy`, `UpdatedBy` - Set from current user context
- `AuditLog` table for detailed audit trail

### 3. Branch Isolation
All entities have `BranchId` for multi-tenant data isolation. Users can only access their branch's data (enforced in application layer).

### 4. Indexes
Optimized indexes on:
- Foreign keys
- Status fields
- Dates (for reporting queries)
- Unique constraints (OrderNumber, InvoiceNumber)

### 5. Relationships
- CASCADE delete for child entities (InvoiceItems, ServiceOrderItems)
- RESTRICT delete for parent entities (Customers, Vehicles)
- SET NULL for optional relationships (AssignedMechanic)

---

## 📝 Next Steps (Phase 3)

In Phase 3, we will:
1. Set up Application layer (DTOs, Services, Interfaces)
2. Set up Infrastructure repositories
3. Create Web API controllers
4. Configure dependency injection
5. Set up database connection and migrations

---

*Database schema is production-ready and follows best practices for enterprise applications.*
