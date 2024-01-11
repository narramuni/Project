using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace ASP_MVC_UI.Controllers
{
    public class AdminModuleController : Controller
    {
        private long GetLoggedInAdminId()
        {
            return Session["AdminId"] as long? ?? 0;
        }

        private long GetRestaurentId()
        {
            return Session["RestaurentId"] as long? ?? 0;
        }




        private bool IsAuthorizedToEdit(Restaurant restaurant)
        {
            var adminId = GetLoggedInAdminId();

            // Check if the restaurant and admin ID match
            return restaurant != null && adminId == restaurant.RestaurentAdminId;
        }



        private OnlineFoodDeliveryAPPDBEntities dbContext = new OnlineFoodDeliveryAPPDBEntities();

/// ************************** Admin  Profile Editing  **********************
        public ActionResult EditAdminProfile()
        {
            // Retrieve admin ID from the session
            long? adminId = Session["AdminId"] as long?;

            if (adminId == null)
            {
                // Handle the case where the admin ID is not found in the session (e.g., redirect to login)
                return RedirectToAction("AdminLogin", "Home");
            }

            // Retrieve the admin details
            var admin = dbContext.Admins.Find(adminId);

            if (admin == null)
            {
                // Handle the case where the admin is not found in the database
                return HttpNotFound();
            }

            // Pass the admin details to the view for editing
            return View(admin);
        }

        [HttpPost]
        public ActionResult UpdateAdminProfile(Admin updatedAdmin)
        {
            // Retrieve admin ID from the session
            long? adminId = Session["AdminId"] as long?;

            if (adminId == null)
            {
                // Handle the case where the admin ID is not found in the session (e.g., redirect to login)
                return RedirectToAction("AdminLogin", "Home");
            }

            // Retrieve the existing admin details from the database
            var existingAdmin = dbContext.Admins.Find(adminId);

            if (existingAdmin == null)
            {
                // Handle the case where the admin is not found in the database
                return HttpNotFound();
            }

            // Update the admin details with the edited values
            existingAdmin.AdminUsername = updatedAdmin.AdminUsername;
            existingAdmin.AdminPhone = updatedAdmin.AdminPhone;
            existingAdmin.AdminFName = updatedAdmin.AdminFName;
            existingAdmin.AdminLName = updatedAdmin.AdminLName;
            existingAdmin.AdminPassword = updatedAdmin.AdminPassword; // You may want to add password hashing logic

            // Save changes to the database
            dbContext.Entry(existingAdmin).State = EntityState.Modified;
            dbContext.SaveChanges();

            // Redirect to the admin login page
            return RedirectToAction("AdminLogin", "Home");
        }







         
     
        // GET: Admin
        public ActionResult Index()
        {
            // Get the logged-in admin ID
            var adminId = GetLoggedInAdminId();

            // Retrieve restaurants associated with the logged-in admin
            var restaurants = dbContext.Restaurants.Where(r => r.RestaurentAdminId == adminId).ToList();

            return View(restaurants);
        }




// *******************************  Restaurents CRUD Operations   ***************************


        // GET: Admin/Create
        // GET: Admin/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Restaurant restaurant)
        {
            var adminId = GetLoggedInAdminId(); // You need to implement this method
            restaurant.RestaurentAdminId = adminId;


            dbContext.Restaurants.Add(restaurant);
            dbContext.SaveChanges();
            TempData["Message"] = "Restaurant created successfully!";
            return RedirectToAction("Index");
        }




        // GET: Admin/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Restaurant restaurant = dbContext.Restaurants.Find(id);

            if (restaurant == null)
            {
                return HttpNotFound();
            }

            return View(restaurant);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            try
            {
                Restaurant restaurant = dbContext.Restaurants.Find(id);

                if (IsAuthorizedToEdit(restaurant))
                {

                    var relatedFoods = dbContext.Foods.Where(Z => Z.FoodRestaurentId == id);
                    foreach (var RF in relatedFoods)
                    {
                        dbContext.Foods.Remove(RF);
                    }


                    dbContext.Restaurants.Remove(restaurant);
                    dbContext.SaveChanges();
                    TempData["Message"] = "Restaurant deleted successfully!";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["Message"] = "You are not authorized to delete this restaurant.";
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                TempData["Message"] = "Please delete food items in Restaurant before going to delete the Restaurant!!!!";
                return RedirectToAction("Index");
            }
        }







        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Restaurant restaurant = dbContext.Restaurants.Find(id);

            if (restaurant == null)
            {
                return HttpNotFound();
            }

            return View(restaurant);
        }






        public ActionResult EditRestaurant(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Restaurant restaurant = dbContext.Restaurants.Find(id);

            if (restaurant == null)
            {
                return HttpNotFound();
            }

            return View(restaurant);
        }


        [HttpPost]
        public ActionResult UpdateRestaurent(Restaurant updateRestaurent)
        {
            // Retrieve admin ID from the session
            long? adminId = Session["AdminId"] as long?;

            if (adminId == null)
            {
                // Handle the case where the admin ID is not found in the session (e.g., redirect to login)
                return RedirectToAction("AdminLogin", "Home");
            }

            // Retrieve the existing restaurant details from the database based on adminId
            var existingRestaurent = dbContext.Restaurants.FirstOrDefault(r => r.RestaurentAdminId == adminId);

            if (existingRestaurent == null)
            {
                // Handle the case where the restaurant is not found in the database
                return HttpNotFound();
            }

            // Update the restaurant details with the edited values
            existingRestaurent.RestaurentName = updateRestaurent.RestaurentName;
            existingRestaurent.RestaurentAvailable = updateRestaurent.RestaurentAvailable;
            existingRestaurent.RestaurentLogoUrl = updateRestaurent.RestaurentLogoUrl;
            // You may want to add additional logic for other fields

            // Save changes to the database
            dbContext.Entry(existingRestaurent).State = EntityState.Modified;
            dbContext.SaveChanges();

            // Redirect to the admin login page
            return RedirectToAction("Index", "AdminModule");

        }



/////    Food Item crud operations ************************///// 



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





        public ActionResult DetailsShowFoodItem(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var food = dbContext.Foods.Find(id);

            if (food == null)
            {
                return HttpNotFound();
            }

            return View(food);
        }




        [HttpGet]
        public ActionResult CreateShowFoodItem(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // Retrieve the list of restaurants for the dropdown
            var restaurants = dbContext.Restaurants.Select(r => new SelectListItem
            {
                Value = r.RestaurentId.ToString(),
                Text = r.RestaurentName
            });

            // Pass the restaurant information and the dropdown list to the view
            ViewBag.RestaurantId = id;
            ViewBag.Restaurants = restaurants;

            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateShowFoodItem(Food food)
        {
            var selectedRestaurantId = food.FoodRestaurentId;

            if (ModelState.IsValid)
            {
                dbContext.Foods.Add(food);
                dbContext.SaveChanges();

                // Redirect to the ShowFoodItems action of the associated restaurant
                return RedirectToAction("ShowFoodItems", new { id = food.FoodRestaurentId });
            }

            // If the model state is not valid, reload the restaurant information and return to the Create view with the current model
            ViewBag.RestaurantId = food.FoodRestaurentId;
            ViewBag.RestaurantName = dbContext.Restaurants.Find(food.FoodRestaurentId)?.RestaurentName;

            return View(food);
        }






        // Edit Action
        [HttpGet]
        public ActionResult EditShowFoodItem(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var food = dbContext.Foods.Find(id);

            if (food == null)
            {
                return HttpNotFound();
            }

            // Retrieve the list of restaurants for the dropdown
            var restaurants = dbContext.Restaurants.Select(r => new SelectListItem
            {
                Value = r.RestaurentId.ToString(),
                Text = r.RestaurentName
            });

            // Set the ViewData for the dropdown
            ViewBag.Restaurants = restaurants;

            return View(food);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditShowFoodItem(Food food)
        {
            if (ModelState.IsValid)
            {
                dbContext.Entry(food).State = EntityState.Modified;
                dbContext.SaveChanges();

                return RedirectToAction("ShowFoodItems", new { id = food.FoodRestaurentId });
            }

            // If the model state is not valid, return to the Edit view with the current model
            return View(food);
        }






        // Delete Action
        [HttpGet]
        public ActionResult DeleteShowFoodItem(long id)
        {
            var food = dbContext.Foods.Find(id);

            if (food == null)
            {
                return HttpNotFound();
            }

            return View(food);
        }

        [HttpPost, ActionName("DeleteShowFoodItem")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmedShowFoodItem(long id)
        {
            var food = dbContext.Foods.Find(id);

            if (food == null)
            {
                return HttpNotFound();
            }

            dbContext.Foods.Remove(food);
            dbContext.SaveChanges();

            // Redirect to the ShowFoodItems action
            return RedirectToAction("ShowFoodItems", new { id = food.FoodRestaurentId });
        }



        /////************************ Customer Ratings ******************************///
        


        public ActionResult CustomerRatings()
        {
            // Retrieve customer ratings from the database
            var customerRatings = dbContext.CustomerRatings.ToList();

            return View(customerRatings);
        }

        
    }


}
