using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWalks.CustomActionFilters;
using NZWalks.Models.Domain;
using NZWalks.Models.DTO;
using NZWalks.Models.Repositories;

namespace NZWalks.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WalksController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IWalkRepository WalkRepository;

        public WalksController(IMapper mapper, IWalkRepository walkRepository)
        {
            this.mapper = mapper;
            WalkRepository = walkRepository;
        }

        // Create Walk
        // Post: api/walks
        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> Create([FromBody] CreateWalkRequestDTO createWalkRequestDTO)
        {

            // Map DTO to Domain model
            var walkDomainModel = mapper.Map<Walk>(createWalkRequestDTO);

            await WalkRepository.CreateAsync(walkDomainModel);

            // Map Domain model to DTO
            var walksDto = mapper.Map<WalksDto>(walkDomainModel);

            return Ok(walksDto);
        }

        // Get walks
        // GET: api/walks?filterOn=Name&filterQuery=Track&sortBy=Name&isAscending=true
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string? filterOn, [FromQuery] string? filterQuery,
        [FromQuery] string? sortBy, [FromQuery] bool? isAscending, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 1000)
        {
            var walkDM = await WalkRepository.GetAllAsync(filterOn, filterQuery, sortBy, isAscending ?? true, pageNumber, pageSize);
            // Map Domain model to DTO
            var walkDTO = mapper.Map<List<WalksDto>>(walkDM);

            return Ok(walkDTO);
        }

        // Get walk by id
        // GET: /api/walks/{id}
        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var walkDM = await WalkRepository.GetByIdAsync(id);

            if (walkDM == null)
            {
                return NotFound();
            }

            var walksDto = mapper.Map<WalksDto>(walkDM);

            return Ok(walksDto);
        }

        [HttpPut]
        [Route("{id:guid}")]
        [ValidateModel]
        public async Task<IActionResult> Update([FromRoute] Guid id, UpdateWalksDto updateWalksDto)
        {

            var walkdDM = mapper.Map<Walk>(updateWalksDto);

            walkdDM = await WalkRepository.UpdateAsync(id, walkdDM);

            if (walkdDM == null)
            {
                return NotFound();
            }

            var walkDto = mapper.Map<WalksDto>(walkdDM);

            return Ok(walkDto);
        }

        // Delete walk by id
        // DELETE: /api/id
        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteById([FromRoute] Guid id)
        {
            var deletedWalkDM = await WalkRepository.DeleteByIdAsync(id);

            if (deletedWalkDM == null)
            {
                return NotFound();
            }

            // Map Domain model to DTO
            var deletedWalkDTO = mapper.Map<Walk>(deletedWalkDM);

            return Ok(deletedWalkDTO);
        }
    }
}