using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using LibraryForEmdx;

namespace ASP_WEB_API.Controllers
{
    public class CustomerRatingsController : ApiController
    {
        private OnlineFoodDeliveryAPPDBEntities db = new OnlineFoodDeliveryAPPDBEntities();

        // GET: api/CustomerRatings
        public IQueryable<CustomerRating> GetCustomerRatings()
        {
            return db.CustomerRatings;
        }

        // GET: api/CustomerRatings/5
        [ResponseType(typeof(CustomerRating))]
        public IHttpActionResult GetCustomerRating(long id)
        {
            CustomerRating customerRating = db.CustomerRatings.Find(id);
            if (customerRating == null)
            {
                return NotFound();
            }

            return Ok(customerRating);
        }

        // PUT: api/CustomerRatings/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutCustomerRating(long id, CustomerRating customerRating)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != customerRating.RatingId)
            {
                return BadRequest();
            }

            db.Entry(customerRating).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomerRatingExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/CustomerRatings
        [ResponseType(typeof(CustomerRating))]
        public IHttpActionResult PostCustomerRating(CustomerRating customerRating)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.CustomerRatings.Add(customerRating);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = customerRating.RatingId }, customerRating);
        }

        // DELETE: api/CustomerRatings/5
        [ResponseType(typeof(CustomerRating))]
        public IHttpActionResult DeleteCustomerRating(long id)
        {
            CustomerRating customerRating = db.CustomerRatings.Find(id);
            if (customerRating == null)
            {
                return NotFound();
            }

            db.CustomerRatings.Remove(customerRating);
            db.SaveChanges();

            return Ok(customerRating);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CustomerRatingExists(long id)
        {
            return db.CustomerRatings.Count(e => e.RatingId == id) > 0;
        }
    }
}