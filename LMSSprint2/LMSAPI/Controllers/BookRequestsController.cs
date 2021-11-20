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
using LMSAPI.Models;

namespace LMSAPI.Controllers
{
    public class BookRequestsController : ApiController
    {
        private BookRequestEntities1 db = new BookRequestEntities1();

        // GET: api/BookRequests
        public IQueryable<BookRequest> GetBookRequests()
        {
            return db.BookRequests;
        }

        // GET: api/BookRequests/5
        [ResponseType(typeof(BookRequest))]
        public IHttpActionResult GetBookRequest(int id)
        {
            BookRequest bookRequest = db.BookRequests.Find(id);
            if (bookRequest == null)
            {
                return NotFound();
            }

            return Ok(bookRequest);
        }

        // PUT: api/BookRequests/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutBookRequest(int id, BookRequest bookRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != bookRequest.BookRequestId)
            {
                return BadRequest();
            }

            db.Entry(bookRequest).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookRequestExists(id))
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

        // POST: api/BookRequests
        [ResponseType(typeof(BookRequest))]
        public IHttpActionResult PostBookRequest(BookRequest bookRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.BookRequests.Add(bookRequest);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = bookRequest.BookRequestId }, bookRequest);
        }

        // DELETE: api/BookRequests/5
        [ResponseType(typeof(BookRequest))]
        public IHttpActionResult DeleteBookRequest(int id)
        {
            BookRequest bookRequest = db.BookRequests.Find(id);
            if (bookRequest == null)
            {
                return NotFound();
            }

            db.BookRequests.Remove(bookRequest);
            db.SaveChanges();

            return Ok(bookRequest);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool BookRequestExists(int id)
        {
            return db.BookRequests.Count(e => e.BookRequestId == id) > 0;
        }
    }
}