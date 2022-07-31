using ChartAPI.Common;
using ChartAPI.Model;
using ChartAPI.Services;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using CreditSafeAPI.Interfaces;
using ChartAPI.Model.DTOs;

namespace UnitTestsCreditsafe
{
    [TestFixture]
    class CountryServiceTest
    {
        [SetUp]
        public void Setup()
        {
           
        }

        [Test]
        public void CountryService_Success_ReturnAllCities_UnitTest()
        {
            City city1 = new City
            {
                Id = 1, 
                Name = "Paris",
                Country = "France",
                State = "Caen",
                TouristRating = 4,
                DateEstablished = 1760,
                EstimatedPopulation = 910000

            };

            City city2 = new City
            {
                Id = 2,
                Name = "Singapore",
                Country = "Singapore",
                State = "Boonkeng",
                TouristRating = 4,
                DateEstablished = 1960,
                EstimatedPopulation = 5550000

            };

            City city3 = new City
            {
                Id = 3,
                Name = "Paris",
                Country = "France",
                State = "Caen",
                TouristRating = 4,
                DateEstablished = 1760,
                EstimatedPopulation = 910000

            };


            City city4 = new City
            {
                Id = 4,
                Name = "Paris",
                Country = "France",
                State = "Caen",
                TouristRating = 4,
                DateEstablished = 1760,
                EstimatedPopulation = 910000

            };


            List<City> cities = new List<City> { city1, city2, city3, city4 };
            Currency[] parisCurrencies = new Currency[] 
            {
                new Currency() { symbol = "$" ,code = "FC" ,name ="Franc"}
            };

            Currency[] singaporeCurrencies = new Currency[]
            {
                new Currency() { symbol = "$" ,code = "SG" ,name ="Franc"}
            };

            var mockRestCountryService = new Mock<IRestCountryService>();
            mockRestCountryService.Setup(m => m.GetCityInformation(It.Is<string>(y => ReferenceEquals("Paris", y)))).ReturnsAsync(new List<CityInfo>() { new CityInfo() { currencies = parisCurrencies } });
            mockRestCountryService.Setup(m => m.GetCityInformation(It.Is<string>(y => ReferenceEquals("Singapore", y)))).ReturnsAsync(new List<CityInfo>() { new CityInfo() { currencies = singaporeCurrencies } });

            var mockOpenWeatherService = new Mock<IOpenWeatherService>();
            mockOpenWeatherService.Setup(m => m.GetCityWeatherInformation(It.Is<string>(y => ReferenceEquals("Paris", y)))).ReturnsAsync(new CityWeatherInfo () { main = new Main() { temp = 286 } });
            mockOpenWeatherService.Setup(m => m.GetCityWeatherInformation(It.Is<string>(y => ReferenceEquals("Singapore", y)))).ReturnsAsync(new CityWeatherInfo() { main = new Main() { temp = 230 } });


            var optionsBuilder = new DbContextOptionsBuilder<WeatherContext>();
            optionsBuilder.UseInMemoryDatabase("CreditSafe1");
            var _dbContext = new WeatherContext(optionsBuilder.Options);
            // Add sample data
            _dbContext.Cities.Add(city1);
            _dbContext.Cities.Add(city2);
            _dbContext.Cities.Add(city3);
            _dbContext.Cities.Add(city4);

            _dbContext.SaveChanges();

            var httpClientFactoryMock = new Mock<IHttpClientFactory>();
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();

            var countryService = new CountryService( mockOpenWeatherService.Object, mockRestCountryService.Object, _dbContext);

            // Act
            var allCities = countryService.GetAllCountriesAsync();

            // Assert
            
            Assert.AreEqual(4, allCities.Result.Count());
        }

