using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RenaultSmartCenter.Application.Common.DTOs;
using RenaultSmartCenter.Application.Features.Vehicles;
using System.Security.Claims;

namespace RenaultSmartCenter.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class VehiclesController : ControllerBase
{
    private readonly IVehicleService _vehicleService;

    public VehiclesController(IVehicleService vehicleService)
    {
        _vehicleService = vehicleService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<VehicleDto>>> GetVehicles([FromQuery] Guid? customerId)
    {
        var vehicles = await _vehicleService.GetAllAsync(customerId);
        return Ok(vehicles);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<VehicleDto>> GetVehicle(Guid id)
    {
        var vehicle = await _vehicleService.GetByIdAsync(id);
        if (vehicle == null)
            return NotFound();

        return Ok(vehicle);
    }

    [HttpPost]
    public async Task<ActionResult<VehicleDto>> CreateVehicle(CreateVehicleDto dto)
    {
        var userId = User.FindFirstValue(System.Security.Claims.ClaimTypes.NameIdentifier) ?? "System";
        var vehicle = await _vehicleService.CreateAsync(dto, userId);
        return CreatedAtAction(nameof(GetVehicle), new { id = vehicle.Id }, vehicle);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<VehicleDto>> UpdateVehicle(Guid id, UpdateVehicleDto dto)
    {
        var userId = User.FindFirstValue(System.Security.Claims.ClaimTypes.NameIdentifier) ?? "System";
        var vehicle = await _vehicleService.UpdateAsync(id, dto, userId);
        if (vehicle == null)
            return NotFound();

        return Ok(vehicle);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteVehicle(Guid id)
    {
        var result = await _vehicleService.DeleteAsync(id);
        if (!result)
            return NotFound();

        return NoContent();
    }
}
