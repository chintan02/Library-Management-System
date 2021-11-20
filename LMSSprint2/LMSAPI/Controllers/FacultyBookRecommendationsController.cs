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
    public class FacultyBookRecommendationsController : ApiController
    {
        private LibraryManagementSystemEntities db = new LibraryManagementSystemEntities();

        // GET: api/FacultyBookRecommendations
        public IQueryable<FacultyBookRecommendation> GetFacultyBookRecommendations()
        {
            return db.FacultyBookRecommendations;
        }

        // GET: api/FacultyBookRecommendations/5
        [ResponseType(typeof(FacultyBookRecommendation))]
        public IHttpActionResult GetFacultyBookRecommendation(int id)
        {
            FacultyBookRecommendation facultyBookRecommendation = db.FacultyBookRecommendations.Find(id);
            if (facultyBookRecommendation == null)
            {
                return NotFound();
            }

            return Ok(facultyBookRecommendation);
        }

        // PUT: api/FacultyBookRecommendations/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutFacultyBookRecommendation(int id, FacultyBookRecommendation facultyBookRecommendation)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != facultyBookRecommendation.FacultyBookRecommnedationId)
            {
                return BadRequest();
            }

            db.Entry(facultyBookRecommendation).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FacultyBookRecommendationExists(id))
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

        // POST: api/FacultyBookRecommendations
        [ResponseType(typeof(FacultyBookRecommendation))]
        public IHttpActionResult PostFacultyBookRecommendation(FacultyBookRecommendation facultyBookRecommendation)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.FacultyBookRecommendations.Add(facultyBookRecommendation);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = facultyBookRecommendation.FacultyBookRecommnedationId }, facultyBookRecommendation);
        }

        // DELETE: api/FacultyBookRecommendations/5
        [ResponseType(typeof(FacultyBookRecommendation))]
        public IHttpActionResult DeleteFacultyBookRecommendation(int id)
        {
            FacultyBookRecommendation facultyBookRecommendation = db.FacultyBookRecommendations.Find(id);
            if (facultyBookRecommendation == null)
            {
                return NotFound();
            }

            db.FacultyBookRecommendations.Remove(facultyBookRecommendation);
            db.SaveChanges();

            return Ok(facultyBookRecommendation);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool FacultyBookRecommendationExists(int id)
        {
            return db.FacultyBookRecommendations.Count(e => e.FacultyBookRecommnedationId == id) > 0;
        }
    }
}