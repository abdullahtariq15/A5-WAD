using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;
using A5_try2.Models;

namespace A5_try2.Controllers
{
    //[EnableCors(origins: "*", headers: "*", methods: "*")]
    public class TouristsController : ApiController
    {
        private travelEntities db = new travelEntities();

        // GET: api/Tourists
        [AllowAnonymous]
        public IQueryable<Tourist> GetTourists()
        {
            return db.Tourists;
        }

        // GET: api/Tourists/5
        [AllowAnonymous]
        [ResponseType(typeof(Tourist))]
        public IHttpActionResult GetTourist(string id)
        {
            Tourist tourist = db.Tourists.Find(id);
            if (tourist == null)
            {
                return NotFound();
            }

            return Ok(tourist);
        }

        // PUT: api/Tourists/5
        [Authorize(Roles = "SuperAdmin")]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutTourist(string id, Tourist tourist)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tourist.TouristId)
            {
                return BadRequest();
            }

            db.Entry(tourist).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TouristExists(id))
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

        // POST: api/Tourists
        [Authorize(Roles = "SuperAdmin")]
        [ResponseType(typeof(Tourist))]
        public IHttpActionResult PostTourist(Tourist tourist)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Tourists.Add(tourist);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (TouristExists(tourist.TouristId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = tourist.TouristId }, tourist);
        }

        // DELETE: api/Tourists/5
        [Authorize(Roles = "Admin")]
        [ResponseType(typeof(Tourist))]
        public IHttpActionResult DeleteTourist(string id)
        {
            Tourist tourist = db.Tourists.Find(id);
            if (tourist == null)
            {
                return NotFound();
            }

            db.Tourists.Remove(tourist);
            db.SaveChanges();

            return Ok(tourist);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TouristExists(string id)
        {
            return db.Tourists.Count(e => e.TouristId == id) > 0;
        }
    }
}