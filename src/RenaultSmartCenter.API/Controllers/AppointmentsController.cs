using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RenaultSmartCenter.Application.Common.DTOs;
using RenaultSmartCenter.Application.Features.Appointments;
using System.Security.Claims;

namespace RenaultSmartCenter.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AppointmentsController : ControllerBase
{
    private readonly IAppointmentService _appointmentService;

    public AppointmentsController(IAppointmentService appointmentService)
    {
        _appointmentService = appointmentService;
    }

    [HttpGet("branch/{branchId}")]
    public async Task<ActionResult<IEnumerable<AppointmentDto>>> GetAppointments(Guid branchId, [FromQuery] DateTime? date)
    {
        var appointments = await _appointmentService.GetAllAsync(branchId, date);
        return Ok(appointments);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<AppointmentDto>> GetAppointment(Guid id)
    {
        var appointment = await _appointmentService.GetByIdAsync(id);
        if (appointment == null)
            return NotFound();

        return Ok(appointment);
    }

    [HttpPost]
    public async Task<ActionResult<AppointmentDto>> CreateAppointment(CreateAppointmentDto dto)
    {
        var userId = User.FindFirstValue(System.Security.Claims.ClaimTypes.NameIdentifier) ?? "System";
        var appointment = await _appointmentService.CreateAsync(dto, userId);
        return CreatedAtAction(nameof(GetAppointment), new { id = appointment.Id }, appointment);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<AppointmentDto>> UpdateAppointment(Guid id, UpdateAppointmentDto dto)
    {
        var userId = User.FindFirstValue(System.Security.Claims.ClaimTypes.NameIdentifier) ?? "System";
        var appointment = await _appointmentService.UpdateAsync(id, dto, userId);
        if (appointment == null)
            return NotFound();

        return Ok(appointment);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAppointment(Guid id)
    {
        var result = await _appointmentService.DeleteAsync(id);
        if (!result)
            return NotFound();

        return NoContent();
    }
}
