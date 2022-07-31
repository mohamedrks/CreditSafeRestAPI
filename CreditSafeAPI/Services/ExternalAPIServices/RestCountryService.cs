using ChartAPI.Model.DTOs;
using CreditSafeAPI.Interfaces;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace CreditSafeAPI.Services.ExternalAPIServices
{
    public class RestCountryService : IRestCountryService
    {

        private readonly IHttpClientFactory _clientFactory;

        private readonly HttpClient _httpClient;

        public RestCountryService
        (
            IHttpClientFactory clientFactory
        )
        {
            _clientFactory = clientFactory;

            _httpClient = _clientFactory.CreateClient();

            _httpClient.BaseAddress = new System.Uri("https://restcountries.com/v2/");
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
