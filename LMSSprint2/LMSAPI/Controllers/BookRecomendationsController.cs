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
    public class BookRecomendationsController : ApiController
    {
        private BookRecommendationEntities db = new BookRecommendationEntities();

        // GET: api/BookRecomendations
        public IQueryable<BookRecomendation> GetBookRecomendations()
        {
            return db.BookRecomendations;
        }

        // GET: api/BookRecomendations/5
        [ResponseType(typeof(BookRecomendation))]
        public IHttpActionResult GetBookRecomendation(int id)
        {
            BookRecomendation bookRecomendation = db.BookRecomendations.Find(id);
            if (bookRecomendation == null)
            {
                return NotFound();
            }

            return Ok(bookRecomendation);
        }

        // PUT: api/BookRecomendations/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutBookRecomendation(int id, BookRecomendation bookRecomendation)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != bookRecomendation.BookRecomendationId)
            {
                return BadRequest();
            }

            db.Entry(bookRecomendation).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookRecomendationExists(id))
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

        // POST: api/BookRecomendations
        [ResponseType(typeof(BookRecomendation))]
        public IHttpActionResult PostBookRecomendation(BookRecomendation bookRecomendation)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.BookRecomendations.Add(bookRecomendation);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = bookRecomendation.BookRecomendationId }, bookRecomendation);
        }

        // DELETE: api/BookRecomendations/5
        [ResponseType(typeof(BookRecomendation))]
        public IHttpActionResult DeleteBookRecomendation(int id)
        {
            BookRecomendation bookRecomendation = db.BookRecomendations.Find(id);
            if (bookRecomendation == null)
            {
                return NotFound();
            }

            db.BookRecomendations.Remove(bookRecomendation);
            db.SaveChanges();

            return Ok(bookRecomendation);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool BookRecomendationExists(int id)
        {
            return db.BookRecomendations.Count(e => e.BookRecomendationId == id) > 0;
        }
    }
}