        [Test]
        public void CountryService_Success_SkipNotFoundCities_ReturnFewCitiesThanOnList_UnitTest()
        {


            City city1 = new City
            {
                Id = 1,
                Name = "Paris",
                Country = "France",
                State = "Île-de-France",
                TouristRating = 5,
                DateEstablished = 1860,
                EstimatedPopulation = 780000

            };

            City city2 = new City
            {
                Id = 2,
                Name = "Singapore",
                Country = "Singapore",
                State = "Boonkeng",
                TouristRating = 4,
                DateEstablished = 1840,
                EstimatedPopulation = 550000

            };

            City city3 = new City
            {
                Id = 3,
                Name = "London",
                Country = "United Kingdom",
                State = "England",
                TouristRating = 5,
                DateEstablished = 1460,
                EstimatedPopulation = 1510000

            };


            City city4 = new City
            {
                Id = 4,
                Name = "Islamabath",
                Country = "Pakistan",
                State = "bihar",
                TouristRating = 2,
                DateEstablished = 1920,
                EstimatedPopulation = 1910000

            };

            List<City> cities = new List<City> { city1, city2, city3, city4 };

            var optionsBuilder = new DbContextOptionsBuilder<WeatherContext>();
            optionsBuilder.UseInMemoryDatabase("CreditSafe2");
            var _dbContext = new WeatherContext(optionsBuilder.Options);
            // Add sample data
            _dbContext.Cities.Add(city1);
            _dbContext.Cities.Add(city2);
            _dbContext.Cities.Add(city3);
            _dbContext.Cities.Add(city4);

            _dbContext.SaveChanges();

            Currency[] parisCurrencies = new Currency[]
            {
                            new Currency() { symbol = "EU" ,code = "FC" ,name ="Franc"}
            };

            Currency[] singaporeCurrencies = new Currency[]
            {
                            new Currency() { symbol = "$" ,code = "SG" ,name ="Singapore Dollar"}
            };

            var mockRestCountryService = new Mock<IRestCountryService>();
            mockRestCountryService.Setup(m => m.GetCityInformation(It.Is<string>(y => ReferenceEquals("Paris", y)))).ReturnsAsync(new List<CityInfo>() { new CityInfo() { currencies = parisCurrencies } });
            mockRestCountryService.Setup(m => m.GetCityInformation(It.Is<string>(y => ReferenceEquals("Singapore", y)))).ReturnsAsync(new List<CityInfo>() { new CityInfo() { currencies = singaporeCurrencies } });

            var mockOpenWeatherService = new Mock<IOpenWeatherService>();
            mockOpenWeatherService.Setup(m => m.GetCityWeatherInformation(It.Is<string>(y => ReferenceEquals("Paris", y)))).ReturnsAsync(new CityWeatherInfo() { main = new Main() { temp = 286 } });
            mockOpenWeatherService.Setup(m => m.GetCityWeatherInformation(It.Is<string>(y => ReferenceEquals("Singapore", y)))).ReturnsAsync(new CityWeatherInfo() { main = new Main() { temp = 230 } });


            var countryService = new CountryService( mockOpenWeatherService.Object, mockRestCountryService.Object, _dbContext);

            // Act
            var allCities = countryService.GetAllCountriesAsync();

            // Assert

            Assert.AreEqual(2, allCities.Result.Count());
        }

