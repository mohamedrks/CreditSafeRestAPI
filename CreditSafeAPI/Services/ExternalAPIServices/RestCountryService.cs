using ChartAPI.Model.DTOs;
using CreditSafeAPI.Interfaces;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace CreditSafeAPI.Services.ExternalAPIServices
{
    public class RestCountryService : IRestCountryService
    {

        private readonly IHttpClientFactory _clientFactory;
        private readonly IConfiguration _configuration;

        private readonly HttpClient _httpClient;

        public RestCountryService
        (
            IHttpClientFactory clientFactory,
            IConfiguration configuration
        )
        {
            _clientFactory = clientFactory;
            _configuration = configuration;

            _httpClient = _clientFactory.CreateClient();
            _httpClient.BaseAddress = new System.Uri(_configuration["Settings:RestCountry_API_URL"]);
        }

        public async Task<IList<CityInfo>> GetCityInformation(string cityName)
        {
            var response = await _httpClient.GetAsync($"capital/{cityName}");
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
