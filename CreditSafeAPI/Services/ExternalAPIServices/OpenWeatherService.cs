using ChartAPI.Model.DTOs;
using CreditSafeAPI.Interfaces;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;

namespace CreditSafeAPI.Services.ExternalAPIServices
{
    public class OpenWeatherService : IOpenWeatherService
    {
        private readonly IHttpClientFactory _clientFactory;

        private readonly HttpClient _httpClient;
        private readonly string _API_KEY;

        public OpenWeatherService
        (
            IHttpClientFactory clientFactory
        )
        {
            _clientFactory = clientFactory;

            _httpClient = _clientFactory.CreateClient();

            _httpClient.BaseAddress = new System.Uri("https://api.openweathermap.org/data/2.5/weather");
            _API_KEY = "71dc8df57b816d6f463f5f82b2f65376";
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
