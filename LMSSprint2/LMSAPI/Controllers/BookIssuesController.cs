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
    public class BookIssuesController : ApiController
    {
        private BookIssueEntities db = new BookIssueEntities();

        // GET: api/BookIssues
        public IQueryable<BookIssue> GetBookIssues()
        {
            return db.BookIssues;
        }

        // GET: api/BookIssues/5
        [ResponseType(typeof(BookIssue))]
        public IHttpActionResult GetBookIssue(int id)
        {
            BookIssue bookIssue = db.BookIssues.Find(id);
            if (bookIssue == null)
            {
                return NotFound();
            }

            return Ok(bookIssue);
        }

        // PUT: api/BookIssues/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutBookIssue(int id, BookIssue bookIssue)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != bookIssue.BookIssueId)
            {
                return BadRequest();
            }

            db.Entry(bookIssue).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookIssueExists(id))
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

        // POST: api/BookIssues
        [ResponseType(typeof(BookIssue))]
        public IHttpActionResult PostBookIssue(BookIssue bookIssue)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.BookIssues.Add(bookIssue);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = bookIssue.BookIssueId }, bookIssue);
        }

        // DELETE: api/BookIssues/5
        [ResponseType(typeof(BookIssue))]
        public IHttpActionResult DeleteBookIssue(int id)
        {
            BookIssue bookIssue = db.BookIssues.Find(id);
            if (bookIssue == null)
            {
                return NotFound();
            }

            db.BookIssues.Remove(bookIssue);
            db.SaveChanges();

            return Ok(bookIssue);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool BookIssueExists(int id)
        {
            return db.BookIssues.Count(e => e.BookIssueId == id) > 0;
        }
    }
}