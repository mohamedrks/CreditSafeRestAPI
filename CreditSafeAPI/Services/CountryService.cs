using ChartAPI.Common;
using ChartAPI.Interfaces;
using ChartAPI.Model;
using ChartAPI.Model.DTOs;
using CreditSafeAPI.Interfaces;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ChartAPI.Services
{
    public class CountryService : ICountryService
    {
        private readonly IOpenWeatherService _openWeatherService;
        private readonly IRestCountryService  _restCountryService;
        private readonly WeatherContext _weatherContext;

        public CountryService
        (
            IOpenWeatherService openWeatherService,
            IRestCountryService restCountryService,
            WeatherContext weatherContext
        )
        {
            _weatherContext = weatherContext;
            _restCountryService = restCountryService;
            _openWeatherService = openWeatherService;

        }

        public async Task<IEnumerable<CityDto>> GetAllCountriesAsync()
        {
            var cities = _weatherContext.Set<City>();
            List<CityDto> citiesDto = new List<CityDto>();
            foreach (var city in cities)
            {
                var cityWeatherInfo = await _openWeatherService.GetCityWeatherInformation(city.Name);
                var cityInfo = await _restCountryService.GetCityInformation(city.Name);
                if( cityWeatherInfo != null && cityInfo != null)
                {
                    var cityDto = new CityDto()
                    {
                        Id = city.Id,
                        Name = city.Name,
                        Country = city.Country,
                        Currency = cityInfo.FirstOrDefault()?.currencies.FirstOrDefault()?.name + $" ({cityInfo.FirstOrDefault()?.currencies.FirstOrDefault()?.symbol})",
                        Weather = cityWeatherInfo.main.temp.ToString(),
                        DateEstablished = city.DateEstablished,
                        EstimatedPopulation = city.EstimatedPopulation,
                        State = city.State,
                        TouristRating = city.TouristRating,
                    };
                    citiesDto.Add(cityDto);
                }
            }
            return citiesDto;
        }

        public async Task<CityDto> GetCityInformationAsync(int id)
        {
            var city = await _weatherContext.Cities.FindAsync(id);
            if( city != null)
            {
                var cityWeatherInfo = await _openWeatherService.GetCityWeatherInformation(city.Name);
                var cityInfo = await _restCountryService.GetCityInformation(city.Name);
                if (cityWeatherInfo != null && cityInfo != null)
                {
                    var cityDto = new CityDto()
                    {
                        Id = city.Id,
                        Name = city.Name,
                        Country = city.Country,
                        Currency = cityInfo.FirstOrDefault()?.currencies.FirstOrDefault()?.name + $" ({cityInfo.FirstOrDefault()?.currencies.FirstOrDefault()?.symbol})",
                        Weather = cityWeatherInfo.main.temp.ToString(),
                        DateEstablished = city.DateEstablished,
                        EstimatedPopulation = city.EstimatedPopulation,
                        State = city.State,
                        TouristRating = city.TouristRating,
                    };
                    return cityDto;
                }
            }
            return null;
        }

        public async Task<City> Post(City city)
        {
            await _weatherContext.Cities.AddAsync(city);
            await _weatherContext.SaveChangesAsync();
            return city;
        }

        public async Task<City> Update(int id, City city)
        {
            if (id != city.Id)
            {
                return null;
            }

            var existingCity = await _weatherContext.Cities.FindAsync(id);
            if (existingCity == null)
            {
                return null;
            }

            existingCity.Name = city.Name;
            existingCity.State = city.State;
            existingCity.Country = city.Country;
            existingCity.TouristRating = city.TouristRating;
            existingCity.DateEstablished = city.DateEstablished;
            existingCity.EstimatedPopulation = city.EstimatedPopulation;

            await _weatherContext.SaveChangesAsync();
            return city;
        }

        public async Task<bool> Delete(int id)
        {
            var cityFound = await _weatherContext.Cities.FindAsync(id);

            if (cityFound == null)
            {
                return false;
            }

            _weatherContext.Cities.Remove(cityFound);
            await _weatherContext.SaveChangesAsync();

            return true;
        }

    }
}