        [Test]
        public void CountryService_Success_SkipNotFoundCities_ReturnEmptyCities_UnitTest()
        {


            City city1 = new City
            {
                Id = 1,
                Name = "Canberra",
                Country = "Australia",
                State = "Southeastern",
                TouristRating = 5,
                DateEstablished = 1860,
                EstimatedPopulation = 780000

            };

            City city2 = new City
            {
                Id = 2,
                Name = "Wellington",
                Country = "	Zew Zealand",
                State = "Auckland",
                TouristRating = 4,
                DateEstablished = 1840,
                EstimatedPopulation = 550000

            };

            City city3 = new City
            {
                Id = 3,
                Name = "London",
                Country = "United Kingdom",
                State = "England",
                TouristRating = 5,
                DateEstablished = 1460,
                EstimatedPopulation = 1510000

            };


            City city4 = new City
            {
                Id = 4,
                Name = "Islamabath",
                Country = "Pakistan",
                State = "bihar",
                TouristRating = 2,
                DateEstablished = 1920,
                EstimatedPopulation = 1910000

            };


            List<City> cities = new List<City> { city1, city2, city3, city4 };

            var optionsBuilder = new DbContextOptionsBuilder<WeatherContext>();
            optionsBuilder.UseInMemoryDatabase("CreditSafe3");
            var _dbContext = new WeatherContext(optionsBuilder.Options);
            // Add sample data
            _dbContext.Cities.Add(city1);
            _dbContext.Cities.Add(city2);
            _dbContext.Cities.Add(city3);
            _dbContext.Cities.Add(city4);

            _dbContext.SaveChanges();

            Currency[] parisCurrencies = new Currency[]
            {
                            new Currency() { symbol = "€" ,code = "FC" ,name ="Franc"}
            };

            Currency[] singaporeCurrencies = new Currency[]
            {
                            new Currency() { symbol = "$" ,code = "SG" ,name ="Singapore Dollar"}
            };

            var mockRestCountryService = new Mock<IRestCountryService>();
            mockRestCountryService.Setup(m => m.GetCityInformation(It.Is<string>(y => ReferenceEquals("Paris", y)))).ReturnsAsync(new List<CityInfo>() { new CityInfo() { currencies = parisCurrencies } });
            mockRestCountryService.Setup(m => m.GetCityInformation(It.Is<string>(y => ReferenceEquals("Singapore", y)))).ReturnsAsync(new List<CityInfo>() { new CityInfo() { currencies = singaporeCurrencies } });

            var mockOpenWeatherService = new Mock<IOpenWeatherService>();
            mockOpenWeatherService.Setup(m => m.GetCityWeatherInformation(It.Is<string>(y => ReferenceEquals("Paris", y)))).ReturnsAsync(new CityWeatherInfo() { main = new Main() { temp = 286 } });
            mockOpenWeatherService.Setup(m => m.GetCityWeatherInformation(It.Is<string>(y => ReferenceEquals("Singapore", y)))).ReturnsAsync(new CityWeatherInfo() { main = new Main() { temp = 230 } });


            var countryService = new CountryService( mockOpenWeatherService.Object, mockRestCountryService.Object, _dbContext);

            // Act
            var allCities = countryService.GetAllCountriesAsync();

            // Assert

            Assert.AreEqual(0, allCities.Result.Count());
        }

        [Test]
        public async Task CountryService_Success_GetCityInformationAsync_ReturnCityInformation_UnitTest()
        {


            City city1 = new City
            {
                Name = "Paris",
                Country = "France",
                State = "Île-de-France",
                TouristRating = 5,
                DateEstablished = 1860,
                EstimatedPopulation = 780000

            };

            City city2 = new City
            {
                Name = "Singapore",
                Country = "Singapore",
                State = "Boonkeng",
                TouristRating = 4,
                DateEstablished = 1840,
                EstimatedPopulation = 550000

            };

            City city3 = new City
            {
                Id = 3,
                Name = "London",
                Country = "United Kingdom",
                State = "England",
                TouristRating = 5,
                DateEstablished = 1460,
                EstimatedPopulation = 1510000

            };


            City city4 = new City
            {
                Id = 4,
                Name = "Islamabath",
                Country = "Pakistan",
                State = "bihar",
                TouristRating = 2,
                DateEstablished = 1920,
                EstimatedPopulation = 1910000

            };

            List<City> cities = new List<City> { city1, city2, city3, city4 };
            Currency[] parisCurrencies = new Currency[]
            {
                new Currency() { symbol = "$" ,code = "FC" ,name ="Franc"}
            };

            Currency[] singaporeCurrencies = new Currency[]
            {
                 new Currency() { symbol = "$" ,code = "SG" ,name ="Franc"}
            };

            var mockRestCountryService = new Mock<IRestCountryService>();
            mockRestCountryService.Setup(m => m.GetCityInformation(It.Is<string>(y => ReferenceEquals("Paris", y)))).ReturnsAsync(new List<CityInfo>() { new CityInfo() { currencies = parisCurrencies } });
            mockRestCountryService.Setup(m => m.GetCityInformation(It.Is<string>(y => ReferenceEquals("Singapore", y)))).ReturnsAsync(new List<CityInfo>() { new CityInfo() { currencies = singaporeCurrencies } });

            var mockOpenWeatherService = new Mock<IOpenWeatherService>();
            mockOpenWeatherService.Setup(m => m.GetCityWeatherInformation(It.Is<string>(y => ReferenceEquals("Paris", y)))).ReturnsAsync(new CityWeatherInfo() { main = new Main() { temp = 286 } });
            mockOpenWeatherService.Setup(m => m.GetCityWeatherInformation(It.Is<string>(y => ReferenceEquals("Singapore", y)))).ReturnsAsync(new CityWeatherInfo() { main = new Main() { temp = 230 } });


            var optionsBuilder = new DbContextOptionsBuilder<WeatherContext>();
            optionsBuilder.UseInMemoryDatabase("CreditSafe4");
            var _dbContext = new WeatherContext(optionsBuilder.Options);
            // Add sample data
            _dbContext.Cities.Add(city1);
            _dbContext.Cities.Add(city2);
            _dbContext.Cities.Add(city3);
            _dbContext.Cities.Add(city4);
            _dbContext.SaveChanges();


            var countryService = new CountryService(mockOpenWeatherService.Object, mockRestCountryService.Object, _dbContext);

            // Act
            var cityInformation = await countryService.GetCityInformationAsync(2);

            Assert.AreEqual("Singapore", cityInformation.Name);
        }

