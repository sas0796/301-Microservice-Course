using Microsoft.AspNetCore.Mvc;
using Moq;
using MT.OnlineRestaurant.BusinessEntities;
using MT.OnlineRestaurant.BusinessLayer;
using MT.OnlineRestaurant.SearchManagement.Controllers;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace MT.OnlineRestaurant.UT
{
    [TestFixture]
    public class SearchRepositoryTest
    {
        //[Test]
        //public void Test_GetResturantDetails()
        //{
        //    int restaurantID = 10;

        //    var options = new DbContextOptionsBuilder<RestaurantManagementContext>()
        //    .UseInMemoryDatabase(databaseName: "RestaurantManagement")
        //    .Options;

        //    RestaurantManagementContext managementContext = new RestaurantManagementContext(options);
        //    TblRestaurant rest = new TblRestaurant
        //    {
        //         Id = 10,
        //         Name = "ABC",
        //         Address = "6th Street",
        //         ContactNo = "9025619584",
        //         CloseTime = "9:00PM",
        //         OpeningTime = "10:00PM",
        //         TblLocationId = 1
        //    };
        //    managementContext.TblRestaurant.Add(rest);

        //    TblLocation location = new TblLocation
        //    {
        //        Id = 1,
        //        X = (decimal)40.987987,
        //        Y = (decimal)36.987656
        //    };
        //    managementContext.TblLocation.Add(location);

        //    SearchRepository _repository = new SearchRepository(managementContext);
        //    var restaurantDetails = _repository.GetResturantDetails(restaurantID);

        //    Assert.NotNull(restaurantDetails);

        //}

        //[Test]
        //public void Test_GetTableDetails()
        //{
        //    int restaurantID = 11;

        //    var options = new DbContextOptionsBuilder<RestaurantManagementContext>()
        //    .UseInMemoryDatabase(databaseName: "RestaurantManagement")
        //    .Options;

        //    SearchRepository _repository = new SearchRepository(new RestaurantManagementContext(options));
        //}

        [Test]
        public void GetRestaurantRating()
        {
            //Arrange
            List<RestaurantRating> restaurantRatings = new List<RestaurantRating>();
            restaurantRatings.Add(new RestaurantRating()
            {
                RestaurantId = 1,
                customerId = 1,
                rating = "2",
                user_Comments = "",
            });
            var mockOrder = new Mock<IRestaurantBusiness>();
            mockOrder.Setup(x => x.GetRestaurantRating(1)).Returns(restaurantRatings.AsQueryable());

            //Act
            var searchController = new SearchController(mockOrder.Object);
            var data = searchController.GetResturantRating(1);
            var okObjectResult = data as OkObjectResult;

            //Assert
            Assert.AreEqual(200, okObjectResult.StatusCode);
            Assert.IsNotNull(okObjectResult);
            Assert.AreEqual((okObjectResult.Value as IEnumerable<RestaurantRating>).Count(), restaurantRatings.Count());
        }

        [Test]
        public void GetRestaurantDetailsBasedOnRating()
        {
            ////Arrange
            //List<RestaurantRating> restaurantRatings = new List<RestaurantRating>();
            //restaurantRatings.Add(new RestaurantRating()
            //{
            //    RestaurantId = 1,
            //    customerId = 1,
            //    rating = "2",
            //    user_Comments = "",
            //});
            //var mockOrder = new Mock<IRestaurantBusiness>();
            //mockOrder.Setup(x => x.GetRestaurantRating(1)).Returns(restaurantRatings.AsQueryable());

            ////Act
            //var searchController = new SearchController(mockOrder.Object);
            //var data = searchController.GetResturantRating(1);
            //var okObjectResult = data as OkObjectResult;

            ////Assert
            //Assert.AreEqual(200, okObjectResult.StatusCode);
            //Assert.IsNotNull(okObjectResult);
            //Assert.AreEqual((okObjectResult.Value as IEnumerable<RestaurantRating>).Count(), restaurantRatings.Count());
        }

    }
}
