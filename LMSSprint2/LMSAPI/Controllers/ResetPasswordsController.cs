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
    public class ResetPasswordsController : ApiController
    {
        private ResetPasswordEntities db = new ResetPasswordEntities();

        // GET: api/ResetPasswords
        public IQueryable<ResetPassword> GetResetPasswords()
        {
            return db.ResetPasswords;
        }

        // GET: api/ResetPasswords/5
        [ResponseType(typeof(ResetPassword))]
        public IHttpActionResult GetResetPassword(string id)
        {
            ResetPassword resetPassword = db.ResetPasswords.Find(id);
            if (resetPassword == null)
            {
                return NotFound();
            }

            return Ok(resetPassword);
        }

        // PUT: api/ResetPasswords/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutResetPassword(string id, ResetPassword resetPassword)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != resetPassword.ResetCode)
            {
                return BadRequest();
            }

            db.Entry(resetPassword).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ResetPasswordExists(id))
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

        // POST: api/ResetPasswords
        [ResponseType(typeof(ResetPassword))]
        public IHttpActionResult PostResetPassword(ResetPassword resetPassword)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.ResetPasswords.Add(resetPassword);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (ResetPasswordExists(resetPassword.ResetCode))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = resetPassword.ResetCode }, resetPassword);
        }

        // DELETE: api/ResetPasswords/5
        [ResponseType(typeof(ResetPassword))]
        public IHttpActionResult DeleteResetPassword(string id)
        {
            ResetPassword resetPassword = db.ResetPasswords.Find(id);
            if (resetPassword == null)
            {
                return NotFound();
            }

            db.ResetPasswords.Remove(resetPassword);
            db.SaveChanges();

            return Ok(resetPassword);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ResetPasswordExists(string id)
        {
            return db.ResetPasswords.Count(e => e.ResetCode == id) > 0;
        }
    }
}