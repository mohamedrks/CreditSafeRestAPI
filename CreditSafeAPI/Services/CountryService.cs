using ChartAPI.Common;
using ChartAPI.Interfaces;
using ChartAPI.Model;
using ChartAPI.Model.DTOs;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ChartAPI.Services
{
    public class CountryService : ICountryService
    {

        private readonly IHttpClientFactory _clientFactory;
        private readonly WeatherContext _weatherContext;

        private readonly HttpClient _httpClientOpenWeather;
        private readonly HttpClient _httpClientRestcountries;
        private readonly string _API_KEY;

        public CountryService
        (
            IHttpClientFactory clientFactory,
            WeatherContext weatherContext
        )
        {
            _clientFactory = clientFactory;
            _weatherContext = weatherContext;

            _httpClientOpenWeather = _clientFactory.CreateClient();
            _httpClientRestcountries = _clientFactory.CreateClient();

            _httpClientRestcountries.BaseAddress = new System.Uri("https://restcountries.com/v2/");
            _httpClientOpenWeather.BaseAddress = new System.Uri("https://api.openweathermap.org/data/2.5/weather");
            _API_KEY = "71dc8df57b816d6f463f5f82b2f65376";
        }

        public async Task<IEnumerable<CityDto>> GetAllCountriesAsync()
        {
            var cities = _weatherContext.Set<City>();
            List<CityDto> citiesDto = new List<CityDto>();
            foreach (var city in cities)
            {
                var cityWeatherInfo = await GetCityWeatherInformation(city.Name);
                var cityInfo = await GetCityInformation(city.Name);
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
                var cityWeatherInfo = await GetCityWeatherInformation(city.Name);
                var cityInfo = await GetCityInformation(city.Name);
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

        private async Task<CityWeatherInfo> GetCityWeatherInformation(string cityName)
        {
            var response = await _httpClientOpenWeather.GetAsync($"?q={cityName}&appid={_API_KEY}");
            if (response.IsSuccessStatusCode)
            {
                var responseData = await response.Content.ReadAsStringAsync();
                var model = JsonConvert.DeserializeObject<CityWeatherInfo>(responseData);
                return model;
            }

            return null;
        }

        private async Task<IList<CityInfo>> GetCityInformation(string cityName)
        {
            var response = await _httpClientRestcountries.GetAsync($"capital/{cityName}");
            if (response.IsSuccessStatusCode)
            {
                var responseData = await response.Content.ReadAsStringAsync();
                var model = JsonConvert.DeserializeObject<IList<CityInfo>>(responseData);
                return model;
            }

            return null;
        }
    }
}
