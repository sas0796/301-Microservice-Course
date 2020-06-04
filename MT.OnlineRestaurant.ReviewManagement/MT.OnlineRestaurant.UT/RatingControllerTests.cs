using Microsoft.AspNetCore.Mvc;
using Moq;
using MT.OnlineRestaurant.BusinessEntities;
using MT.OnlineRestaurant.BusinessLayer;
using MT.OnlineRestaurant.DataLayer.Repository;
using MT.OnlineRestaurant.ReviewManagement.Controllers;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

namespace MT.OnlineRestaurant.UT
{
    public class Tests
    {
        //Get Ratings Test Cases
        [Test]
        public void GetRestaurantRatings_CorrectOutput()
        {
            //Arrange
            List<RestaurantRating> restauratRatings = new List<RestaurantRating>();
            restauratRatings.Add(new RestaurantRating()
            {
                RestaurantId = 1,
                RatingId = 1,
                rating = "9",
                user_Comments = "Good place to eat"
            });
            var mockRatingBusiness = new Mock<IRatingBusiness>();
            mockRatingBusiness.Setup(x => x.GetRestaurantRatings(1)).Returns(restauratRatings);

            //Act
            var ratingController = new RatingController(mockRatingBusiness.Object);
            var data = ratingController.GetResturantRating(1);
            var okObjectResult = data as ObjectResult;

            //Assert
            Assert.AreEqual(200, okObjectResult.StatusCode);
            Assert.IsNotNull(okObjectResult);
            Assert.AreEqual((okObjectResult.Value as IEnumerable<RestaurantRating>).Count(), restauratRatings.Count);
        }
        [Test]
        public void GetRestaurantRatings_NoOutput()
        {
            //Arrange
            string result = "No ratings found for Restaurant Id : 1000 ! Try again with other Restaurant Id";
            List<RestaurantRating> restauratRatings = new List<RestaurantRating>();
            var mockRatingBusiness = new Mock<IRatingBusiness>();
            mockRatingBusiness.Setup(x => x.GetRestaurantRatings(1)).Returns(restauratRatings);

            //Act
            var ratingController = new RatingController(mockRatingBusiness.Object);
            var data = ratingController.GetResturantRating(1000);
            var okObjectResult = data as ObjectResult;

            //Assert
            Assert.AreEqual(400, okObjectResult.StatusCode);
            Assert.IsNotNull(okObjectResult);
            Assert.AreEqual(okObjectResult.Value, result);
        }
        [Test]
        public void GetRestaurantRatings_WrongInput_LessThanOne()
        {
            //Arrange
            Exception exception= new Exception("Restaurant Id cannot be less than one! Try again with input for Restaurant Id greater than or equals one");
            var mockRatingBusiness = new Mock<IRatingBusiness>();
            mockRatingBusiness.Setup(x => x.GetRestaurantRatings(0)).Throws(exception);

            //Act
            var ratingController = new RatingController(mockRatingBusiness.Object);
            var data = ratingController.GetResturantRating(0);
            var okObjectResult = data as ObjectResult;

            //Assert
            Assert.AreEqual(400, okObjectResult.StatusCode);
            Assert.IsNotNull(okObjectResult);
            Assert.AreEqual(okObjectResult.Value, exception.Message);
        }
        [Test]
        public void GetRestaurantRatings_UnexpectedException()
        {
            //Arrange
            Exception exception = new Exception("Error in getting restaurant for Id : 100! Try again after some time");
            var mockRatingBusiness = new Mock<IRatingBusiness>();
            mockRatingBusiness.Setup(x => x.GetRestaurantRatings(100)).Throws(exception);

            //Act
            var ratingController = new RatingController(mockRatingBusiness.Object);
            var data = ratingController.GetResturantRating(100);
            var okObjectResult = data as ObjectResult;

            //Assert
            Assert.AreEqual(500, okObjectResult.StatusCode);
            Assert.IsNotNull(okObjectResult);
            Assert.AreEqual(okObjectResult.Value, exception.Message);
        }
        // Add Rating Test Cases
        [Test]
        public void AddRestaurantRating_CorrectOutput()
        {
            //Arrange
            RestaurantRating restauratRatings = new RestaurantRating()
            {
                RestaurantId = 1,
                RatingId = 0,
                rating = "9",
                user_Comments = "Good place to eat"
            };
            var mockRatingBusiness = new Mock<IRatingBusiness>();
            mockRatingBusiness.Setup(x => x.AddRestaurantRating(restauratRatings, out restauratRatings)).Returns(true);

            //Act
            var ratingController = new RatingController(mockRatingBusiness.Object);
            var data = ratingController.AddRestaurantRating(restauratRatings);
            var okObjectResult = data as ObjectResult;

            //Assert
            Assert.AreEqual(201, okObjectResult.StatusCode);
            Assert.IsNotNull(okObjectResult);
            Assert.AreEqual((okObjectResult.Value as RestaurantRating), restauratRatings);
        }
        [Test]
        public void AddRestaurantRating_WrongInput_RestaurantIdLessThanOne()
        {
            //Arrange
            RestaurantRating restauratRatings = new RestaurantRating()
            {
                RestaurantId = 0,
                RatingId = 0,
                rating = "9",
                user_Comments = "Good place to eat"
            };
            Exception exception = new Exception("Entity Error : Restaurant Id cannot be less than one");
            var mockRatingBusiness = new Mock<IRatingBusiness>();
            mockRatingBusiness.Setup(x => x.AddRestaurantRating(restauratRatings, out restauratRatings)).Throws(exception);

            //Act
            var ratingController = new RatingController(mockRatingBusiness.Object);
            var data = ratingController.AddRestaurantRating(restauratRatings);
            var okObjectResult = data as ObjectResult;

            //Assert
            Assert.AreEqual(400, okObjectResult.StatusCode);
            Assert.IsNotNull(okObjectResult);
            Assert.AreEqual(okObjectResult.Value, exception.Message);
        }
        [Test]
        public void AddRestaurantRating_WrongInput_RestaurantIdDoesntExist()
        {
            //Arrange
            RestaurantRating restauratRatings = new RestaurantRating()
            {
                RestaurantId = 100,
                RatingId = 0,
                rating = "9",
                user_Comments = "Good place to eat"
            };
            Exception exception = new Exception("Restaurant doesn't exist! Try sending a valid Restaurant Id");
            var mockRatingBusiness = new Mock<IRatingBusiness>();
            mockRatingBusiness.Setup(x => x.AddRestaurantRating(restauratRatings, out restauratRatings)).Throws(exception);

            //Act
            var ratingController = new RatingController(mockRatingBusiness.Object);
            var data = ratingController.AddRestaurantRating(restauratRatings);
            var okObjectResult = data as ObjectResult;

            //Assert
            Assert.AreEqual(400, okObjectResult.StatusCode);
            Assert.IsNotNull(okObjectResult);
            Assert.AreEqual(okObjectResult.Value, exception.Message);
        }
        [Test]
        public void AddRestaurantRating_WrongInput_RatingIsEmpty()
        {
            //Arrange
            RestaurantRating restauratRatings = new RestaurantRating()
            {
                RestaurantId = 100,
                RatingId = 0,
                rating = string.Empty,
                user_Comments = "Good place to eat"
            };
            Exception exception = new Exception("Entity Error : Rating cannot be empty");
            var mockRatingBusiness = new Mock<IRatingBusiness>();
            mockRatingBusiness.Setup(x => x.AddRestaurantRating(restauratRatings, out restauratRatings)).Throws(exception);

            //Act
            var ratingController = new RatingController(mockRatingBusiness.Object);
            var data = ratingController.AddRestaurantRating(restauratRatings);
            var okObjectResult = data as ObjectResult;

            //Assert
            Assert.AreEqual(400, okObjectResult.StatusCode);
            Assert.IsNotNull(okObjectResult);
            Assert.AreEqual(okObjectResult.Value, exception.Message);
        }
        [Test]
        public void AddRestaurantRating_ReturnedFalse()
        {
            //Arrange
            string result = "Error in creating restaurant! Try again after some time.";
            RestaurantRating restauratRatings = new RestaurantRating()
            {
                RestaurantId = 1,
                RatingId = 0,
                rating = "9",
                user_Comments = "Good place to eat"
            };
            var mockRatingBusiness = new Mock<IRatingBusiness>();
            mockRatingBusiness.Setup(x => x.AddRestaurantRating(restauratRatings, out restauratRatings)).Returns(false);

            //Act
            var ratingController = new RatingController(mockRatingBusiness.Object);
            var data = ratingController.AddRestaurantRating(restauratRatings);
            var okObjectResult = data as ObjectResult;

            //Assert
            Assert.AreEqual(500, okObjectResult.StatusCode);
            Assert.IsNotNull(okObjectResult);
            Assert.AreEqual(okObjectResult.Value, result);
        }
        [Test]
        public void AddRestaurantRating_UnexpectedException()
        {
            //Arrange
            RestaurantRating restauratRatings = new RestaurantRating()
            {
                RestaurantId = 100,
                RatingId = 0,
                rating = string.Empty,
                user_Comments = "Good place to eat"
            };
            Exception exception = new Exception("Error in creating restaurant! Try again after some time.");
            var mockRatingBusiness = new Mock<IRatingBusiness>();
            mockRatingBusiness.Setup(x => x.AddRestaurantRating(restauratRatings, out restauratRatings)).Throws(exception);

            //Act
            var ratingController = new RatingController(mockRatingBusiness.Object);
            var data = ratingController.AddRestaurantRating(restauratRatings);
            var okObjectResult = data as ObjectResult;

            //Assert
            Assert.AreEqual(500, okObjectResult.StatusCode);
            Assert.IsNotNull(okObjectResult);
            Assert.AreEqual(okObjectResult.Value, exception.Message);
        }
        // Update Rating Test Cases
        [Test]
        public void UpdateRestaurantRating_CorrectOutput()
        {
            //Arrange
            int ratingId = 1;
            RestaurantRating restauratRatings = new RestaurantRating()
            {
                RestaurantId = 1,
                RatingId = 1,
                rating = "9",
                user_Comments = "Good place to eat"
            };
            var mockRatingBusiness = new Mock<IRatingBusiness>();
            mockRatingBusiness.Setup(x => x.UpdateRestaurantRating(ratingId, restauratRatings)).Returns(true);

            //Act
            var ratingController = new RatingController(mockRatingBusiness.Object);
            var data = ratingController.UpdateRestaurantRating(ratingId, restauratRatings);
            var okObjectResult = data as ObjectResult;

            //Assert
            Assert.AreEqual(202, okObjectResult.StatusCode);
            Assert.IsNotNull(okObjectResult);
        }
        [Test]
        public void UpdateRestaurantRating_WrongInput_RestaurantIdLessThanOne()
        {
            //Arrange
            int ratingId = 1;
            RestaurantRating restauratRatings = new RestaurantRating()
            {
                RestaurantId = 0,
                RatingId = 1,
                rating = "9",
                user_Comments = "Good place to eat"
            };
            Exception exception = new Exception("Entity Error : Restaurant Id cannot be less than one");
            var mockRatingBusiness = new Mock<IRatingBusiness>();
            mockRatingBusiness.Setup(x => x.UpdateRestaurantRating(ratingId, restauratRatings)).Throws(exception);

            //Act
            var ratingController = new RatingController(mockRatingBusiness.Object);
            var data = ratingController.UpdateRestaurantRating(ratingId, restauratRatings);
            var okObjectResult = data as ObjectResult;

            //Assert
            Assert.AreEqual(400, okObjectResult.StatusCode);
            Assert.IsNotNull(okObjectResult);
            Assert.AreEqual(okObjectResult.Value, exception.Message);
        }
        [Test]
        public void UpdateRestaurantRating_WrongInput_RatingIdLessThanOne()
        {
            //Arrange
            int ratingId = 0;
            RestaurantRating restauratRatings = new RestaurantRating()
            {
                RestaurantId = 1,
                RatingId = 0,
                rating = "9",
                user_Comments = "Good place to eat"
            };
            Exception exception = new Exception("Entity Error : Rating Id cannot be less than one");
            var mockRatingBusiness = new Mock<IRatingBusiness>();
            mockRatingBusiness.Setup(x => x.UpdateRestaurantRating(ratingId, restauratRatings)).Throws(exception);

            //Act
            var ratingController = new RatingController(mockRatingBusiness.Object);
            var data = ratingController.UpdateRestaurantRating(ratingId, restauratRatings);
            var okObjectResult = data as ObjectResult;

            //Assert
            Assert.AreEqual(400, okObjectResult.StatusCode);
            Assert.IsNotNull(okObjectResult);
            Assert.AreEqual(okObjectResult.Value, exception.Message);
        }
        [Test]
        public void UpdateRestaurantRating_WrongInput_RatingIsEmpty()
        {
            //Arrange
            int ratingId = 1;
            RestaurantRating restauratRatings = new RestaurantRating()
            {
                RestaurantId = 1,
                RatingId = 1,
                rating = string.Empty,
                user_Comments = "Good place to eat"
            };
            Exception exception = new Exception("Entity Error : Rating cannot be empty");
            var mockRatingBusiness = new Mock<IRatingBusiness>();
            mockRatingBusiness.Setup(x => x.UpdateRestaurantRating(ratingId, restauratRatings)).Throws(exception);

            //Act
            var ratingController = new RatingController(mockRatingBusiness.Object);
            var data = ratingController.UpdateRestaurantRating(ratingId, restauratRatings);
            var okObjectResult = data as ObjectResult;

            //Assert
            Assert.AreEqual(400, okObjectResult.StatusCode);
            Assert.IsNotNull(okObjectResult);
            Assert.AreEqual(okObjectResult.Value, exception.Message);
        }
        [Test]
        public void UpdateRestaurantRating_WrongInput_RatingIDDoesntExist()
        {
            //Arrange
            int ratingId = 1000;
            RestaurantRating restauratRatings = new RestaurantRating()
            {
                RestaurantId = 1,
                RatingId = 1000,
                rating = "9",
                user_Comments = "Good place to eat"
            };
            Exception exception = new Exception("Rating doesn't exist for the Restaurant 1000! Try creating the same rating using HttpPost method.");
            var mockRatingBusiness = new Mock<IRatingBusiness>();
            mockRatingBusiness.Setup(x => x.UpdateRestaurantRating(ratingId, restauratRatings)).Throws(exception);

            //Act
            var ratingController = new RatingController(mockRatingBusiness.Object);
            var data = ratingController.UpdateRestaurantRating(ratingId, restauratRatings);
            var okObjectResult = data as ObjectResult;

            //Assert
            Assert.AreEqual(400, okObjectResult.StatusCode);
            Assert.IsNotNull(okObjectResult);
            Assert.AreEqual(okObjectResult.Value, exception.Message);
        }
        [Test]
        public void UpdateRestaurantRating_ReturnedFalse()
        {
            //Arrange
            string result = "Error in updating restaurant! Try again after some time.";
            int ratingId = 1;
            RestaurantRating restauratRatings = new RestaurantRating()
            {
                RestaurantId = 1,
                RatingId = 1,
                rating = "9",
                user_Comments = "Good place to eat"
            };
            var mockRatingBusiness = new Mock<IRatingBusiness>();
            mockRatingBusiness.Setup(x => x.UpdateRestaurantRating(ratingId, restauratRatings)).Returns(false);

            //Act
            var ratingController = new RatingController(mockRatingBusiness.Object);
            var data = ratingController.UpdateRestaurantRating(ratingId, restauratRatings);
            var okObjectResult = data as ObjectResult;

            //Assert
            Assert.AreEqual(500, okObjectResult.StatusCode);
            Assert.IsNotNull(okObjectResult);
            Assert.AreEqual(okObjectResult.Value, result);
        }
        [Test]
        public void UpdateRestaurantRating_UnexpectedException()
        {
            //Arrange
            int ratingId = 1000;
            RestaurantRating restauratRatings = new RestaurantRating()
            {
                RestaurantId = 1,
                RatingId = 1000,
                rating = "9",
                user_Comments = "Good place to eat"
            };
            Exception exception = new Exception("Error in updating restaurant! Try again after some time.");
            var mockRatingBusiness = new Mock<IRatingBusiness>();
            mockRatingBusiness.Setup(x => x.UpdateRestaurantRating(ratingId, restauratRatings)).Throws(exception);

            //Act
            var ratingController = new RatingController(mockRatingBusiness.Object);
            var data = ratingController.UpdateRestaurantRating(ratingId, restauratRatings);
            var okObjectResult = data as ObjectResult;

            //Assert
            Assert.AreEqual(500, okObjectResult.StatusCode);
            Assert.IsNotNull(okObjectResult);
            Assert.AreEqual(okObjectResult.Value, exception.Message);
        }
        // Remove Rating Test Cases
        [Test]
        public void RemoveRestaurantRatings_CorrectOutput()
        {
            //Arrange
            int ratingId = 1;
            var mockRatingBusiness = new Mock<IRatingBusiness>();
            mockRatingBusiness.Setup(x => x.RemoveRestaurantRating(ratingId)).Returns(true);

            //Act
            var ratingController = new RatingController(mockRatingBusiness.Object);
            var data = ratingController.RemoveRestaurantRating(ratingId);
            var okObjectResult = data as NoContentResult;

            //Assert
            Assert.AreEqual(204, okObjectResult.StatusCode);
            Assert.IsNotNull(okObjectResult);
        }
        [Test]
        public void RemoveRestaurantRatings_WrongInput_RatingIdLessThanOne()
        {
            //Arrange
            int ratingId = 1;
            Exception exception = new Exception("Rating Id cannot be less than one! Try again with input for Rating Id greater than or equals one");
            var mockRatingBusiness = new Mock<IRatingBusiness>();
            mockRatingBusiness.Setup(x => x.RemoveRestaurantRating(ratingId)).Throws(exception);

            //Act
            var ratingController = new RatingController(mockRatingBusiness.Object);
            var data = ratingController.RemoveRestaurantRating(ratingId);
            var okObjectResult = data as ObjectResult;

            //Assert
            Assert.AreEqual(400, okObjectResult.StatusCode);
            Assert.IsNotNull(okObjectResult);
            Assert.AreEqual(okObjectResult.Value, exception.Message);
        }
        [Test]
        public void RemoveRestaurantRatings_WrongInput_RatingIdDoesntExist()
        {
            //Arrange
            int ratingId = 1;
            Exception exception = new Exception("Rating doesn't exist! Try creating the rating first");
            var mockRatingBusiness = new Mock<IRatingBusiness>();
            mockRatingBusiness.Setup(x => x.RemoveRestaurantRating(ratingId)).Throws(exception);

            //Act
            var ratingController = new RatingController(mockRatingBusiness.Object);
            var data = ratingController.RemoveRestaurantRating(ratingId);
            var okObjectResult = data as ObjectResult;

            //Assert
            Assert.AreEqual(400, okObjectResult.StatusCode);
            Assert.IsNotNull(okObjectResult);
            Assert.AreEqual(okObjectResult.Value, exception.Message);
        }
        [Test]
        public void RemoveRestaurantRatings_ReturnedFalse()
        {
            //Arrange
            string result = "Error in deleting rating for Id : 1! Try again after some time";
            int ratingId = 1;
            var mockRatingBusiness = new Mock<IRatingBusiness>();
            mockRatingBusiness.Setup(x => x.RemoveRestaurantRating(ratingId)).Returns(false);

            //Act
            var ratingController = new RatingController(mockRatingBusiness.Object);
            var data = ratingController.RemoveRestaurantRating(ratingId);
            var okObjectResult = data as ObjectResult;

            //Assert
            Assert.AreEqual(500, okObjectResult.StatusCode);
            Assert.IsNotNull(okObjectResult);
            Assert.AreEqual(okObjectResult.Value, result);
        }
        [Test]
        public void RemoveRestaurantRatings_UnexpectedException()
        {
            //Arrange
            int ratingId = 1;
            Exception exception = new Exception("Error in deleting rating for Id : 1! Try again after some time");
            var mockRatingBusiness = new Mock<IRatingBusiness>();
            mockRatingBusiness.Setup(x => x.RemoveRestaurantRating(ratingId)).Throws(exception);

            //Act
            var ratingController = new RatingController(mockRatingBusiness.Object);
            var data = ratingController.RemoveRestaurantRating(ratingId);
            var okObjectResult = data as ObjectResult;

            //Assert
            Assert.AreEqual(500, okObjectResult.StatusCode);
            Assert.IsNotNull(okObjectResult);
            Assert.AreEqual(okObjectResult.Value, exception.Message);
        }

    }
}