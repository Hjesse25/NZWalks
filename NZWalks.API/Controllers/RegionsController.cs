using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.DTOs;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Repositories;
using AutoMapper;

namespace NZWalks.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RegionsController : ControllerBase
{
    private readonly NZWalksDbContext dbContext;
    private readonly IRegionRepository regionRepository;
    private readonly IMapper mapper;

    public RegionsController(NZWalksDbContext dbContext, IRegionRepository regionRepository,
        IMapper mapper)
    {
        this.dbContext = dbContext;
        this.regionRepository = regionRepository;
        this.mapper = mapper;
    }

    // GET all regions
    // GET http://localhost:portnumber/api/regions
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        // Get Data from Database - Domain models
        var regionsDomain = await regionRepository.GetAllAsync();

        // Return DTOs
        return Ok(mapper.Map<List<RegionDto>>(regionsDomain));
    }

    // GET single region (Get region by ID)
    // GET http://localhost:portnumber/api/regions/{id}
    [HttpGet]
    [Route("{id:Guid}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    {
        // var region = dbContext.Regions.Find(id);
        // Get region domain model from database
        var regionDomain = await regionRepository.GetByIdAsync(id);

        if (regionDomain == null)
        {
            return NotFound();
        }

        // return DTO back to client
        return Ok(mapper.Map<RegionDto>(regionDomain));
    }

    // POST To create new region
    // POST: http://localhost:portnumber/api/regions
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] AddRegionRequestDto request)
    {
        // Map or Convert DTO to Domain Model
        var regionDomainModel = mapper.Map<Region>(request);

        // Use Domain Model to create Region
        regionDomainModel = await regionRepository.CreateAsync(regionDomainModel);

        // Map Domain model back to DTO
        var regionDto = mapper.Map<RegionDto>(regionDomainModel);

        return CreatedAtAction(nameof(GetById), new { id = regionDto.Id }, regionDto);
    }

    // Update region
    // PUT: http://localhost:portnumber/api/regions/{id}
    [HttpPut]
    [Route("{id:Guid}")]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateRegionRequestDto request)
    {
        // Map DTO to Domain Model
        var regionDomainModel = mapper.Map<Region>(request);

        // Check if region exists
        regionDomainModel = await regionRepository.UpdateAsync(id, regionDomainModel);

        if (regionDomainModel == null)
        {
            return NotFound();
        }

        return Ok(mapper.Map<RegionDto>(regionDomainModel));
    }

    // Delete region
    // DELETE: http://localhost:portnumber/api/regions/{id}
    [HttpDelete]
    [Route("{id:Guid}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        var regionDomainModel = await regionRepository.DeleteAsync(id);

        if (regionDomainModel == null)
        {
            return NotFound();
        }

        return Ok(mapper.Map<RegionDto>(regionDomainModel));
    }
}