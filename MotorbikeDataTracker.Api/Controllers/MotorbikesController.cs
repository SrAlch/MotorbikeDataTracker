using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MotorbikeDataTracker.Api.Dtos;
using MotorbikeDataTracker.Api.Entities;
using MotorbikeDataTracker.Api.Repositories;

namespace MotorbikeDataTracker.Api.Controllers
{
    [ApiController]
    [Route("motorbikes")]
    public class MotorbikesController : ControllerBase
    {
        private readonly IMotorbikesRepository repository;
        private readonly ILogger<MotorbikesController> logger;

        public MotorbikesController(IMotorbikesRepository repository, ILogger<MotorbikesController> logger)
        {
            this.repository = repository;
            this.logger = logger;
        }

        // GET /items
        [HttpGet]
        public async Task<IEnumerable<MotorbikeDto>> GetMotorbikesAsync(string name = null)
        {
            var motorbikes = (await repository.GetMotorbikesAsync())
                        .Select( motorbike => motorbike.AsDto());

            if (!string.IsNullOrWhiteSpace(name))
            {
                motorbikes = motorbikes.Where(motorbike => motorbike.Brand.Contains(name, StringComparison.OrdinalIgnoreCase));
            }

            logger.LogInformation($"{DateTime.UtcNow.ToString("hh:mm:ss")}: Retrived {motorbikes.Count()} items");

            return motorbikes;
        }

        // GET /items/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<MotorbikeDto>> GetMotorbikeAsync(Guid id)
        {
            var motorbike = await repository.GetMotorbikeAsync(id);

            if (motorbike is null)
            {
                return NotFound();
            }

            return motorbike.AsDto(); 
        }

        //POST /items
        [HttpPost]
        public async Task<ActionResult<MotorbikeDto>> CreateMotorbikeAsync(CreateMotorbikeDto motorbikeDto)
        {
            Motorbike motorbike = new()
            {
                Id = Guid.NewGuid(),
                Brand = motorbikeDto.Brand,
                Year = motorbikeDto.Year,
                Model = motorbikeDto.Model,
                Trim = motorbikeDto.Trim
            };

            await repository.CreateMotorbikeAsync(motorbike);

            return CreatedAtAction(nameof(GetMotorbikeAsync), new { id = motorbike.Id }, motorbike.AsDto());
        }

        // PUT /items/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateMotorbikeAsync(Guid id, UpdateMotorbikeDto motorbikeDto)
        {
            var existingItem = await repository.GetMotorbikeAsync(id);

            if (existingItem is null)
            {
                return NotFound();
            }
            
            existingItem.Brand = motorbikeDto.Brand;
            existingItem.Year = motorbikeDto.Year;
            existingItem.Model = motorbikeDto.Model;
            existingItem.Trim = motorbikeDto.Trim;
           
            await repository.UpdateMotorbikeAsync(existingItem);

            return NoContent();
        }

        // DELETE /items/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteMotorbikeAsync(Guid id)
        {
            var existingItem = await repository.GetMotorbikeAsync(id);

            if (existingItem is null)
            {
                return NotFound();
            }

            await repository.DeleteMotorbikeAsync(id);
            
            return NoContent();
        }
    }
}