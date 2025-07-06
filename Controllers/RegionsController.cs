using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalks.Data;
using NZWalks.Models.Domain;
using NZWalks.Models.DTO;
using NZWalks.Models.Repositories;

namespace NZWalks.Controllers
{
    //https://localhost:7259/api/regions
    [ApiController]
    [Route("api/[controller]")]
    public class RegionsController : ControllerBase
    {
        private readonly NZWalksDbContext dbContext;
        private readonly IRegionRepository regionRepository;
        public RegionsController(NZWalksDbContext dbContext, IRegionRepository regionRepository)
        {
            this.dbContext = dbContext;
            this.regionRepository = regionRepository;
        }
        //GET ALL REGIONS
        //GET: https://localhost:7259/api/regions
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            // Get data from database - Domain Model
            var Regions = await regionRepository.GetAllAsync();

            //Map Domain Models to DTOs
            var RegionsDto = new List<RegionDTO>();
            foreach (var region in Regions)
            {
                RegionsDto.Add(
                    new RegionDTO
                    {
                        Id = region.Id,
                        Name = region.Name,
                        Code = region.Code,
                        RegionImageUrl = region.RegionImageUrl
                    }
                );
            }

            // Return DTOs
            return Ok(RegionsDto);
        }

        //GET SINGLE REGION BY ID
        //GET: https://localhost:7259/api/regions/{id}
        [HttpGet]
        [Route("{id:guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            // Get data from database - Domain Model
            var region = await regionRepository.GetByIdAsync(id);
            //Map Domain Models to DTOs
            var RegionsDto = new List<RegionDTO>();

            if (region == null)
                return NotFound();

            var regionDto = new RegionDTO
            {
                Id = region.Id,
                Name = region.Name,
                Code = region.Code,
                RegionImageUrl = region.RegionImageUrl
            };
            // Return DTOs
            return Ok(regionDto);
        }

        // Post to create new region
        // POST: https://localhost:7259/api/regions
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateRegionRequest createRegionRequest)
        {
            //Map DTO to Domain Model
            var regionDomainModel = new Region
            {
                Name = createRegionRequest.Name,
                Code = createRegionRequest.Code,
                RegionImageUrl = createRegionRequest.RegionImageUrl
            };

            //Use Domain Model to create a new region
            regionDomainModel = await regionRepository.CreateAsync(regionDomainModel);

            //Map Domain Model back to DTO
            var regionDto = new RegionDTO
            {
                Id = regionDomainModel.Id,
                Name = regionDomainModel.Name,
                Code = regionDomainModel.Code,
                RegionImageUrl = regionDomainModel.RegionImageUrl
            };

            return CreatedAtAction(nameof(GetById), new { Id = regionDto.Id }, regionDto);
        }

        // Put to update a region
        // PUT: https://localhost:7259/api/regions/{id}
        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateRegionRequest updateRegionRequest)
        {
            var RegionDM = new Region
            {
                Code = updateRegionRequest.Code,
                Name = updateRegionRequest.Name,
                RegionImageUrl = updateRegionRequest.RegionImageUrl
            };

            RegionDM = await regionRepository.UpdateAsync(id, RegionDM);

            if (RegionDM == null)
            {
                return NotFound();
            }

            RegionDM.Name = updateRegionRequest.Name;
            RegionDM.Code = updateRegionRequest.Code;
            RegionDM.RegionImageUrl = updateRegionRequest.RegionImageUrl;

            await dbContext.SaveChangesAsync();

            var RegionDto = new RegionDTO
            {
                Id = RegionDM.Id,
                Name = RegionDM.Name,
                Code = RegionDM.Code,
                RegionImageUrl = RegionDM.RegionImageUrl
            };
            return Ok(RegionDto);
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var RegionDM = await regionRepository.DeleteAsync(id);
            if (RegionDM == null)
            {
                return NotFound();
            }

            var RegionDTO = new Region
            {
                Id = RegionDM.Id,
                Name = RegionDM.Name,
                Code = RegionDM.Code,
                RegionImageUrl = RegionDM.RegionImageUrl
            };

            return Ok(RegionDTO);
        }
    }
}