using ChartAPI.Model;
using Microsoft.EntityFrameworkCore;

namespace ChartAPI.Common
{
    public class WeatherContext : DbContext
    {
        public WeatherContext(DbContextOptions<WeatherContext> options) : base(options)
        {
        }

        public DbSet<City> Cities { get; set; }
    }
}

