using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;
using A5_try2.Models;

namespace A5_try2.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class PackagesController : ApiController
    {
        private travelEntities db = new travelEntities();

        // GET: api/Packages
        [AllowAnonymous]
        public IQueryable<Package> GetPackages()
        {
            return db.Packages;
        }

        // GET: api/Packages/5
        [AllowAnonymous]
        [ResponseType(typeof(Package))]
        public IHttpActionResult GetPackage(string id)
        {
            Package package = db.Packages.Find(id);
            if (package == null)
            {
                return NotFound();
            }

            return Ok(package);
        }

        // PUT: api/Packages/5
        [Authorize(Roles = "Admin")]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutPackage(string id, Package package)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != package.PackageId)
            {
                return BadRequest();
            }

            db.Entry(package).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PackageExists(id))
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

        // POST: api/Packages
        [Authorize(Roles = "SuperAdmin")]
        [ResponseType(typeof(Package))]
        public IHttpActionResult PostPackage(Package package)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                db.Packages.Add(package);

                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateException)
                {
                    if (PackageExists(package.PackageId))
                    {
                        return Conflict();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                foreach (var entityValidationErrors in ex.EntityValidationErrors)
                {
                    foreach (var validationError in entityValidationErrors.ValidationErrors)
                    {
                        System.Diagnostics.Debug.WriteLine("Property: " + validationError.PropertyName + " Error: " + validationError.ErrorMessage);
                    }
                }
            }
            return CreatedAtRoute("DefaultApi", new { id = package.PackageId }, package);
        }

        // DELETE: api/Packages/5
        [Authorize(Roles = "SuperAdmin")]
        [ResponseType(typeof(Package))]
        public IHttpActionResult DeletePackage(string id)
        {
            Package package = db.Packages.Find(id);
            if (package == null)
            {
                return NotFound();
            }

            db.Packages.Remove(package);
            db.SaveChanges();

            return Ok(package);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PackageExists(string id)
        {
            return db.Packages.Count(e => e.PackageId == id) > 0;
        }
    }
}