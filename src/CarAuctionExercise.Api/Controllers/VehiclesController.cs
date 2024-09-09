using CarAuctionExercise.Application.DTOs.Vehicles;
using CarAuctionExercise.Application.Interfaces;
using CarAuctionExercise.Domain;
using Microsoft.AspNetCore.Mvc;

namespace CarAuctionExercise.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class VehiclesController : ControllerBase
{
    private readonly IVehiclesManagementService _vehiclesManagementService;

    public VehiclesController(IVehiclesManagementService vehiclesManagementService)
    {
        _vehiclesManagementService = vehiclesManagementService;
    }

    [HttpPost]
    public IActionResult Add([FromBody] AddVehicle request)
    { 
        var result = _vehiclesManagementService.Add(request);

        if (result.IsSuccess)
        {
            return Created("", result.Value); 
        }

        return BadRequest(new { Message = result.Errors.Select(e => e.Message).ToList()});
    }
    
    [HttpGet("Search")]
    public IActionResult Search(
        [FromQuery] VehicleType? vehicleType, 
        [FromQuery] string? manufacturer, 
        [FromQuery] string? model, 
        [FromQuery] int? year)
    {
        var result = _vehiclesManagementService.Search(
            vehicleType, 
            manufacturer, 
            model, 
            year);

        return Ok(result);
    }
}