        [Test]
        public async Task CountryService_Success_GetNonExistingCityInformationAsync_ReturnNull_UnitTest()
        {
            City city1 = new City
            {
                Id = 1,
                Name = "Canberra",
                Country = "Australia",
                State = "Southeastern",
                TouristRating = 5,
                DateEstablished = 1860,
                EstimatedPopulation = 780000

            };

            City city2 = new City
            {
                Id = 2,
                Name = "Wellington",
                Country = "	Zew Zealand",
                State = "Auckland",
                TouristRating = 4,
                DateEstablished = 1840,
                EstimatedPopulation = 550000

            };

            City city3 = new City
            {
                Id = 3,
                Name = "London",
                Country = "United Kingdom",
                State = "England",
                TouristRating = 5,
                DateEstablished = 1460,
                EstimatedPopulation = 1510000

            };


            City city4 = new City
            {
                Id = 4,
                Name = "Islamabath",
                Country = "Pakistan",
                State = "bihar",
                TouristRating = 2,
                DateEstablished = 1920,
                EstimatedPopulation = 1910000

            };

            List<City> cities = new List<City> { city1, city2, city3, city4 };
            Currency[] parisCurrencies = new Currency[]
            {
                new Currency() { symbol = "$" ,code = "FC" ,name ="Franc"}
            };

            Currency[] singaporeCurrencies = new Currency[]
            {
                 new Currency() { symbol = "$" ,code = "SG" ,name ="Franc"}
            };

            var mockRestCountryService = new Mock<IRestCountryService>();
            mockRestCountryService.Setup(m => m.GetCityInformation(It.Is<string>(y => ReferenceEquals("Paris", y)))).ReturnsAsync(new List<CityInfo>() { new CityInfo() { currencies = parisCurrencies } });
            mockRestCountryService.Setup(m => m.GetCityInformation(It.Is<string>(y => ReferenceEquals("Singapore", y)))).ReturnsAsync(new List<CityInfo>() { new CityInfo() { currencies = singaporeCurrencies } });

            var mockOpenWeatherService = new Mock<IOpenWeatherService>();
            mockOpenWeatherService.Setup(m => m.GetCityWeatherInformation(It.Is<string>(y => ReferenceEquals("Paris", y)))).ReturnsAsync(new CityWeatherInfo() { main = new Main() { temp = 286 } });
            mockOpenWeatherService.Setup(m => m.GetCityWeatherInformation(It.Is<string>(y => ReferenceEquals("Singapore", y)))).ReturnsAsync(new CityWeatherInfo() { main = new Main() { temp = 230 } });


            var optionsBuilder = new DbContextOptionsBuilder<WeatherContext>();
            optionsBuilder.UseInMemoryDatabase("CreditSafe5");
            var _dbContext = new WeatherContext(optionsBuilder.Options);
            // Add sample data
            _dbContext.Cities.Add(city1);
            _dbContext.Cities.Add(city2);
            _dbContext.Cities.Add(city3);
            _dbContext.Cities.Add(city4);
            _dbContext.SaveChanges();


            var countryService = new CountryService(mockOpenWeatherService.Object, mockRestCountryService.Object, _dbContext);

            // Act
            var cityInformation = await countryService.GetCityInformationAsync(4);

            Assert.AreEqual(null, cityInformation);
        }

