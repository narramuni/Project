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
    public class AppAdminsController : ApiController
    {
        private OnlineFoodDeliveryAPPDBEntities db = new OnlineFoodDeliveryAPPDBEntities();

        // GET: api/AppAdmins
        public IQueryable<AppAdmin> GetAppAdmins()
        {
            return db.AppAdmins;
        }

        // GET: api/AppAdmins/5
        [ResponseType(typeof(AppAdmin))]
        public IHttpActionResult GetAppAdmin(long id)
        {
            AppAdmin appAdmin = db.AppAdmins.Find(id);
            if (appAdmin == null)
            {
                return NotFound();
            }

            return Ok(appAdmin);
        }

        // PUT: api/AppAdmins/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutAppAdmin(long id, AppAdmin appAdmin)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != appAdmin.AppAdminId)
            {
                return BadRequest();
            }

            db.Entry(appAdmin).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AppAdminExists(id))
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

        // POST: api/AppAdmins
        [ResponseType(typeof(AppAdmin))]
        public IHttpActionResult PostAppAdmin(AppAdmin appAdmin)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.AppAdmins.Add(appAdmin);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = appAdmin.AppAdminId }, appAdmin);
        }

        // DELETE: api/AppAdmins/5
        [ResponseType(typeof(AppAdmin))]
        public IHttpActionResult DeleteAppAdmin(long id)
        {
            AppAdmin appAdmin = db.AppAdmins.Find(id);
            if (appAdmin == null)
            {
                return NotFound();
            }

            db.AppAdmins.Remove(appAdmin);
            db.SaveChanges();

            return Ok(appAdmin);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool AppAdminExists(long id)
        {
            return db.AppAdmins.Count(e => e.AppAdminId == id) > 0;
        }
    }
}