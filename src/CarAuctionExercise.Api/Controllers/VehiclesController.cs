namespace CarAuctionExercise.Api.Controllers;

using CarAuctionExercise.Application.DTOs.Vehicles;
using CarAuctionExercise.Application.Interfaces;
using CarAuctionExercise.Domain;
using Microsoft.AspNetCore.Mvc;

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
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(object))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(object))]
    public IActionResult Add([FromBody] AddVehicle request)
    {
        var result = _vehiclesManagementService.Add(request);

        if (result.IsSuccess)
        {
            return Created(string.Empty, result.Value);
        }

        return BadRequest(new { Message = result.Errors.Select(e => e.Message).ToList()});
    }

    [HttpGet("Search")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(object))]
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