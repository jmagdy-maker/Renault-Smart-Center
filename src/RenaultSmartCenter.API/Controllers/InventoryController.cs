using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RenaultSmartCenter.Application.Common.DTOs;
using RenaultSmartCenter.Application.Features.Inventory;

namespace RenaultSmartCenter.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class InventoryController : ControllerBase
{
    private readonly ISparePartService _sparePartService;

    public InventoryController(ISparePartService sparePartService)
    {
        _sparePartService = sparePartService;
    }

    [HttpGet("branch/{branchId}")]
    public async Task<ActionResult<IEnumerable<SparePartDto>>> GetSpareParts(Guid branchId, [FromQuery] bool? lowStockOnly)
    {
        var parts = await _sparePartService.GetAllAsync(branchId, lowStockOnly);
        return Ok(parts);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<SparePartDto>> GetSparePart(Guid id)
    {
        var part = await _sparePartService.GetByIdAsync(id);
        if (part == null)
            return NotFound();

        return Ok(part);
    }

    [HttpPost]
    public async Task<ActionResult<SparePartDto>> CreateSparePart(CreateSparePartDto dto)
    {
        var userId = User.FindFirstValue(System.Security.Claims.ClaimTypes.NameIdentifier) ?? "System";
        var part = await _sparePartService.CreateAsync(dto, userId);
        return CreatedAtAction(nameof(GetSparePart), new { id = part.Id }, part);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<SparePartDto>> UpdateSparePart(Guid id, UpdateSparePartDto dto)
    {
        var userId = User.FindFirstValue(System.Security.Claims.ClaimTypes.NameIdentifier) ?? "System";
        var part = await _sparePartService.UpdateAsync(id, dto, userId);
        if (part == null)
            return NotFound();

        return Ok(part);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSparePart(Guid id)
    {
        var result = await _sparePartService.DeleteAsync(id);
        if (!result)
            return NotFound();

        return NoContent();
    }
}
