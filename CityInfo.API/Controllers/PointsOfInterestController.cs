using AutoMapper;
using CityInfo.API.Entities;
using CityInfo.API.Extensions;
using CityInfo.API.Models;
using CityInfo.API.Services;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CityInfo.API.Controllers
{
    [Route("api/cities")]
    public class PointsOfInterestController : Controller
    {
        private ILogger<PointsOfInterestController> _logger;
        private IMailService _mailService;
        private ICityInfoRepository _cityInfoRepository;

        public PointsOfInterestController(ILogger<PointsOfInterestController> logger,
            IMailService service,
            ICityInfoRepository cityInfoRepository)
        {
            _logger = logger;
            _mailService = service;
            _cityInfoRepository = cityInfoRepository;
        }
        [HttpGet("{cityId}/pointofinterest")]
        public async Task<IActionResult> GetPointsOfInterest(int cityId)
        {
            if (!await _cityInfoRepository.CityExistsAsync(cityId))
            {
                _logger.LogInformation($"city with id {cityId} not found");
                return NotFound();
            }

            var city = await _cityInfoRepository.GetCityAsync(cityId, true);
            return Ok(Mapper.Map<IEnumerable<PointOfInterestDto>>(city.PointsOfInterest));
        }

        [HttpGet("{cityId}/pointofinterest/{id}", Name = "GetPointOfInterest")]
        public async Task<IActionResult> GetPointOfInterest(int cityId, int id)
        {
            if (!await _cityInfoRepository.CityExistsAsync(cityId))
            {
                _logger.LogInformation($"city with id {cityId} not found");
                return BadRequest();
            }
            //var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);
            var city = await _cityInfoRepository.GetCityAsync(cityId, true);
            var pointOfInterest = city.PointsOfInterest.EmptyIfNull().FirstOrDefault(p => p.Id == id);
            if (pointOfInterest == null)
            {
                return BadRequest();
            }
            return Ok(Mapper.Map<PointOfInterestDto>(pointOfInterest));
        }

        [HttpPost("{cityId}/pointofinterest")]
        public async Task<IActionResult> CreatePointOfInterest(int cityId,
            [FromBody] PointOfInterestForCreationDto pointOfInterest)
        {
            if (pointOfInterest == null)
            {
                return BadRequest();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!await _cityInfoRepository.CityExistsAsync(cityId))
            {
                _logger.LogInformation($"city with id {cityId} not found");
                return NotFound();
            }

            var newPointOfInterest = Mapper.Map<PointOfInterest>(pointOfInterest);
            await _cityInfoRepository.AddPointOfInterestForCityAsync(cityId, newPointOfInterest);
            if (! await _cityInfoRepository.SaveAsync())
            {
                return StatusCode(500, $"Something went wrong while creating the point of interest");
            }
            var newPointOfInterestDto = Mapper.Map<PointOfInterestDto>(newPointOfInterest);
            return CreatedAtRoute("GetPointOfInterest", new { cityId, id = newPointOfInterestDto.Id }, newPointOfInterestDto);
        }

        [HttpPut("{cityId}/pointofinterest/{id}")]
        public async Task<IActionResult> UpdatePointOfInterest(int cityId, int id,
            [FromBody]PointOfInterestForUpdateDto pointOfInterest)
        {
            if (pointOfInterest == null)
            {
                return BadRequest();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!await _cityInfoRepository.CityExistsAsync(cityId))
            {
                _logger.LogInformation($"city with id {cityId} not found");
                return NotFound();
            }

            var existingPointOfInterest = await _cityInfoRepository.GetPointOfInterestForCityAsync(cityId, id);
            if (existingPointOfInterest == null)
            {
                return NotFound();
            }

            Mapper.Map(pointOfInterest, existingPointOfInterest);
            if (!await _cityInfoRepository.SaveAsync())
            {
                return StatusCode(500, $"Something went wrong while updating the point of interest with id {id}");
            }
            return NoContent();
        }

        [HttpPatch("{cityId}/pointofinterest/{id}")]
        public async Task<IActionResult> PartiallyUpdatePointOfInterest(int cityId, int id,
            [FromBody]JsonPatchDocument<PointOfInterestForUpdateDto> patchDoc)
        {
            if (patchDoc == null)
            {
                return BadRequest();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!await _cityInfoRepository.CityExistsAsync(cityId))
            {
                _logger.LogInformation($"city with id {cityId} not found");
                return NotFound();
            }

            var existingPointOfInterest = await _cityInfoRepository.GetPointOfInterestForCityAsync(cityId, id);
            if (existingPointOfInterest == null)
            {
                return NotFound();
            }

            var pointOfInterestToPatch = Mapper.Map<PointOfInterestForUpdateDto>(existingPointOfInterest);

            patchDoc.ApplyTo(pointOfInterestToPatch, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await TryUpdateModelAsync(pointOfInterestToPatch);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Mapper.Map(pointOfInterestToPatch, existingPointOfInterest);
            if (! await _cityInfoRepository.SaveAsync())
            {
                return StatusCode(500, $"Something went wrong while patching point of interest with id {id}");
            }
            return NoContent();
        }

        [HttpDelete("{cityId}/pointofinterest/{id}")]
        public async Task<IActionResult> DeletePointOfInterest(int cityId, int id)
        {
            if (!await _cityInfoRepository.CityExistsAsync(cityId))
            {
                _logger.LogInformation($"city with id {cityId} not found");
                return NotFound();
            }

            var existingPointOfInterest = await _cityInfoRepository.GetPointOfInterestForCityAsync(cityId, id);
            if (existingPointOfInterest == null)
            {
                return NotFound();
            }

            await _cityInfoRepository.DeletePointOfInterestAsync(existingPointOfInterest);

            if (!await _cityInfoRepository.SaveAsync())
            {
                return StatusCode(500, $"Something went wrong while deleting point of interest with id {id}");
            }

            _mailService.Send("Point of interest deleted",
                $"Point of intereset {existingPointOfInterest.Name} has been deleted.");
            return NoContent();
        }

    }
}