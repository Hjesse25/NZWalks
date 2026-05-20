using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.DTOs;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RegionsController : ControllerBase
{
    private readonly NZWalksDbContext dbContext;

    public RegionsController(NZWalksDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    // GET all regions
    // GET http://localhost:portnumber/api/regions
    [HttpGet]
    public IActionResult GetAll()
    {
        // Get Data from Database - Domain models
        var regionsDomain = dbContext.Regions.ToList();

        // Map Domain Models to DTOs
        var regionDto = new List<RegionDto>();
        foreach (var regionDomain in regionsDomain)
        {
            regionDto.Add(new RegionDto()
            {
                Id = regionDomain.Id,
                Code = regionDomain.Code,
                Name = regionDomain.Name,
                RegionImageUrl = regionDomain.RegionImageUrl
            });
        }

        // Return DTOs
        return Ok(regionDto);
    }

    // GET single region (Get region by ID)
    // GET http://localhost:portnumber/api/regions/{id}
    [HttpGet]
    [Route("{id:Guid}")]
    public IActionResult GetById([FromRoute] Guid id)
    {
        // var region = dbContext.Regions.Find(id);
        // Get region domain model from database
        var regionDomain = dbContext.Regions.FirstOrDefault(x => x.Id == id);

        if (regionDomain == null)
        {
            return NotFound();
        }

        // Map/Convert region domain model to region DTO
        var regionDto = new RegionDto
        {
            Id = regionDomain.Id,
            Code = regionDomain.Code,
            Name = regionDomain.Name,
            RegionImageUrl = regionDomain.RegionImageUrl
        };

        // return DTO back to client
        return Ok(regionDto);
    }

    // POST To create new region
    // POST: http://localhost:portnumber/api/regions
    [HttpPost]
    public IActionResult Create([FromBody] AddRegionRequestDto request)
    {
        // Map or Convert DTO to Domain Model
        var regionDomainModel = new Region
        {
            Code = request.Code,
            Name = request.Name,
            RegionImageUrl = request.RegionImageUrl
        };

        // Use Domain Model to create Region
        dbContext.Regions.Add(regionDomainModel);
        dbContext.SaveChanges();

        // Map Domain model back to DTO
        var regionDto = new RegionDto
        {
            Id = regionDomainModel.Id,
            Code = regionDomainModel.Code,
            Name = regionDomainModel.Name,
            RegionImageUrl = regionDomainModel.RegionImageUrl
        };

        return CreatedAtAction(nameof(GetById), new { id = regionDto.Id }, regionDto);
    }

    // Update region
    // PUT: http://localhost:portnumber/api/regions/{id}
    [HttpPut]
    public IActionResult Update([FromBody] )
    {
        
    }
}