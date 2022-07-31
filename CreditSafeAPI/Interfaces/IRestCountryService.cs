using ChartAPI.Model.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CreditSafeAPI.Interfaces
{
    public interface IRestCountryService
    {
        Task<IList<CityInfo>> GetCityInformation(string cityName);
        
    }
}
