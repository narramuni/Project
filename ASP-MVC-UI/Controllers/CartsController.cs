using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ASP_MVC_UI;

namespace ASP_MVC_UI.Controllers
{
    public class CartsController : Controller
    {
        private OnlineFoodDeliveryAPPDBEntities db = new OnlineFoodDeliveryAPPDBEntities();

        // GET: Carts
        public ActionResult Index()
        {
            var carts = db.Carts.Include(c => c.Food).Include(c => c.Customer);
            return View(carts.ToList());
        }

        // GET: Carts/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cart cart = db.Carts.Find(id);
            if (cart == null)
            {
                return HttpNotFound();
            }
            return View(cart);
        }

        // GET: Carts/Create
        public ActionResult Create()
        {
            ViewBag.CartFoodId = new SelectList(db.Foods, "FoodId", "FoodName");
            ViewBag.CartCustId = new SelectList(db.Customers, "CustId", "CustEmail");
            return View();
        }

        // POST: Carts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CartId,CartCustId,CartFoodId,CartFoodName,CartFoodQty,CartFoodPrice,CartFoodImage")] Cart cart)
        {
            if (ModelState.IsValid)
            {
                db.Carts.Add(cart);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CartFoodId = new SelectList(db.Foods, "FoodId", "FoodName", cart.CartFoodId);
            ViewBag.CartCustId = new SelectList(db.Customers, "CustId", "CustEmail", cart.CartCustId);
            return View(cart);
        }

        // GET: Carts/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cart cart = db.Carts.Find(id);
            if (cart == null)
            {
                return HttpNotFound();
            }
            ViewBag.CartFoodId = new SelectList(db.Foods, "FoodId", "FoodName", cart.CartFoodId);
            ViewBag.CartCustId = new SelectList(db.Customers, "CustId", "CustEmail", cart.CartCustId);
            return View(cart);
        }

        // POST: Carts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CartId,CartCustId,CartFoodId,CartFoodName,CartFoodQty,CartFoodPrice,CartFoodImage")] Cart cart)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cart).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CartFoodId = new SelectList(db.Foods, "FoodId", "FoodName", cart.CartFoodId);
            ViewBag.CartCustId = new SelectList(db.Customers, "CustId", "CustEmail", cart.CartCustId);
            return View(cart);
        }

        // GET: Carts/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cart cart = db.Carts.Find(id);
            if (cart == null)
            {
                return HttpNotFound();
            }
            return View(cart);
        }

        // POST: Carts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            Cart cart = db.Carts.Find(id);
            db.Carts.Remove(cart);
            db.SaveChanges();
            return RedirectToAction("ShowCart", "CustomerMenus");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
