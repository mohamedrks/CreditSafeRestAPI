using ChartAPI.Model.DTOs;
using CreditSafeAPI.Interfaces;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;

namespace CreditSafeAPI.Services.ExternalAPIServices
{
    public class OpenWeatherService : IOpenWeatherService
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly IConfiguration _configuration;

        private readonly HttpClient _httpClient;
        private readonly string _API_KEY;

        public OpenWeatherService
        (
            IHttpClientFactory clientFactory,
            IConfiguration configuration
        )
        {
            _clientFactory = clientFactory;
            _configuration = configuration;

            _httpClient = _clientFactory.CreateClient();

            _httpClient.BaseAddress = new System.Uri(_configuration["Settings:OpenWeather_API_URL"]);
            _API_KEY = _configuration["Settings:OpenWeather_API_Key"];
        }

        public async Task<CityWeatherInfo> GetCityWeatherInformation(string cityName)
        {
            var response = await _httpClient.GetAsync($"?q={cityName}&appid={_API_KEY}");
            if (response.IsSuccessStatusCode)
            {
                var responseData = await response.Content.ReadAsStringAsync();
                var model = JsonConvert.DeserializeObject<CityWeatherInfo>(responseData);
                return model;
            }

            return null;
        }
    }
}