        [Test]
        public async Task CountryService_Success_Post_ReturnCityWithId_UnitTest()
        {
            City city1 = new City
            {
                Id = 1,
                Name = "Canberra",
                Country = "Australia",
                State = "Southeastern",
                TouristRating = 5,
                DateEstablished = 1860,
                EstimatedPopulation = 780000

            };

            City city2 = new City
            {
                Id = 2,
                Name = "Wellington",
                Country = "	Zew Zealand",
                State = "Auckland",
                TouristRating = 4,
                DateEstablished = 1840,
                EstimatedPopulation = 550000

            };

            City city3 = new City
            {
                Id = 3,
                Name = "London",
                Country = "United Kingdom",
                State = "England",
                TouristRating = 5,
                DateEstablished = 1460,
                EstimatedPopulation = 1510000

            };

            City city4 = new City
            {
                Id = 4,
                Name = "Islamabath",
                Country = "Pakistan",
                State = "bihar",
                TouristRating = 2,
                DateEstablished = 1920,
                EstimatedPopulation = 1910000

            };

            List<City> cities = new List<City> { city1, city2, city3, city4 };
            Currency[] parisCurrencies = new Currency[]
            {
                new Currency() { symbol = "$" ,code = "FC" ,name ="Franc"}
            };

            Currency[] singaporeCurrencies = new Currency[]
            {
                 new Currency() { symbol = "$" ,code = "SG" ,name ="Franc"}
            };

            var mockRestCountryService = new Mock<IRestCountryService>();
            var mockOpenWeatherService = new Mock<IOpenWeatherService>();
           
            var optionsBuilder = new DbContextOptionsBuilder<WeatherContext>();
            optionsBuilder.UseInMemoryDatabase("CreditSafe6");
            var _dbContext = new WeatherContext(optionsBuilder.Options);
            // Add sample data
            _dbContext.Cities.Add(city1);
            _dbContext.Cities.Add(city2);
            _dbContext.Cities.Add(city3);
            _dbContext.Cities.Add(city4);
            _dbContext.SaveChanges();

            var countryService = new CountryService(mockOpenWeatherService.Object, mockRestCountryService.Object, _dbContext);

            City newCity = new City
            {
                Name = "Budapest",
                Country = "Hungary",
                State = "Central Hungary",
                TouristRating = 4,
                DateEstablished = 1820,
                EstimatedPopulation = 121000

            };

            // Act
            var newlyCreatedCity = await countryService.Post(newCity);

            Assert.AreEqual("Hungary", newlyCreatedCity.Country);
            Assert.IsNotNull(newlyCreatedCity.Id);
        }

