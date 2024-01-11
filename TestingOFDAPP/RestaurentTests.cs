using NUnit.Framework;
using NUnit.Framework.Legacy;
using System.Linq;

namespace TestingOFDAPP
{
    [TestFixture]
    public class RestaurantTests
    {
        private OnlineFoodDeliveryAPPDBEntities1 dbContext = new OnlineFoodDeliveryAPPDBEntities1();

        [Test]
        public void AddNewRestaurant_ShouldIncreaseRestaurantCount()
        {
            // Arrange
            var initialRestaurantCount = dbContext.Restaurants.Count();

            // Act
            var newRestaurant = new Restaurant
            {
                RestaurentName = "KFC",
                RestaurentAvailable = true,
                RestaurentLogoUrl = "/pictures/KFC.jpg",
                RestaurentAdminId = 1
            };

            dbContext.Restaurants.Add(newRestaurant);
            dbContext.SaveChanges();

            // Assert
            var updatedRestaurantCount = dbContext.Restaurants.Count();
            ClassicAssert.AreEqual(initialRestaurantCount + 1, updatedRestaurantCount, "Restaurant count should be increased after adding a new restaurant.");
        }

       
       

        private bool RestaurantNameExists(string restaurantName)
        {
            // Implement logic to check if the restaurant name exists
            var existingRestaurant = dbContext.Restaurants.FirstOrDefault(r => r.RestaurentName == restaurantName);
            return existingRestaurant != null;
        }


        [Test]
        public void CheckRestaurantName_ShouldReturnTrue()
        {
            // Arrange
            var restaurantNameToCheck = "KFC";

            // Act
            var isRestaurantNameExist = RestaurantNameExists(restaurantNameToCheck);

            // Assert
            ClassicAssert.IsTrue(isRestaurantNameExist, $"Restaurant name '{restaurantNameToCheck}' should exist.");
        }


        private bool RestaurantAvailabilityChecking(int restaurantId)
        {
            var existingRestaurant = dbContext.Restaurants.FirstOrDefault(r => r.RestaurentId == restaurantId && r.RestaurentAvailable);
            return existingRestaurant != null;
        }



        [Test]
        public void CheckRestaurantAvailability_ShouldReturnTrue()
        {
            // Arrange
            var restaurantIdToCheck = 5;

            // Act
            var isRestaurantAvailable = RestaurantAvailabilityChecking(restaurantIdToCheck);

            // Assert
            ClassicAssert.IsTrue(isRestaurantAvailable, $"Restaurant with ID '{restaurantIdToCheck}' should be available.");
        }
    }
}
