using ChartAPI.Common;
using ChartAPI.Interfaces;
using ChartAPI.Services;
using CreditSafeAPI.Interfaces;
using CreditSafeAPI.Services.ExternalAPIServices;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ChartAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.WithOrigins("http://localhost:4200")
                    .AllowAnyMethod()
                    .AllowAnyOrigin()
                    .AllowAnyHeader());
                //.AllowCredentials());
            });

            services.AddDbContext<WeatherContext>(op => op.UseSqlServer(Configuration.GetConnectionString("Database")));


            services.AddHttpClient();

            services.AddSingleton(Configuration);

            services.AddTransient<ICountryService, CountryService>();
            services.AddTransient<IOpenWeatherService, OpenWeatherService>();
            services.AddTransient<IRestCountryService, RestCountryService>();

            services.AddResponseCaching();

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseResponseCaching();

            app.UseRouting();

            app.UseCors("CorsPolicy");

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
