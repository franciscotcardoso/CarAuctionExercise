using CarAuctionExercise.Application.DTOs.Auctions;
using CarAuctionExercise.Application.DTOs.Bids;
using CarAuctionExercise.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CarAuctionExercise.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class AuctionsController : ControllerBase
{
    private readonly IAuctionsManagementService _auctionsManagementService;

    public AuctionsController(IAuctionsManagementService auctionsManagementService)
    {
        _auctionsManagementService = auctionsManagementService;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(object))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(object))]
    public IActionResult Add([FromBody] AddAuction request)
    {
        var result = _auctionsManagementService.Add(request);

        if (result.IsSuccess)
        {
            return Created("", result.Value);
        }

        return BadRequest(new { Message = result.Errors.Select(e => e.Message).ToList()});
    }
    
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(object))]
    public IActionResult Get()
    {
        var result = _auctionsManagementService.GetAllAuctions();

        return Ok(result);
    }
    
    [HttpPost("{auctionId}/start")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(object))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(object))]
    public IActionResult Start(string auctionId)
    {
        var result = _auctionsManagementService.Start(auctionId);

        if (result.IsSuccess)
        {
            return Ok(new { Message = "Auction started successfully."});
        }

        return BadRequest(new { Message = result.Errors.Select(e => e.Message).ToList()});
    }
    
    [HttpPost("{auctionId}/close")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(object))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(object))]
    public IActionResult Close(string auctionId)
    {
        var result = _auctionsManagementService.Close(auctionId);

        if (result.IsSuccess)
        {
            return Ok(new { Message = "Auction closed successfully."});
        }

        return BadRequest(new { Message = result.Errors.Select(e => e.Message).ToList()});
    }
    
    [HttpPost("bid")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(object))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(object))]
    public IActionResult Bid([FromBody] AddBid request)
    {
        var result = _auctionsManagementService.Bid(request);

        if (result.IsSuccess)
        {
            return Ok(new { Message = "Bid placed successfully"});
        }

        return BadRequest(new { Message = result.Errors.Select(e => e.Message).ToList()});
    }
}