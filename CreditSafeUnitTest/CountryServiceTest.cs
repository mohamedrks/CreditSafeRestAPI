using Castle.Core.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace JustEatUnitTest
{
    [TestFixture]
    class RestaurentServiceTest
    {
        [SetUp]
        public void Setup()
        {
            
        }

        [Test]
        public void RestaurentService_Success_ReturnOpenedRestaurents_UnitTest()
        {
            Cuisine cuisine1 = new Cuisine
            {

                Name = "Pizza",
                SeoName = "Pizza seo"
            };

            Cuisine cuisine2 = new Cuisine
            {

                Name = "Pizza",
                SeoName = "Pizza seo"
            };

            Cuisine[] cuisines = new Cuisine[] { cuisine1, cuisine2 };

            Rating rating1 = new Rating
            {
                Average = 4.6,
                Count = 5600,
                StarRating = 4.2
            };

            Restaurant restaurant1 = new Restaurant()
            {

                Cuisines = cuisines,
                Rating = rating1,
                Name = "bublugy",
                IsOpenNow = true
            };


            Restaurant restaurant3 = new Restaurant()
            {

                Cuisines = cuisines,
                Rating = rating1,
                Name = "Macdonalds",
                IsOpenNow = false
            };

            Restaurant[] restaurantArray = new Restaurant[] {

                restaurant1,
                restaurant2,
                restaurant3
            };

            JustEatAPI justEatAPI = new JustEatAPI()
            {
                Restaurants = restaurantArray
            };


            var json = JsonSerializer.Serialize(justEatAPI);

            var httpClientFactoryMock = new Mock<IHttpClientFactory>();
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(json.ToString())

                });



            var client = new HttpClient(mockHttpMessageHandler.Object);
            httpClientFactoryMock.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(client);

            // Mock configuration.
            Mock<IConfiguration> mockConfiguration = new Mock<IConfiguration>();
            mockConfiguration.SetupGet(x => x[It.IsAny<string>()]).Returns("https://uk.api.just-eat.io/");

            // Mock Logger.
            var loggerMock = new Mock<ILogger<CountryService>>();


            var restaurantService = new RestaurentService(httpClientFactoryMock.Object, loggerMock.Object, mockConfiguration.Object);


            // Act
            var openedRestaurants = restaurantService.GetRestaurentsByPostCodeAsync("restaurants/byPostCode/" + "ec4m");

            // Assert
            loggerMock.Verify(
                x => x.Log(
                    It.Is<LogLevel>(l => l == LogLevel.Information),
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString() == "Call to Just Eat was success."),
                    It.IsAny<Exception>(),
                    It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)));

            Assert.AreEqual(1, openedRestaurants.Result.Count);
        }

        //[Test]
        //public void RestaurentService_Success_ReturnOpenedRestaurents_IsEmpty_UnitTest()
        //{
        //    Cuisine cuisine1 = new Cuisine
        //    {

        //        Name = "Pizza",
        //        SeoName = "Pizza seo"
        //    };

        //    Cuisine cuisine2 = new Cuisine
        //    {

        //        Name = "Pizza",
        //        SeoName = "Pizza seo"
        //    };

        //    Cuisine[] cuisines = new Cuisine[] { cuisine1, cuisine2 };

        //    Rating rating1 = new Rating
        //    {
        //        Average = 4.6,
        //        Count = 5600,
        //        StarRating = 4.2
        //    };

        //    Restaurant restaurant1 = new Restaurant()
        //    {

        //        Cuisines = cuisines,
        //        Rating = rating1,
        //        Name = "bublugy",
        //        IsOpenNow = false
        //    };

        //    Restaurant restaurant2 = new Restaurant()
        //    {

        //        Cuisines = cuisines,
        //        Rating = rating1,
        //        Name = "KFC",
        //        IsOpenNow = false
        //    };

        //    Restaurant restaurant3 = new Restaurant()
        //    {

        //        Cuisines = cuisines,
        //        Rating = rating1,
        //        Name = "Macdonalds",
        //        IsOpenNow = false
        //    };

        //    Restaurant[] restaurantArray = new Restaurant[] {

        //        restaurant1,
        //        restaurant2,
        //        restaurant3
        //    };

        //    JustEatAPI justEatAPI = new JustEatAPI()
        //    {
        //        Restaurants = restaurantArray
        //    };


        //    var json = JsonSerializer.Serialize(justEatAPI);

        //    var httpClientFactoryMock = new Mock<IHttpClientFactory>();
        //    var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
        //    mockHttpMessageHandler.Protected()
        //        .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
        //        .ReturnsAsync(new HttpResponseMessage
        //        {
        //            StatusCode = HttpStatusCode.OK,
        //            Content = new StringContent(json.ToString())

        //        });



        //    var client = new HttpClient(mockHttpMessageHandler.Object);
        //    httpClientFactoryMock.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(client);

        //    // Mock configuration.
        //    Mock<IConfiguration> mockConfiguration = new Mock<IConfiguration>();
        //    mockConfiguration.SetupGet(x => x[It.IsAny<string>()]).Returns("https://uk.api.just-eat.io/");

        //    // Mock Logger.
        //    var loggerMock = new Mock<ILogger<RestaurentService>>();


        //    var restaurantService = new RestaurentService(httpClientFactoryMock.Object, loggerMock.Object, mockConfiguration.Object);


        //    // Act
        //    var openedRestaurants = restaurantService.GetRestaurentsByPostCodeAsync("restaurants/byPostCode/" + "ec4m");

        //    // Assert
        //    loggerMock.Verify(
        //        x => x.Log(
        //            It.Is<LogLevel>(l => l == LogLevel.Information),
        //            It.IsAny<EventId>(),
        //            It.Is<It.IsAnyType>((v, t) => v.ToString() == "Call to Just Eat was success."),
        //            It.IsAny<Exception>(),
        //            It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)));

        //    Assert.AreEqual(0, openedRestaurants.Result.Count);
        //}

        //[Test]
        //public void RestaurentService_Success_LogMessage_UnitTest()
        //{
        //    Cuisine cuisine1 = new Cuisine
        //    {

        //        Name = "Pizza",
        //        SeoName = "Pizza seo"
        //    };

        //    Cuisine cuisine2 = new Cuisine
        //    {

        //        Name = "Pizza",
        //        SeoName = "Pizza seo"
        //    };

        //    Cuisine[] cuisines = new Cuisine[] { cuisine1, cuisine2 };

        //    Rating rating1 = new Rating
        //    {
        //        Average = 4.6,
        //        Count = 5600,
        //        StarRating = 4.2
        //    };

        //    Restaurant restaurant1 = new Restaurant()
        //    {

        //        Cuisines = cuisines,
        //        Rating = rating1,
        //        Name = "bublugy",
        //        IsOpenNow = true
        //    };

        //    Restaurant restaurant2 = new Restaurant()
        //    {

        //        Cuisines = cuisines,
        //        Rating = rating1,
        //        Name = "KFC",
        //        IsOpenNow = false
        //    };

        //    Restaurant restaurant3 = new Restaurant()
        //    {

        //        Cuisines = cuisines,
        //        Rating = rating1,
        //        Name = "Macdonalds",
        //        IsOpenNow = false
        //    };

        //    Restaurant[] restaurantArray = new Restaurant[] {

        //        restaurant1,
        //        restaurant2,
        //        restaurant3
        //    };

        //    JustEatAPI justEatAPI = new JustEatAPI()
        //    {
        //        Restaurants = restaurantArray
        //    };


        //    var json = JsonSerializer.Serialize(justEatAPI);

        //    var httpClientFactoryMock = new Mock<IHttpClientFactory>();
        //    var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
        //    mockHttpMessageHandler.Protected()
        //        .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
        //        .ReturnsAsync(new HttpResponseMessage
        //        {
        //            StatusCode = HttpStatusCode.OK,
        //            Content = new StringContent(json.ToString())
     
        //        });



        //    var client = new HttpClient(mockHttpMessageHandler.Object);
        //    httpClientFactoryMock.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(client);

        //    // Mock configuration.
        //    Mock<IConfiguration> mockConfiguration = new Mock<IConfiguration>();
        //    mockConfiguration.SetupGet(x => x[It.IsAny<string>()]).Returns("https://uk.api.just-eat.io/");

        //    // Mock Logger.
        //    var loggerMock = new Mock<ILogger<RestaurentService>>();


        //    var restaurantService = new RestaurentService(httpClientFactoryMock.Object, loggerMock.Object, mockConfiguration.Object);


        //    // Act
        //    var openedRestaurants = restaurantService.GetRestaurentsByPostCodeAsync("restaurants/byPostCode/" + "ec4m");

        //    // Assert
        //    loggerMock.Verify(
        //        x => x.Log(
        //            It.Is<LogLevel>(l => l == LogLevel.Information),
        //            It.IsAny<EventId>(),
        //            It.Is<It.IsAnyType>((v, t) => v.ToString() == "Call to Just Eat was success."),
        //            It.IsAny<Exception>(),
        //            It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)));
        //}


        //[Test]
        //public void RestaurentService_Error_LogMessage_UnitTest()
        //{
   
        //    var httpClientFactoryMock = new Mock<IHttpClientFactory>();
        //    var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
        //    mockHttpMessageHandler.Protected()
        //        .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
        //        .ReturnsAsync(new HttpResponseMessage
        //        {
        //            StatusCode = HttpStatusCode.InternalServerError,
        //            Content = new StringContent("")

        //        });



        //    var client = new HttpClient(mockHttpMessageHandler.Object);
        //    httpClientFactoryMock.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(client);

        //    // Mock configuration.
        //    Mock<IConfiguration> mockConfiguration = new Mock<IConfiguration>();
        //    mockConfiguration.SetupGet(x => x[It.IsAny<string>()]).Returns("https://uk.api.just-eat.io/");

        //    // Mock Logger.
        //    var loggerMock = new Mock<ILogger<RestaurentService>>();


        //    var restaurantService = new RestaurentService(httpClientFactoryMock.Object, loggerMock.Object, mockConfiguration.Object);


        //    // Act
        //    var openedRestaurants = restaurantService.GetRestaurentsByPostCodeAsync("restaurants/byPostCode/" + "ec4m");

        //    // Assert
        //    loggerMock.Verify(
        //        x => x.Log(
        //            It.Is<LogLevel>(l => l == LogLevel.Error),
        //            It.IsAny<EventId>(),
        //            It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Error : ")),
        //            It.IsAny<Exception>(),
        //            It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)));
        //}
    }
}
