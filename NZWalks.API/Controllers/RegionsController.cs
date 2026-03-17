using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NZWalks.CustomActionFilters;
using NZWalks.Models.Domain;
using NZWalks.Models.DTO;
using NZWalks.Models.Repositories;

namespace NZWalks.Controllers
{
    // https://localhost:7259/api/regions
    [ApiController]
    [Route("api/[controller]")]

    public class RegionsController : ControllerBase
    {
        private readonly IRegionRepository regionRepository;
        private readonly IMapper mapper;
        public RegionsController(IRegionRepository regionRepository, IMapper mapper)
        {
            this.regionRepository = regionRepository;
            this.mapper = mapper;
        }
        //GET ALL REGIONS
        //GET: https://localhost:7259/api/regions
        [HttpGet]
        [Authorize(Roles = "Reader, Writer")]
        public async Task<IActionResult> GetAll()
        {
            // Get data from database - Domain Model
            var RegionsDM = await regionRepository.GetAllAsync();

            //Map Domain Models to DTOs
            var RegionsDto = mapper.Map<List<RegionDTO>>(RegionsDM);

            // Return DTOs
            return Ok(RegionsDto);
        }

        //GET SINGLE REGION BY ID
        //GET: https://localhost:7259/api/regions/{id}
        [HttpGet]
        [Route("{id:guid}")]
        [Authorize(Roles = "Reader, Writer")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            // Get data from database - Domain Model
            var region = await regionRepository.GetByIdAsync(id);
            //Map Domain Models to DTOs
            var RegionsDto = new List<RegionDTO>();

            if (region == null)
                return NotFound();

            var regionDto = mapper.Map<RegionDTO>(region);
            // Return DTOs
            return Ok(regionDto);
        }

        // Post to create new region
        // POST: https://localhost:7259/api/regions
        [HttpPost]
        [ValidateModel]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> Create([FromBody] CreateRegionRequest createRegionRequestDto)
        {

            //Map DTO to Domain Model
            var regionDomainModel = mapper.Map<Region>(createRegionRequestDto);

            //Use Domain Model to create a new region
            regionDomainModel = await regionRepository.CreateAsync(regionDomainModel);

            //Map Domain Model back to DTO
            var regionDto = mapper.Map<RegionDTO>(regionDomainModel);

            return CreatedAtAction(nameof(GetById), new { Id = regionDto.Id }, regionDto);
        }

        // Put to update a region
        // PUT: https://localhost:7259/api/regions/{id}
        [HttpPut]
        [Route("{id:guid}")]
        [ValidateModel]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateRegionRequest updateRegionRequest)
        {

            // DTO -> DM
            var RegionDM = mapper.Map<Region>(updateRegionRequest);

            RegionDM = await regionRepository.UpdateAsync(id, RegionDM);

            if (RegionDM == null)
            {
                return NotFound();
            }

            // DM -> DTO
            var RegionDto = mapper.Map<RegionDTO>(RegionDM);

            return Ok(RegionDto);

        }

        [HttpDelete]
        [Route("{id:guid}")]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var RegionDM = await regionRepository.DeleteAsync(id);

            if (RegionDM == null)
            {
                return NotFound();
            }

            var RegionDTO = mapper.Map<RegionDTO>(RegionDM);

            return Ok(RegionDTO);
        }
    }
}