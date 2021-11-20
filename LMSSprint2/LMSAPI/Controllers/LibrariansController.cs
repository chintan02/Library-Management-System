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
    public class LibrariansController : ApiController
    {
        private LibrarianEntities db = new LibrarianEntities();

        // GET: api/Librarians
        public IQueryable<Librarian> GetLibrarians()
        {
            return db.Librarians;
        }

        // GET: api/Librarians/5
        [ResponseType(typeof(Librarian))]
        public IHttpActionResult GetLibrarian(int id)
        {
            Librarian librarian = db.Librarians.Find(id);
            if (librarian == null)
            {
                return NotFound();
            }

            return Ok(librarian);
        }

        // PUT: api/Librarians/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutLibrarian(int id, Librarian librarian)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != librarian.LibrarianId)
            {
                return BadRequest();
            }

            db.Entry(librarian).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LibrarianExists(id))
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

        // POST: api/Librarians
        [ResponseType(typeof(Librarian))]
        public IHttpActionResult PostLibrarian(Librarian librarian)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Librarians.Add(librarian);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = librarian.LibrarianId }, librarian);
        }

        // DELETE: api/Librarians/5
        [ResponseType(typeof(Librarian))]
        public IHttpActionResult DeleteLibrarian(int id)
        {
            Librarian librarian = db.Librarians.Find(id);
            if (librarian == null)
            {
                return NotFound();
            }

            db.Librarians.Remove(librarian);
            db.SaveChanges();

            return Ok(librarian);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool LibrarianExists(int id)
        {
            return db.Librarians.Count(e => e.LibrarianId == id) > 0;
        }
    }
}