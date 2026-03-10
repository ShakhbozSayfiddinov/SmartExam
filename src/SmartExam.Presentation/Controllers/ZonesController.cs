using Microsoft.AspNetCore.Mvc;
using SmartExam.Application.DTOs.Zones;
using SmartExam.Application.Interfaces;

namespace SmartExam.Presentation.Controllers;

public class ZonesController(IZoneService zoneService) : AppController
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var zones = await zoneService.GetAllAsync();
        return Success(zones);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var zone = await zoneService.GetByIdAsync(id);
        return zone is null ? NotFound() : Success(zone);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateZoneDto dto)
    {
        var zone = await zoneService.CreateAsync(dto);
        return Created(nameof(GetById), new { id = zone.Id }, zone);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateZoneDto dto)
    {
        var zone = await zoneService.UpdateAsync(id, dto);
        return Success(zone);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await zoneService.DeleteAsync(id);
        return NoContent();
    }
}
