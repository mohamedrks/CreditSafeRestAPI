using ChartAPI.Model.DTOs;
using System.Threading.Tasks;

namespace CreditSafeAPI.Interfaces
{
    public interface IOpenWeatherService
    {
        Task<CityWeatherInfo> GetCityWeatherInformation(string cityName);
    }
}
