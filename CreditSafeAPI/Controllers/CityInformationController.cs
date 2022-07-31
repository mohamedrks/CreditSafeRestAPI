using ChartAPI.Common;
using ChartAPI.Interfaces;
using ChartAPI.Model;
using ChartAPI.Model.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChartAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CityInformationController : ControllerBase
    {

        private readonly ICountryService _countryService;

        public CityInformationController(
             ICountryService countryService
        )
        {
            _countryService = countryService;
        }


        [HttpGet]
        [ResponseCache(VaryByHeader = "User-Agent", Duration = 30)]
        public async Task<IEnumerable<CityDto>> Get() 
        {
            return await _countryService.GetAllCountriesAsync();
        }

        [Route("GetCityInfo/{id}")]
        public async Task<CityDto> GetCityInformation(int id)
        {
            return await _countryService.GetCityInformationAsync(id);
        }

        [HttpPost]
        public async Task<City> Post(City city)
        {
            return await _countryService.Post(city);
        }

        [HttpPut("{id}")]
        public async Task<City> Update(int id, City city)
        {
            return await _countryService.Update(id, city);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var isDeleted = await _countryService.Delete(id);

            if (isDeleted)
            {
                return NoContent();
            }
            return NotFound();
            
        }
    }
}