        [Test]
        public async Task CountryService_Success_Update_ReturnCityWithModification_UnitTest()
        {
            City city1 = new City
            {
                Id = 1,
                Name = "Canberra",
                Country = "Australia",
                State = "Southeastern",
                TouristRating = 5,
                DateEstablished = 1860,
                EstimatedPopulation = 780000

            };

            City city2 = new City
            {
                Id = 2,
                Name = "Wellington",
                Country = "	Zew Zealand",
                State = "Auckland",
                TouristRating = 4,
                DateEstablished = 1840,
                EstimatedPopulation = 550000

            };

            City city3 = new City
            {
                Id = 3,
                Name = "London",
                Country = "United Kingdom",
                State = "England",
                TouristRating = 5,
                DateEstablished = 1460,
                EstimatedPopulation = 1510000

            };

            City city4 = new City
            {
                Id = 4,
                Name = "Islamabath",
                Country = "Pakistan",
                State = "bihar",
                TouristRating = 2,
                DateEstablished = 1920,
                EstimatedPopulation = 1910000

            };

            List<City> cities = new List<City> { city1, city2, city3, city4 };
            Currency[] parisCurrencies = new Currency[]
            {
                new Currency() { symbol = "$" ,code = "FC" ,name ="Franc"}
            };

            Currency[] singaporeCurrencies = new Currency[]
            {
                 new Currency() { symbol = "$" ,code = "SG" ,name ="Franc"}
            };

            var mockRestCountryService = new Mock<IRestCountryService>();
            var mockOpenWeatherService = new Mock<IOpenWeatherService>();

            var optionsBuilder = new DbContextOptionsBuilder<WeatherContext>();
            optionsBuilder.UseInMemoryDatabase("CreditSafe7");
            var _dbContext = new WeatherContext(optionsBuilder.Options);
            // Add sample data
            _dbContext.Cities.Add(city1);
            _dbContext.Cities.Add(city2);
            _dbContext.Cities.Add(city3);
            _dbContext.Cities.Add(city4);
            _dbContext.SaveChanges();

            var countryService = new CountryService(mockOpenWeatherService.Object, mockRestCountryService.Object, _dbContext);

            City modifiedCity = new City
            {
                Id = 1,
                Name = "Canberra",
                Country = "Australia",
                State = "Southeastern",
                TouristRating = 4,
                DateEstablished = 1760,
                EstimatedPopulation = 680000

            };

            // Act
            var newlyModifiedCity = await countryService.Update(modifiedCity.Id, modifiedCity);

            Assert.AreEqual(4, newlyModifiedCity.TouristRating);
            Assert.AreEqual(1760, newlyModifiedCity.DateEstablished);
            Assert.AreEqual(680000, newlyModifiedCity.EstimatedPopulation);
            Assert.AreEqual(1, newlyModifiedCity.Id);

        }

        [Test]
        public async Task CountryService_Success_Update_Input_MissMatchingIds_ReturnNull_UnitTest()
        {
            City city1 = new City
            {
                Id = 1,
                Name = "Canberra",
                Country = "Australia",
                State = "Southeastern",
                TouristRating = 5,
                DateEstablished = 1860,
                EstimatedPopulation = 780000

            };

            City city2 = new City
            {
                Id = 2,
                Name = "Wellington",
                Country = "	Zew Zealand",
                State = "Auckland",
                TouristRating = 4,
                DateEstablished = 1840,
                EstimatedPopulation = 550000

            };

            City city3 = new City
            {
                Id = 3,
                Name = "London",
                Country = "United Kingdom",
                State = "England",
                TouristRating = 5,
                DateEstablished = 1460,
                EstimatedPopulation = 1510000

            };

            City city4 = new City
            {
                Id = 4,
                Name = "Islamabath",
                Country = "Pakistan",
                State = "bihar",
                TouristRating = 2,
                DateEstablished = 1920,
                EstimatedPopulation = 1910000

            };

            List<City> cities = new List<City> { city1, city2, city3, city4 };
            Currency[] parisCurrencies = new Currency[]
            {
                new Currency() { symbol = "$" ,code = "FC" ,name ="Franc"}
            };

            Currency[] singaporeCurrencies = new Currency[]
            {
                 new Currency() { symbol = "$" ,code = "SG" ,name ="Franc"}
            };

            var mockRestCountryService = new Mock<IRestCountryService>();
            var mockOpenWeatherService = new Mock<IOpenWeatherService>();

            var optionsBuilder = new DbContextOptionsBuilder<WeatherContext>();
            optionsBuilder.UseInMemoryDatabase("CreditSafe8");
            var _dbContext = new WeatherContext(optionsBuilder.Options);
            // Add sample data
            _dbContext.Cities.Add(city1);
            _dbContext.Cities.Add(city2);
            _dbContext.Cities.Add(city3);
            _dbContext.Cities.Add(city4);
            _dbContext.SaveChanges();

            var countryService = new CountryService(mockOpenWeatherService.Object, mockRestCountryService.Object, _dbContext);

            City modifiedCity = new City
            {
                Id = 1,
                Name = "Canberra",
                Country = "Australia",
                State = "Southeastern",
                TouristRating = 4,
                DateEstablished = 1760,
                EstimatedPopulation = 680000

            };

            // Act
            var newlyModifiedCity = await countryService.Update(2, modifiedCity);

            Assert.IsNull(newlyModifiedCity);
        }

