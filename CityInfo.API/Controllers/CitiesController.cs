using AutoMapper;
using CityInfo.API.Entities;
using CityInfo.API.Extensions;
using CityInfo.API.Models;
using CityInfo.API.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CityInfo.API.Controllers
{
    [Route("api/cities")]
    public class CitiesController : Controller
    {
        private ICityInfoRepository _cityInfoRepository;

        public CitiesController(ICityInfoRepository cityInfoRepository)
        {
            _cityInfoRepository = cityInfoRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetCities(bool includePointsOfInterest = false)
        {
            var citiesWithoutPOIDto = new List<CityWithoutPointOfInterestDto>();
            var citiesDto = new List<CityDto>();
            var cities = await _cityInfoRepository.GetCitiesAsync(includePointsOfInterest);
            foreach (City city in cities)
            {

                if (includePointsOfInterest)
                {
                    citiesDto.Add(Mapper.Map<CityDto>(city));
                }
                else
                {
                    citiesWithoutPOIDto.Add(Mapper.Map<CityWithoutPointOfInterestDto>(city));
                }
            }
            if (includePointsOfInterest)
            {
                return Ok(citiesDto);
            }
            else
            {
                return Ok(citiesWithoutPOIDto);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCity(int id, bool includePointsOfInterest = false)
        {
            if (!await _cityInfoRepository.CityExistsAsync(id))
            {
                return NotFound();
            }
            var city = await _cityInfoRepository.GetCityAsync(id, includePointsOfInterest);
            
            if (includePointsOfInterest)
            {
                return Ok(Mapper.Map<IEnumerable<CityDto>>(city));
            }
            else
            {
                return Ok(Mapper.Map<IEnumerable<CityWithoutPointOfInterestDto>>(city));
            }
        }
    }
}