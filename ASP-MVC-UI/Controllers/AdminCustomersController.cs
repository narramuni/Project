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
    public class AdminCustomersController : Controller
    {
        private OnlineFoodDeliveryAPPDBEntities db = new OnlineFoodDeliveryAPPDBEntities();

        // GET: AdminCustomers
        public ActionResult Index()
        {
            return View(db.Customers.ToList());
        }

        // GET: AdminCustomers/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // GET: AdminCustomers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AdminCustomers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CustId,CustEmail,CustPhone,CustFName,CustLName,CustPassword")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                db.Customers.Add(customer);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(customer);
        }

        // GET: AdminCustomers/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // POST: AdminCustomers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CustId,CustEmail,CustPhone,CustFName,CustLName,CustPassword")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                db.Entry(customer).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(customer);
        }

        // GET: AdminCustomers/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // POST: AdminCustomers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            // Find the customer to delete
            Customer customer = db.Customers.Find(id);

            if (customer == null)
            {
                return HttpNotFound();
            }

            // Delete related records in the Cart table
            var relatedCarts = db.Carts.Where(c => c.CartCustId == id);

            foreach (var cart in relatedCarts)
            {
                db.Carts.Remove(cart);
            }

           var relatedAdresses = db.Addresses.Where(b => b.AddressCustId == id);
            foreach (var Adress in relatedAdresses)
            {
                db.Addresses.Remove(Adress);
            }

            var relatedCustomerRatings = db.CustomerRatings.Where(a => a.RatingCustId == id);
            foreach (var CR in relatedCustomerRatings)
            {
                db.CustomerRatings.Remove(CR);
            }


            db.Customers.Remove(customer);
            db.SaveChanges();

            return RedirectToAction("Index");
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
