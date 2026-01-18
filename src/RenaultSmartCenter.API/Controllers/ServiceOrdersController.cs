using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RenaultSmartCenter.Application.Common.DTOs;
using RenaultSmartCenter.Application.Features.ServiceOrders;
using RenaultSmartCenter.Domain.Enums;
using System.Security.Claims;

namespace RenaultSmartCenter.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ServiceOrdersController : ControllerBase
{
    private readonly IServiceOrderService _serviceOrderService;

    public ServiceOrdersController(IServiceOrderService serviceOrderService)
    {
        _serviceOrderService = serviceOrderService;
    }

    [HttpGet("branch/{branchId}")]
    public async Task<ActionResult<IEnumerable<ServiceOrderDto>>> GetServiceOrders(Guid branchId)
    {
        var orders = await _serviceOrderService.GetAllAsync(branchId);
        return Ok(orders);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ServiceOrderDto>> GetServiceOrder(Guid id)
    {
        var order = await _serviceOrderService.GetByIdAsync(id);
        if (order == null)
            return NotFound();

        return Ok(order);
    }

    [HttpPost]
    public async Task<ActionResult<ServiceOrderDto>> CreateServiceOrder(CreateServiceOrderDto dto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "System";
        var order = await _serviceOrderService.CreateAsync(dto, userId);
        return CreatedAtAction(nameof(GetServiceOrder), new { id = order.Id }, order);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ServiceOrderDto>> UpdateServiceOrder(Guid id, UpdateServiceOrderDto dto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "System";
        var order = await _serviceOrderService.UpdateAsync(id, dto, userId);
        if (order == null)
            return NotFound();

        return Ok(order);
    }

    [HttpPatch("{id}/status")]
    public async Task<IActionResult> UpdateStatus(Guid id, [FromBody] ServiceOrderStatus status)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "System";
        var result = await _serviceOrderService.UpdateStatusAsync(id, status, userId);
        if (!result)
            return NotFound();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteServiceOrder(Guid id)
    {
        var result = await _serviceOrderService.DeleteAsync(id);
        if (!result)
            return NotFound();

        return NoContent();
    }
}
