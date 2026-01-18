using Microsoft.EntityFrameworkCore;
using RenaultSmartCenter.Application.Interfaces;
using RenaultSmartCenter.Domain.Entities;
using RenaultSmartCenter.Domain.Enums;
using RenaultSmartCenter.Infrastructure.Data;

namespace RenaultSmartCenter.Application.Features.Dashboard;

public interface IDashboardService
{
    Task<DashboardDto> GetDashboardDataAsync(Guid branchId);
}

public class DashboardDto
{
    public int CarsInService { get; set; }
    public int TodaysAppointments { get; set; }
    public int CompletedJobsToday { get; set; }
    public decimal DailyRevenue { get; set; }
    public decimal MonthlyRevenue { get; set; }
    public int DelayedVehicles { get; set; }
    public int LowStockParts { get; set; }
    public List<RevenueTrendDto> RevenueTrend { get; set; } = new();
    public List<ServiceTypeDistributionDto> ServiceTypeDistribution { get; set; } = new();
    public List<TopModelDto> TopModels { get; set; } = new();
}

public class RevenueTrendDto
{
    public DateTime Date { get; set; }
    public decimal Revenue { get; set; }
}

public class ServiceTypeDistributionDto
{
    public string ServiceType { get; set; } = string.Empty;
    public int Count { get; set; }
}

public class TopModelDto
{
    public string Model { get; set; } = string.Empty;
    public int ServiceCount { get; set; }
}

public class DashboardService : IDashboardService
{
    private readonly ApplicationDbContext _context;

    public DashboardService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<DashboardDto> GetDashboardDataAsync(Guid branchId)
    {
        var today = DateTime.UtcNow.Date;
        var startOfMonth = new DateTime(today.Year, today.Month, 1);

        var carsInService = await _context.ServiceOrders
            .CountAsync(so => so.BranchId == branchId &&
                            !so.IsDeleted &&
                            so.Status != ServiceOrderStatus.Completed &&
                            so.Status != ServiceOrderStatus.Delivered &&
                            so.Status != ServiceOrderStatus.Cancelled);

        var todaysAppointments = await _context.Appointments
            .CountAsync(a => a.BranchId == branchId &&
                           !a.IsDeleted &&
                           a.AppointmentDate.Date == today);

        var completedJobsToday = await _context.ServiceOrders
            .CountAsync(so => so.BranchId == branchId &&
                            !so.IsDeleted &&
                            so.CompletedDate.HasValue &&
                            so.CompletedDate.Value.Date == today);

        var dailyRevenue = await _context.ServiceOrders
            .Where(so => so.BranchId == branchId &&
                        !so.IsDeleted &&
                        so.CompletedDate.HasValue &&
                        so.CompletedDate.Value.Date == today)
            .SumAsync(so => so.TotalAmount);

        var monthlyRevenue = await _context.ServiceOrders
            .Where(so => so.BranchId == branchId &&
                        !so.IsDeleted &&
                        so.CompletedDate.HasValue &&
                        so.CompletedDate.Value >= startOfMonth)
            .SumAsync(so => so.TotalAmount);

        var delayedVehicles = await _context.ServiceOrders
            .CountAsync(so => so.BranchId == branchId &&
                            !so.IsDeleted &&
                            so.ScheduledDate.HasValue &&
                            so.ScheduledDate.Value < today &&
                            so.Status != ServiceOrderStatus.Completed &&
                            so.Status != ServiceOrderStatus.Delivered);

        var lowStockParts = await _context.SpareParts
            .CountAsync(sp => sp.BranchId == branchId &&
                            !sp.IsDeleted &&
                            sp.IsActive &&
                            sp.StockQuantity <= sp.MinimumStock);

        // Revenue Trend (Last 7 days)
        var revenueTrend = await _context.ServiceOrders
            .Where(so => so.BranchId == branchId &&
                        !so.IsDeleted &&
                        so.CompletedDate.HasValue &&
                        so.CompletedDate.Value >= today.AddDays(-7))
            .GroupBy(so => so.CompletedDate!.Value.Date)
            .Select(g => new RevenueTrendDto
            {
                Date = g.Key,
                Revenue = g.Sum(so => so.TotalAmount)
            })
            .OrderBy(r => r.Date)
            .ToListAsync();

        // Service Type Distribution
        var serviceTypeDistribution = await _context.ServiceOrders
            .Where(so => so.BranchId == branchId &&
                        !so.IsDeleted &&
                        so.CompletedDate.HasValue &&
                        so.CompletedDate.Value >= startOfMonth)
            .GroupBy(so => so.ServiceType)
            .Select(g => new ServiceTypeDistributionDto
            {
                ServiceType = g.Key.ToString(),
                Count = g.Count()
            })
            .ToListAsync();

        // Top Models
        var topModels = await _context.ServiceOrders
            .Include(so => so.Vehicle)
            .Where(so => so.BranchId == branchId &&
                        !so.IsDeleted &&
                        so.CompletedDate.HasValue &&
                        so.CompletedDate.Value >= startOfMonth)
            .GroupBy(so => so.Vehicle.Model)
            .Select(g => new TopModelDto
            {
                Model = g.Key,
                ServiceCount = g.Count()
            })
            .OrderByDescending(tm => tm.ServiceCount)
            .Take(5)
            .ToListAsync();

        return new DashboardDto
        {
            CarsInService = carsInService,
            TodaysAppointments = todaysAppointments,
            CompletedJobsToday = completedJobsToday,
            DailyRevenue = dailyRevenue,
            MonthlyRevenue = monthlyRevenue,
            DelayedVehicles = delayedVehicles,
            LowStockParts = lowStockParts,
            RevenueTrend = revenueTrend,
            ServiceTypeDistribution = serviceTypeDistribution,
            TopModels = topModels
        };
    }
}