        [Test]
        public async Task CountryService_Success_Update_Input_NotExistingId_ReturnNull_UnitTest()
        {
            City city1 = new City
            {
                Id = 1,
                Name = "Canberra",
                Country = "Australia",
                State = "Southeastern",
                TouristRating = 5,
                DateEstablished = 1860,
                EstimatedPopulation = 780000

            };

            City city2 = new City
            {
                Id = 2,
                Name = "Wellington",
                Country = "	Zew Zealand",
                State = "Auckland",
                TouristRating = 4,
                DateEstablished = 1840,
                EstimatedPopulation = 550000

            };

            City city3 = new City
            {
                Id = 3,
                Name = "London",
                Country = "United Kingdom",
                State = "England",
                TouristRating = 5,
                DateEstablished = 1460,
                EstimatedPopulation = 1510000

            };

            City city4 = new City
            {
                Id = 4,
                Name = "Islamabath",
                Country = "Pakistan",
                State = "bihar",
                TouristRating = 2,
                DateEstablished = 1920,
                EstimatedPopulation = 1910000

            };

            List<City> cities = new List<City> { city1, city2, city3, city4 };
            Currency[] parisCurrencies = new Currency[]
            {
                new Currency() { symbol = "$" ,code = "FC" ,name ="Franc"}
            };

            Currency[] singaporeCurrencies = new Currency[]
            {
                 new Currency() { symbol = "$" ,code = "SG" ,name ="Franc"}
            };

            var mockRestCountryService = new Mock<IRestCountryService>();
            var mockOpenWeatherService = new Mock<IOpenWeatherService>();

            var optionsBuilder = new DbContextOptionsBuilder<WeatherContext>();
            optionsBuilder.UseInMemoryDatabase("CreditSafe9");
            var _dbContext = new WeatherContext(optionsBuilder.Options);
            // Add sample data
            _dbContext.Cities.Add(city1);
            _dbContext.Cities.Add(city2);
            _dbContext.Cities.Add(city3);
            _dbContext.Cities.Add(city4);
            _dbContext.SaveChanges();

            var countryService = new CountryService(mockOpenWeatherService.Object, mockRestCountryService.Object, _dbContext);

            City modifiedCity = new City
            {
                Id = 5,
                Name = "Canberra",
                Country = "Australia",
                State = "Southeastern",
                TouristRating = 4,
                DateEstablished = 1760,
                EstimatedPopulation = 680000

            };

            // Act
            var newlyModifiedCity = await countryService.Update(modifiedCity.Id, modifiedCity);

            Assert.IsNull(newlyModifiedCity);
        }

