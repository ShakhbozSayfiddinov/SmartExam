using Microsoft.AspNetCore.Mvc;
using SmartExam.Application.DTOs.Admin;
using SmartExam.Application.Interfaces;

namespace SmartExam.Presentation.Controllers;

public class AdminController(IAdminService adminService) : AppController
{
    [HttpGet("users")]
    public async Task<IActionResult> GetUsers([FromQuery] GetUsersQuery query)
    {
        var result = await adminService.GetAllUsersAsync(query);

        return Success(new
        {
            users      = result.Items,
            total      = result.Total,
            page       = result.Page,
            limit      = result.Limit,
            totalPages = result.TotalPages,
        });
    }
}
