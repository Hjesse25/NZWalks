using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.DTOs;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using Microsoft.EntityFrameworkCore;

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
    public async Task<IActionResult> GetAll()
    {
        // Get Data from Database - Domain models
        var regionsDomain = await dbContext.Regions.ToListAsync();

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
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    {
        // var region = dbContext.Regions.Find(id);
        // Get region domain model from database
        var regionDomain = await dbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);

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
    public async Task<IActionResult> Create([FromBody] AddRegionRequestDto request)
    {
        // Map or Convert DTO to Domain Model
        var regionDomainModel = new Region
        {
            Code = request.Code,
            Name = request.Name,
            RegionImageUrl = request.RegionImageUrl
        };

        // Use Domain Model to create Region
        await dbContext.Regions.AddAsync(regionDomainModel);
        await dbContext.SaveChangesAsync();

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
    [Route("{id:Guid}")]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateRegionRequestDto request)
    {
        // Check if region exists
        var regionDomainModel = await dbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);

        if (regionDomainModel == null)
        {
            return NotFound();
        }

        // Map DTO to Domain model
        regionDomainModel.Code = request.Code;
        regionDomainModel.Name = request.Name;
        regionDomainModel.RegionImageUrl = request.RegionImageUrl;

        await dbContext.SaveChangesAsync();

        // Convert Domain model to DTO
        var regionDto = new RegionDto
        {
            Id = regionDomainModel.Id,
            Code = regionDomainModel.Code,
            Name = regionDomainModel.Name,
            RegionImageUrl = regionDomainModel.RegionImageUrl
        };

        return Ok(regionDto);
    }

    // Delete region
    // DELETE: http://localhost:portnumber/api/regions/{id}
    [HttpDelete]
    [Route("{id:Guid}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        var regionDomainModel = await dbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);

        if (regionDomainModel == null)
        {
            return NotFound();
        }

        // Delete region
        dbContext.Regions.Remove(regionDomainModel);
        await dbContext.SaveChangesAsync();

        // return deleted Region back
        // map Domain model to DTO
        // you can leave the return type empty without DTO
        var regionDto = new RegionDto
        {
            Id = regionDomainModel.Id,
            Code = regionDomainModel.Code,
            Name = regionDomainModel.Name,
            RegionImageUrl = regionDomainModel.RegionImageUrl
        };

        return Ok(regionDto);
    }
}