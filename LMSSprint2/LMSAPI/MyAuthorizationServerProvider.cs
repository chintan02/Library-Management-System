using LMSAPI.Models;
using Microsoft.Owin.Security.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace LMSAPI
{
    public class MyAuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        private LibrarianEntities db = new LibrarianEntities();
        private StudentEntities sdb = new StudentEntities();
        private FacultyEntities fdb = new FacultyEntities();

        public override async Task  ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            string Role = context.Parameters
                       .Where(f => f.Key == "Role")
                       .Select(f => f.Value).SingleOrDefault()[0];
            context.OwinContext.Set<string>("Role", Role);
            context.Validated();

        }
        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            string Role = context.OwinContext.Get<string>("Role");



            var identity = new ClaimsIdentity(context.Options.AuthenticationType);
            bool found = false;




            if (Role == "Librarian")
            {
                foreach (var v in db.Librarians)
                {
                    if (context.UserName == v.UserName && context.Password == v.LibrarianPassword)
                    {



                        identity.AddClaim(new Claim(ClaimTypes.Role, "Librarian"));



                        identity.AddClaim(new Claim("username", v.UserName));
                        identity.AddClaim(new Claim(ClaimTypes.Name, v.UserName));
                        context.Validated(identity);
                        found = true;
                    }



                }
            }



            else if (Role == "Student")
            {
                foreach (var v in sdb.Students)
                {
                    if (context.UserName == v.UserName && context.Password == v.StudentPassword)
                    {



                        identity.AddClaim(new Claim(ClaimTypes.Role, "Student"));



                        identity.AddClaim(new Claim("username", v.UserName));
                        identity.AddClaim(new Claim(ClaimTypes.Name, v.UserName));
                        context.Validated(identity);
                        found = true;
                    }



                }
            }



            else if (Role == "Faculty")
            {
                foreach (var v in fdb.Faculties)
                {
                    if (context.UserName == v.UserName && context.Password == v.FacultyPassword)
                    {



                        identity.AddClaim(new Claim(ClaimTypes.Role, "Faculty"));



                        identity.AddClaim(new Claim("username", v.UserName));
                        identity.AddClaim(new Claim(ClaimTypes.Name, v.UserName));
                        context.Validated(identity);
                        found = true;
                    }



                }
            }



            if (found == false)
            {
                context.SetError("invalid_grant", "Provided UserName and Password is incorrect");
                return;
            }
            //var identity = new ClaimsIdentity(context.Options.AuthenticationType);
            //bool found = false;



            //var username = allRegister.reg_name;



            //foreach (var v in db.AllRegisters)
            //{
            //    if (context.UserName == v.reg_name && context.Password == v.reg_password)
            //    {



            //        identity.AddClaim(new Claim(ClaimTypes.Role, v.reg_type));



            //        identity.AddClaim(new Claim("username", v.reg_name));
            //        identity.AddClaim(new Claim(ClaimTypes.Name, v.reg_name));
            //        context.Validated(identity);
            //        found = true;
            //    }



            //}



            //else if (context.UserName == "user" && context.Password == "user")
            //{
            //    identity.AddClaim(new Claim(ClaimTypes.Role, "user"));
            //    identity.AddClaim(new Claim("username", "user"));
            //    identity.AddClaim(new Claim(ClaimTypes.Name, "Yash"));
            //    context.Validated(identity);
            //}
            //if (found == false)
            //{
            //    context.SetError("invalid_grant", "Provided UserName and Password is incorrect");
            //    return;
            //}
        }
    }
}