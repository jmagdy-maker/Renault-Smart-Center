# Renault Smart Center - Enterprise ERP System

A full-featured Enterprise Resource Planning (ERP) system built for Renault Authorized Service Centers using .NET 8, Blazor Server, and SQL Server.

## 🚀 Technology Stack

- **Backend**: ASP.NET Core Web API (.NET 8)
- **Database**: SQL Server with Entity Framework Core (Code First)
- **UI**: Blazor Server
- **Authentication**: ASP.NET Core Identity + JWT
- **Architecture**: Clean Architecture + DDD + Repository Pattern

## 📋 Features

### Core Modules

1. **Authentication & Security**
   - JWT-based authentication
   - Role-based access control (RBAC)
   - Multi-branch data isolation
   - Audit logging

2. **Dashboard**
   - Real-time statistics
   - Revenue trends
   - Service type distribution
   - Top serviced models

3. **Customer & Vehicle Management**
   - Complete customer profiles
   - Multiple vehicles per customer
   - Full service history tracking

4. **Service Orders (Work Orders)**
   - Complete workflow management
   - Status tracking (Created → In Progress → Completed → Delivered)
   - Mechanic assignment
   - Parts and labor tracking

5. **Appointments System**
   - Calendar-based scheduling
   - Daily/weekly views
   - Mechanic availability tracking

6. **Inventory Management**
   - Spare parts catalog
   - Stock tracking (In/Out)
   - Low stock alerts
   - Supplier management

7. **Billing & Invoicing**
   - Auto-generated invoices
   - VAT support
   - Payment tracking
   - PDF invoice generation (structured for future implementation)

8. **Reporting**
   - Daily operations reports
   - Monthly revenue reports
   - Mechanic performance metrics
   - Inventory valuation

9. **Multi-Branch Support**
   - Branch-specific data isolation
   - Consolidated reporting (SuperAdmin)

## 🏗️ Project Structure

```
RenaultSmartCenter/
├── src/
│   ├── RenaultSmartCenter.Domain/          # Domain Layer (Entities, Enums)
│   ├── RenaultSmartCenter.Application/     # Application Layer (Services, DTOs)
│   ├── RenaultSmartCenter.Infrastructure/  # Infrastructure (EF Core, Repositories)
│   ├── RenaultSmartCenter.API/             # Web API Controllers
│   └── RenaultSmartCenter.BlazorUI/        # Blazor Server UI
├── RenaultSmartCenter.sln
└── README.md
```

## 🚀 Getting Started

### Prerequisites

- .NET 8 SDK
- SQL Server (LocalDB or SQL Server Express)
- Visual Studio 2022 or VS Code

### Installation Steps

1. **Clone the repository**
   ```bash
   git clone <repository-url>
   cd "Renault Smart Center"
   ```

2. **Restore packages**
   ```bash
   dotnet restore
   ```

3. **Update connection string**
   - Open `src/RenaultSmartCenter.API/appsettings.json`
   - Update `ConnectionStrings:DefaultConnection` with your SQL Server connection string

4. **Run database migrations**
   ```bash
   cd src/RenaultSmartCenter.Infrastructure
   dotnet ef migrations add InitialCreate --startup-project ../RenaultSmartCenter.API --context ApplicationDbContext
   dotnet ef database update --startup-project ../RenaultSmartCenter.API --context ApplicationDbContext
   ```

   Or let the application auto-apply migrations on startup (configured in `Program.cs`).

5. **Run the application**
   ```bash
   # Terminal 1: API
   cd src/RenaultSmartCenter.API
   dotnet run

   # Terminal 2: Blazor UI
   cd src/RenaultSmartCenter.BlazorUI
   dotnet run
   ```

6. **Access the application**
   - API Swagger UI: `https://localhost:7001/swagger`
   - Blazor UI: `https://localhost:5001` or `http://localhost:5000`

## 🔐 Default Login Credentials

The system seeds the following test users on first run:

| Email | Password | Role |
|-------|----------|------|
| admin@renault.com | Admin@123 | SuperAdmin |
| manager@renault.com | Manager@123 | BranchManager |
| mechanic@renault.com | Mechanic@123 | Mechanic |
| reception@renault.com | Reception@123 | Reception |

## 📊 User Roles

- **SuperAdmin**: Full system access across all branches
- **BranchManager**: Branch-level management and reporting
- **Reception**: Customer and appointment management
- **Mechanic**: Service order execution and updates
- **Accountant**: Billing and financial access
- **InventoryManager**: Inventory and supplier management

## 🎨 UI Theme

- **Dark Theme**: Enterprise dark theme (#1a1a1a, #2d2d2d)
- **Accent Color**: Renault Yellow (#FFD700, #FFB700)
- **Responsive Design**: Mobile-friendly layout

## 📦 Database

### Initial Seed Data

- **2 Branches**: Main Branch (Downtown), Branch - North
- **6 Roles**: All role types
- **4 Test Users**: One for each primary role

### Database Features

- Soft deletes (IsDeleted flag)
- Audit fields (CreatedAt, UpdatedAt, CreatedBy, UpdatedBy)
- Branch-based multi-tenancy
- Optimized indexes on frequently queried fields

## 🔧 Development

### Building the Solution

```bash
dotnet build RenaultSmartCenter.sln
```

### Running Tests

```bash
# (Test projects will be added in future updates)
dotnet test
```

## 📝 API Documentation

Swagger UI is available at `/swagger` when running the API in Development mode.

### Key Endpoints

- `POST /api/auth/login` - User authentication
- `GET /api/dashboard/{branchId}` - Dashboard data
- `GET /api/customers/branch/{branchId}` - Get customers
- `GET /api/serviceorders/branch/{branchId}` - Get service orders
- `GET /api/appointments/branch/{branchId}` - Get appointments
- `GET /api/inventory/branch/{branchId}` - Get spare parts

## 🚧 Future Enhancements

- PDF invoice generation (QuestPDF integration)
- Real-time notifications (SignalR)
- SMS/WhatsApp integration
- Advanced reporting with charts
- Mobile app support
- Cloud deployment (Azure)

## 📄 License

This project is proprietary software for Renault Authorized Service Centers.

## 👥 Support

For technical support, contact the development team.

---

**Built with ❤️ for Renault Smart Center**
