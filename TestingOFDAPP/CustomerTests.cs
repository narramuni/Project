using NUnit.Framework;
using NUnit.Framework.Legacy;
using System.Linq;

namespace TestingOFDAPP
{
    [TestFixture]
    public class CustomerTests
    {
        private OnlineFoodDeliveryAPPDBEntities1 dbContext = new OnlineFoodDeliveryAPPDBEntities1();

        [Test]
        public void GetCustomerById_ShouldReturnCorrectCustomer()
        {
            // Arrange
            long customerId = 1;

            // Act
            var customer = dbContext.Customers.FirstOrDefault(c => c.CustId == customerId);

            // Assert
            ClassicAssert.IsNotNull(customer);
            ClassicAssert.AreEqual(customerId, customer.CustId);
        }

        [Test]
        public void GetCustomerByEmailAndPassword_ShouldReturnCorrectCustomer()
        {
            // Arrange
            string email = "Sai@gmail.com";
            string password = "Sai@123";

            // Act
            var customer = dbContext.Customers.FirstOrDefault(c => c.CustEmail == email && c.CustPassword == password);

            // Assert
            ClassicAssert.IsNotNull(customer);
            ClassicAssert.AreEqual(email, customer.CustEmail);
            ClassicAssert.AreEqual(password, customer.CustPassword);
        }

        [Test]
        public void GetCustomerByInvalidCredentials_ShouldReturnNull()
        {
            // Arrange
            string email = "nonexistent@example.com";
            string password = "invalidpassword";

            // Act
            var customer = dbContext.Customers.FirstOrDefault(c => c.CustEmail == email && c.CustPassword == password);

            // Assert
            ClassicAssert.IsNull(customer);
        }
    }
}
