using ChartAPI.Model;
using ChartAPI.Model.DTOs;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace ChartAPI.Interfaces
{
    public interface ICountryService
    {
        Task<IEnumerable<CityDto>> GetAllCountriesAsync();

        Task<CityDto> GetCityInformationAsync(int id);

        Task<City> Update(int id, City city);

        Task<City> Post(City city);

        Task<bool> Delete(int id);
    }
}
