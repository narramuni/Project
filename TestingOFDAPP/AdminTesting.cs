using NUnit.Framework;
using NUnit.Framework.Legacy;
using System;
using System.Linq;

namespace TestingOFDAPP
{
    [TestFixture]
    public class AdminTesting
    {
        private OnlineFoodDeliveryAPPDBEntities1 dbContext = new OnlineFoodDeliveryAPPDBEntities1();

        [Test]
        public void TestingLoginDetails()
        {
            // Arrange
            string ausername1 = "Saipavan";
            string apassword1 = "Saipavan@123";

            //string ausername2 = "Admin2";
            //string apassword2 = "Admin2@123";

            // Act
            var admin1 = dbContext.Admins.FirstOrDefault(a => a.AdminUsername == ausername1 && a.AdminPassword == apassword1);
            //var admin2 = dbContext.Admins.FirstOrDefault(a => a.AdminUsername == ausername2 && a.AdminPassword == apassword2);

            // Assert
            ClassicAssert.IsNotNull(admin1);
            ClassicAssert.AreEqual(ausername1, admin1.AdminUsername);
            ClassicAssert.AreEqual(apassword1, admin1.AdminPassword);

            //ClassicAssert.IsNotNull(admin2);
            //ClassicAssert.AreEqual(ausername2, admin2.AdminUsername);
            //ClassicAssert.AreEqual(apassword2, admin2.AdminPassword);
        }

        [Test]
        public void TestingAdminId()
        {
            // Arrange
            long Admin1 = 1;
            long Admin2 = 7;

            // Act
            var aid1 = dbContext.Admins.FirstOrDefault(a => a.AdminId == Admin1);
            var aid2 = dbContext.Admins.FirstOrDefault(a => a.AdminId == Admin2);

            // Assert
            ClassicAssert.IsNotNull(aid1);
            ClassicAssert.AreEqual(Admin1, aid1.AdminId);

            ClassicAssert.IsNotNull(aid2);
            ClassicAssert.AreEqual(Admin2, aid2.AdminId);
        }

        [Test]
        public void TestingPhoneNo()
        {
            // Arrange
            string Admin1 = "9441109518";
            //string Admin2 = "8787878787";

            // Act
            var pno1 = dbContext.Admins.FirstOrDefault(a => a.AdminPhone == Admin1);
            //var pno2 = dbContext.Admins.FirstOrDefault(a => a.AdminPhone == Admin2);

            // Assert
            ClassicAssert.IsNotNull(pno1);
            ClassicAssert.AreEqual(Admin1, pno1.AdminPhone);

            //ClassicAssert.IsNotNull(pno2);
            //ClassicAssert.AreEqual(Admin2, pno2.AdminPhone);
        }

        [Test]
        public void TestingFNameAndLName()
        {
            // Arrange
            string fn1 = "Saipavan";
            string ln1 = "NM";

            //string fn2 = "Admin2";
            //string ln2 = "Admin2";

            // Act
            var fln1 = dbContext.Admins.FirstOrDefault(a => a.AdminFName == fn1 && a.AdminLName == ln1);
           // var fln2 = dbContext.Admins.FirstOrDefault(a => a.AdminFName == fn2 && a.AdminLName == ln2);

            // Assert
            ClassicAssert.IsNotNull(fn1);
            ClassicAssert.AreEqual(fn1, fln1.AdminFName);
            ClassicAssert.IsNotNull(ln1);
            ClassicAssert.AreEqual(ln1, fln1.AdminLName);

            //ClassicAssert.IsNotNull(fn2);
            //ClassicAssert.AreEqual(fn2, fln2.AdminFName);
            //ClassicAssert.IsNotNull(ln2);
            //ClassicAssert.AreEqual(ln2, fln2.AdminLName);
        }

        [Test]
        public void TestingAddingNewAdmin()
        {
            // Arrange
            var newAdmin = new Admin
            {
                AdminId = 2,
                AdminUsername = "admin2",
                AdminPhone = "2222222222",
                AdminFName = "jack",
                AdminLName = "ron",
                AdminPassword = "admin456"
            };

            // Act
            dbContext.Admins.Add(newAdmin);
            dbContext.SaveChanges();

            // Assert
            var retrievedAdmin = dbContext.Admins.FirstOrDefault(a => a.AdminFName == "jack");
            ClassicAssert.IsNotNull(retrievedAdmin);
            ClassicAssert.AreEqual(newAdmin.AdminFName, retrievedAdmin.AdminFName);
            // Add more assertions based on your table structure
        }

        [Test]
        public void TestingAdminWithInvalidCredentials()
        {
            // Arrange
            string username = "Admin3@gmail.com";
            string password = "Admin3@123";

            // Act
            var admin = dbContext.Admins.FirstOrDefault(a => a.AdminUsername != username && a.AdminPassword != password);

            // Assert
            ClassicAssert.AreNotEqual(username, admin?.AdminUsername);
            ClassicAssert.AreNotEqual(password, admin?.AdminPassword);

            string username2 = "user2@gmail.com";
            string password2 = "user2/123";

            // Act
            var admin2 = dbContext.Admins.FirstOrDefault(a => a.AdminUsername != username2 && a.AdminPassword != password2);

            // Assert
            ClassicAssert.AreNotEqual(username2, admin2?.AdminUsername);
            ClassicAssert.AreNotEqual(password2, admin2?.AdminPassword);
        }
    }
}
