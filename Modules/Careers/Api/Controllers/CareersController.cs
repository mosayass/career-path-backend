using CareerPath.Careers.Core.Features.Queries.GetCareerById;
using CareerPath.Careers.Core.Features.Queries.SearchCareers;
using CareerPath.Shared.Api.Controllers; 
using CareerPath.Shared.Contracts.Careers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace CareerPath.Careers.Api.Controllers;

public class CareersController(ISender sender) : ApiControllerBase(sender)
{
    [HttpGet("by-ai-label/{aiLabelId:int}")]
    public async Task<IActionResult> GetCareerById(int aiLabelId)
    {
        var result = await Sender.Send(new GetCareerMatchDetailsQuery(aiLabelId));
        return HandleResult(result);
    }
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetCareerById(Guid id)
    {
        var result = await Sender.Send(new GetCareerSummaryByIdQuery(id));
        return HandleResult(result);
    }

    [HttpGet]
    public async Task<IActionResult> SearchCareers(
        [FromQuery] string? searchTerm,
        [FromQuery] int? sectorId,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20)
    {
        var result = await Sender.Send(new SearchCareersQuery(searchTerm, sectorId, page, pageSize));
        return HandleResult(result);
    }
}