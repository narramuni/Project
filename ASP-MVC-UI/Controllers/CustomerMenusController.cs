using ASP_MVC_UI.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace ASP_MVC_UI.Controllers
{
    public class CustomerMenusController : Controller
    {

        private OnlineFoodDeliveryAPPDBEntities dbContext = new OnlineFoodDeliveryAPPDBEntities();



        // GET: CustomerMenus
        public ActionResult Index()
        {

            return View(dbContext.Restaurants.ToList());
        }

        public ActionResult ShowFoodItems(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // Retrieve the selected menu and its associated food items
            var menu = dbContext.Restaurants.Find(id);
            var foodItems = dbContext.Foods.Where(f => f.FoodRestaurentId == id).ToList();

            if (menu == null)
            {
                return HttpNotFound();
            }

            // Pass the menu and food items to the view
            ViewBag.Menu = menu;
            return View(foodItems);
        }
        public ActionResult AddToCart(long? foodId)
        {
            if (foodId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var foodItem = dbContext.Foods.Find(foodId);

            if (foodItem == null)
            {
                return HttpNotFound();
            }

            // Retrieve the customer ID from the session
            long? customerId = Session["CustomerId"] as long?;

            if (customerId == null)
            {
                // Handle the case where the customer ID is not found in the session (e.g., redirect to login)
                return RedirectToAction("CustomerLogin", "Home");
            }

            // Add the selected food item to the cart
            var cart = new Cart
            {
                CartCustId = customerId.Value,
                CartFoodId = foodItem.FoodId,
                CartFoodName = foodItem.FoodName,
                CartFoodQty = 1,
                CartFoodPrice = foodItem.FoodUnitPrice,
                CartFoodImage = foodItem.FoodImage
            };

           
            dbContext.Carts.Add(cart);
            dbContext.SaveChanges();

            // Pass a confirmation message to the view
            ViewBag.ConfirmationMessage = "Item added to cart. Do you want to shop more or go to the cart?";

            return View("Confirmation"); // Redirect to a confirmation view with two buttons
        }



     public ActionResult ShowCart()
        {
            // Retrieve the customer ID from the session
            long? customerId = Session["CustomerId"] as long?;

            if (customerId == null)
            {
                // Handle the case where the customer ID is not found in the session (e.g., redirect to login)
                return RedirectToAction("CustomerLogin", "Home");
            }

            // Retrieve the cart items for the current customer
            var cartItems = dbContext.Carts.Where(c => c.CartCustId == customerId.Value).ToList();

            return View(cartItems);
        }



        public ActionResult Orders()
        {
            // Retrieve the customer ID from the session
            long? customerId = Session["CustomerId"] as long?;

            if (customerId == null)
            {
                // Handle the case where the customer ID is not found in the session (e.g., redirect to login)
                return RedirectToAction("CustomerLogin", "Home");
            }

            // Retrieve the cart items for the current customer
            var cartItems = dbContext.Carts.Where(c => c.CartCustId == customerId.Value).ToList();

            return View(cartItems);
        }













        public ActionResult AddAddress()
        {
            return View();
        }






        [HttpPost]
        public ActionResult SaveAddress(Address address)
        {
            try
            {
                // Retrieve the customer ID from the session
                long? customerId = Session["CustomerId"] as long?;

                if (customerId == null)
                {
                    // Handle the case where the customer ID is not found in the session (e.g., redirect to login)
                    return RedirectToAction("CustomerLogin", "Home");
                }

                // Check if the ModelState is valid
                if (ModelState.IsValid)
                {
                    // Set the customer ID for the address
                    address.AddressCustId = customerId.Value;

                    // Save the address to the database
                    dbContext.Addresses.Add(address);
                    dbContext.SaveChanges();

                    // Redirect to the ShowAddress action
                    return RedirectToAction("ShowAddress");
                }
                else
                {
                    // If ModelState is not valid, there are validation errors
                    TempData["Message"] = "Please enter valid adress before going to Save";
                    return RedirectToAction("AddAddress");
                }
            }
            catch (Exception ex)
            {
                TempData["Message"] = "An error occurred while saving the address";
                return RedirectToAction("AddAddress");
            }
        }











        public ActionResult ShowAddress()
        {
            // Retrieve the customer ID from the session
            long? customerId = Session["CustomerId"] as long?;

            if (customerId == null)
            {
                // Handle the case where the customer ID is not found in the session (e.g., redirect to login)
                return RedirectToAction("CustomerLogin", "Home");
            }

            // Retrieve the customer's addresses
            var addresses = dbContext.Addresses.Where(a => a.AddressCustId == customerId.Value).ToList();

            return View(addresses);
        }














        public ActionResult OrderSuccess()
        {
            // Retrieve customer ID from the session
            long? customerId = Session["CustomerId"] as long?;

            // Fetch customer addresses and other data as needed
            var addresses = dbContext.Addresses.Where(a => a.AddressCustId == customerId).ToList();

            // Fetch customer rating from the database
            var customerRating = dbContext.CustomerRatings
                .Where(r => r.RatingCustId == customerId)
                .Select(r => r.RatingNumber)
                .FirstOrDefault();

            var viewModel = new OrderSuccessViewModel
            {
                Addresses = addresses,
                CustomerRating = customerRating
            };

            return View(viewModel);
        }













        [HttpPost]
        public ActionResult SubmitRating(int rating)
        {
            // Retrieve customer ID from the session
            long? customerId = Session["CustomerId"] as long?;

            // Update or insert the customer rating in the database
            var customerRating = dbContext.CustomerRatings
                .Where(r => r.RatingCustId == customerId)
                .FirstOrDefault();

            if (customerRating == null)
            {
                customerRating = new CustomerRating
                {
                    RatingCustId = customerId.Value,
                    RatingNumber = rating
                };

                dbContext.CustomerRatings.Add(customerRating);
            }
            else
            {
                customerRating.RatingNumber = rating;
            }

            dbContext.SaveChanges();

            return RedirectToAction("Index", "Home"); // Redirect to the home page or another page
        }













        public ActionResult EditProfile()
        {
            // Retrieve customer ID from the session
            long? customerId = Session["CustomerId"] as long?;

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
        public ActionResult UpdateProfile(Customer updatedCustomer)
        {
            // Retrieve customer ID from the session
            long? customerId = Session["CustomerId"] as long?;

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











        [HttpPost]
        public ActionResult RemoveFromCart(long? foodId)
        {
            if (foodId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // Retrieve the customer ID from the session
            long? customerId = Session["CustomerId"] as long?;

            if (customerId == null)
            {
                // Handle the case where the customer ID is not found in the session (e.g., redirect to login)
                return RedirectToAction("CustomerLogin", "Home");
            }

            // Find the cart item to be removed
            var cartItemToRemove = dbContext.Carts.FirstOrDefault(c => c.CartCustId == customerId && c.CartFoodId == foodId);

            if (cartItemToRemove != null)
            {
                // Remove the item from the cart and update the database
                dbContext.Carts.Remove(cartItemToRemove);
                dbContext.SaveChanges();
            }

            // You can return a JSON result if needed
            return Json(new { success = true });
        }




        /////////////////////////////////////////////////////////////////////////



        


    }


}