        [Test]
        public async Task CountryService_Success_Delete_Input_ExistingId_ReturnTrue_UnitTest()
        {
            City city1 = new City
            {
                Id = 1,
                Name = "Canberra",
                Country = "Australia",
                State = "Southeastern",
                TouristRating = 5,
                DateEstablished = 1860,
                EstimatedPopulation = 780000

            };

            City city2 = new City
            {
                Id = 2,
                Name = "Wellington",
                Country = "	Zew Zealand",
                State = "Auckland",
                TouristRating = 4,
                DateEstablished = 1840,
                EstimatedPopulation = 550000

            };

            City city3 = new City
            {
                Id = 3,
                Name = "London",
                Country = "United Kingdom",
                State = "England",
                TouristRating = 5,
                DateEstablished = 1460,
                EstimatedPopulation = 1510000

            };

            City city4 = new City
            {
                Id = 4,
                Name = "Islamabath",
                Country = "Pakistan",
                State = "bihar",
                TouristRating = 2,
                DateEstablished = 1920,
                EstimatedPopulation = 1910000

            };

            List<City> cities = new List<City> { city1, city2, city3, city4 };
            Currency[] parisCurrencies = new Currency[]
            {
                new Currency() { symbol = "$" ,code = "FC" ,name ="Franc"}
            };

            Currency[] singaporeCurrencies = new Currency[]
            {
                 new Currency() { symbol = "$" ,code = "SG" ,name ="Franc"}
            };

            var mockRestCountryService = new Mock<IRestCountryService>();
            var mockOpenWeatherService = new Mock<IOpenWeatherService>();

            var optionsBuilder = new DbContextOptionsBuilder<WeatherContext>();
            optionsBuilder.UseInMemoryDatabase("CreditSafe10");
            var _dbContext = new WeatherContext(optionsBuilder.Options);
            // Add sample data
            _dbContext.Cities.Add(city1);
            _dbContext.Cities.Add(city2);
            _dbContext.Cities.Add(city3);
            _dbContext.Cities.Add(city4);
            _dbContext.SaveChanges();

            var countryService = new CountryService(mockOpenWeatherService.Object, mockRestCountryService.Object, _dbContext);

            City deletingCity = new City
            {
                Id = 1,
                Name = "Canberra",
                Country = "Australia",
                State = "Southeastern",
                TouristRating = 5,
                DateEstablished = 1860,
                EstimatedPopulation = 780000

            };

            // Act
            var deletedCityResponse = await countryService.Delete(deletingCity.Id);

            Assert.IsTrue(deletedCityResponse);
        }

        [Test]
        public async Task CountryService_Success_Delete_Input_NotExistingId_ReturnTrue_UnitTest()
        {
            City city1 = new City
            {
                Id = 1,
                Name = "Canberra",
                Country = "Australia",
                State = "Southeastern",
                TouristRating = 5,
                DateEstablished = 1860,
                EstimatedPopulation = 780000

            };

            City city2 = new City
            {
                Id = 2,
                Name = "Wellington",
                Country = "	Zew Zealand",
                State = "Auckland",
                TouristRating = 4,
                DateEstablished = 1840,
                EstimatedPopulation = 550000

            };

            City city3 = new City
            {
                Id = 3,
                Name = "London",
                Country = "United Kingdom",
                State = "England",
                TouristRating = 5,
                DateEstablished = 1460,
                EstimatedPopulation = 1510000

            };

            City city4 = new City
            {
                Id = 4,
                Name = "Islamabath",
                Country = "Pakistan",
                State = "bihar",
                TouristRating = 2,
                DateEstablished = 1920,
                EstimatedPopulation = 1910000

            };

            List<City> cities = new List<City> { city1, city2, city3, city4 };
            Currency[] parisCurrencies = new Currency[]
            {
                new Currency() { symbol = "$" ,code = "FC" ,name ="Franc"}
            };

            Currency[] singaporeCurrencies = new Currency[]
            {
                 new Currency() { symbol = "$" ,code = "SG" ,name ="Franc"}
            };

            var mockRestCountryService = new Mock<IRestCountryService>();
            var mockOpenWeatherService = new Mock<IOpenWeatherService>();

            var optionsBuilder = new DbContextOptionsBuilder<WeatherContext>();
            optionsBuilder.UseInMemoryDatabase("CreditSafe11");
            var _dbContext = new WeatherContext(optionsBuilder.Options);
            // Add sample data
            _dbContext.Cities.Add(city1);
            _dbContext.Cities.Add(city2);
            _dbContext.Cities.Add(city3);
            _dbContext.Cities.Add(city4);
            _dbContext.SaveChanges();

            var countryService = new CountryService(mockOpenWeatherService.Object, mockRestCountryService.Object, _dbContext);

            City deletingCity = new City
            {
                Id = 5,
                Name = "Melbourn",
                Country = "Australia",
                State = "South",
                TouristRating = 5,
                DateEstablished = 1860,
                EstimatedPopulation = 780000

            };

            // Act
            var deletedCityResponse = await countryService.Delete(deletingCity.Id);

            Assert.IsFalse(deletedCityResponse);
        }

    }
}
