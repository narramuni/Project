using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ASP_MVC_UI.Controllers
{
    public class HomeController : Controller
    {
        OnlineFoodDeliveryAPPDBEntities dbContext = new OnlineFoodDeliveryAPPDBEntities();
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CustomerRegister()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CustomerRegister(Customer customer)
        {
            if (ModelState.IsValid)
            {
                // You may want to add additional validation logic here

                dbContext.Customers.Add(customer);
                dbContext.SaveChanges();

                return RedirectToAction("CustomerLogin", "Home"); 
            }

            return View(customer);
        }

        public ActionResult CustomerLogin()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CustomerLogin(string custEmail, string custPassword)
        {
            
            var customer = dbContext.Customers
                .FirstOrDefault(c => c.CustEmail == custEmail && c.CustPassword == custPassword);

            if (customer != null)
            {
                
                Session["CustomerId"] = customer.CustId;

               
                return RedirectToAction("Index", "CustomerMenus");
            }
            else
            {
                
                ModelState.AddModelError("", "please enter correct Email and Password");
                return View();
            }
        }


        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ForgotPassword(string custEmail,string CustPhone)
        {
            var customers = dbContext.Customers
                .FirstOrDefault(y => y.CustEmail == custEmail && y.CustPhone == CustPhone);
            if (customers != null)
            {
                
                Session["CustomerIdddd"] = customers.CustId;

                
                return RedirectToAction("ResetPassword", "Home");
            }
            else
            {
                
                ModelState.AddModelError("", "your email and phone No is Incorrect so you cant reset your Passwors");
                return View();
            }
        }



        public ActionResult ResetPassword()
        {
            // Retrieve customer ID from the session
            long? customerId = Session["CustomerIdddd"] as long?;

            if (customerId == null)
            {
                // Handle the case where the customer ID is not found in the session (e.g., redirect to login)
                return RedirectToAction("CustomerLogin", "Home");
            }

            // Retrieve the customer details
            var customer = dbContext.Customers.Find(customerId);

            if (customer == null)
            {
                // Handle the case where the customer is not found in the database
                return HttpNotFound();
            }

            // Pass the customer details to the view for editing
            return View(customer);
        }

        [HttpPost]
        public ActionResult UpdateResetPassword(Customer updatedCustomer)
        {
            try
            {
                // Retrieve customer ID from the session
                long? customerId = Session["CustomerIdddd"] as long?;

                if (customerId == null)
                {
                    // Handle the case where the customer ID is not found in the session (e.g., redirect to login)
                    return RedirectToAction("CustomerLogin", "Home");
                }

                // Retrieve the existing customer details from the database
                var existingCustomer = dbContext.Customers.Find(customerId);

                if (existingCustomer == null)
                {
                    // Handle the case where the customer is not found in the database
                    return HttpNotFound();
                }

                // Update the customer details with the edited values
                existingCustomer.CustEmail = updatedCustomer.CustEmail;
                existingCustomer.CustPhone = updatedCustomer.CustPhone;
                existingCustomer.CustFName = updatedCustomer.CustFName;
                existingCustomer.CustLName = updatedCustomer.CustLName;
                existingCustomer.CustPassword = updatedCustomer.CustPassword; // You may want to add password hashing logic

                // Save changes to the database
                dbContext.Entry(existingCustomer).State = EntityState.Modified;
                dbContext.SaveChanges();

                // Redirect to the customer login page
                return RedirectToAction("CustomerLogin", "Home");
            }
            catch (Exception ex)
            {
                TempData["Message"] = "Please enter All fields before going to Update Password";
                return RedirectToAction("Index");
            }
        }





        public ActionResult AdminLogin()
        {
            return View();
        }  
            [HttpPost]
        public ActionResult AdminLogin(string adminUsername, string adminPassword)
        {
            var admin = dbContext.Admins
                .FirstOrDefault(a => a.AdminUsername == adminUsername && a.AdminPassword == adminPassword);

            if (admin != null)
            {
                // Admin authentication successful

                // Store admin information in session or cookie
                Session["AdminId"] = admin.AdminId;
                Session["AdminUsername"] = admin.AdminUsername;

                // Store RestaurentId in session
                var restaurant = dbContext.Restaurants.FirstOrDefault(r => r.RestaurentAdminId == admin.AdminId);
                if (restaurant != null)
                {
                    Session["RestaurentId"] = restaurant.RestaurentId;
                }

                return RedirectToAction("Index", "AdminModule");
            }
            else
            {
                // Admin authentication failed
                ModelState.AddModelError("", "Please enter valid Username and Password");
                return View();
            }
        }

    }
}