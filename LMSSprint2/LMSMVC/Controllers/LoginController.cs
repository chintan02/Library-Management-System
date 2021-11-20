using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text.RegularExpressions;
using System.Net.Http;
using LMSMVC.Models;
using System.Net.Http.Formatting;
using System.Web.Helpers;
using System.Net.Mail;
using System.Net;
using System.Text;

namespace LMSMVC.Controllers
{
    public class LoginController : Controller
    {
        //[Route("LoginUser")]
        // GET: Login
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Index(LoginModel loginModel)
        {
            if (String.IsNullOrEmpty(loginModel.UserName) || String.IsNullOrEmpty(loginModel.UserPassword))
            {
                ViewBag.message = "Login Credentials are Invalid or Empty !!!";
                return View();
            }

            string checkStudent = @"SH[0-9]+";
            string checkLibrarian = @"LB[0-9]+";
            string checkFaculty = @"FC[0-9]+";
            string str = loginModel.UserName.ToString();
            char[] charArr = loginModel.UserName.ToCharArray();
            int loginid = 0;
            for (int i = 2; i < charArr.Length; i++)
            {
                if (Char.IsDigit(charArr[i]))
                {
                    loginid = (loginid * 10) + int.Parse(charArr[i].ToString());
                }
            }



            if (Regex.IsMatch(str, checkStudent))
            {
                Student obj = new Student();
                using (var client = new HttpClient())
                {
                    var form = new Dictionary<string, string>
                        {
                           {"grant_type", "password"},
                           {"username", loginModel.UserName.ToString()},
                           {"password",  loginModel.UserPassword},
                           {"Role", "Student"}
                       };
                    client.BaseAddress = new Uri("http://localhost:52503/");



                    HttpResponseMessage tokenResponse = client.PostAsync("token", new FormUrlEncodedContent(form)).Result;
                    if (tokenResponse.IsSuccessStatusCode)
                    {
                        var tokenKey = tokenResponse.Content.ReadAsAsync<Token>(new[] { new JsonMediaTypeFormatter() }).Result;
                        HttpCookie MyCookie = new HttpCookie("token");
                        DateTime now = DateTime.Now;
                        MyCookie["tokenkey"] = tokenKey.AccessToken;
                        // MyCookie.Value = tokenKey.ToString();
                        MyCookie.Expires = now.AddHours(1);



                        Response.Cookies.Add(MyCookie);


                    }



                    HttpResponseMessage StudentDetailsResponse = client.GetAsync("api/Students/" + loginid).Result;
                    if (StudentDetailsResponse.IsSuccessStatusCode)
                    {
                        obj = StudentDetailsResponse.Content.ReadAsAsync<Student>().Result;

                    }




                }
                //if (Crypto.VerifyHashedPassword(obj.StudentPassword, loginModel.UserPassword)
                if (obj.StudentPassword== loginModel.UserPassword )
                {
                    return RedirectToAction("StudentHome", "Student", new { StudentId = loginid });
                }
                else
                {
                    ViewBag.message = "Login Credentials are Invalid !!!";
                    return View();
                }



            }
            else if (Regex.IsMatch(str, checkLibrarian))
            {
                Librarian obj = new Librarian();
                using (var client = new HttpClient())
                {
                    var form = new Dictionary<string, string>
                        {
                           {"grant_type", "password"},
                           {"username", loginModel.UserName.ToString()},
                           {"password", loginModel.UserPassword},
                           {"Role", "Librarian"}
                       };
                    client.BaseAddress = new Uri("http://localhost:52503/");



                    HttpResponseMessage tokenResponse = client.PostAsync("token", new FormUrlEncodedContent(form)).Result;
                    if (tokenResponse.IsSuccessStatusCode)
                    {
                        var tokenKey = tokenResponse.Content.ReadAsAsync<Token>(new[] { new JsonMediaTypeFormatter() }).Result;
                        HttpCookie MyCookie = new HttpCookie("token");
                        DateTime now = DateTime.Now;
                        MyCookie["tokenkey"] = tokenKey.AccessToken;
                        // MyCookie.Value = tokenKey.ToString();
                        MyCookie.Expires = now.AddHours(1);



                        Response.Cookies.Add(MyCookie);



                    }



                    HttpResponseMessage LibrarianDetailsResponse = client.GetAsync("api/Librarians/" + loginid).Result;
                    if (LibrarianDetailsResponse.IsSuccessStatusCode)
                    {
                        obj = LibrarianDetailsResponse.Content.ReadAsAsync<Librarian>().Result;



                    }




                }
                if (obj.LibrarianPassword==loginModel.UserPassword)
                {
                    return RedirectToAction("LibrarianHome", "Librarian", new { LibrarianId = loginid });
                }
                else
                {
                    ViewBag.message = "Login Credentials are Invalid !!!";
                    return View();
                }




            }
            else if (Regex.IsMatch(str, checkFaculty))
            {
                Faculty obj = new Faculty();
                using (var client = new HttpClient())
                {
                    var form = new Dictionary<string, string>
                        {
                           {"grant_type", "password"},
                           {"username", loginModel.UserName.ToString()},
                           {"password", loginModel.UserPassword},
                           {"Role", "Faculty"}
                       };
                    client.BaseAddress = new Uri("http://localhost:52503/");



                    HttpResponseMessage tokenResponse = client.PostAsync("token", new FormUrlEncodedContent(form)).Result;
                    if (tokenResponse.IsSuccessStatusCode)
                    {
                        var tokenKey = tokenResponse.Content.ReadAsAsync<Token>(new[] { new JsonMediaTypeFormatter() }).Result;
                        HttpCookie MyCookie = new HttpCookie("token");
                        DateTime now = DateTime.Now;
                        MyCookie["tokenkey"] = tokenKey.AccessToken;
                        // MyCookie.Value = tokenKey.ToString();
                        MyCookie.Expires = now.AddHours(1);



                        Response.Cookies.Add(MyCookie);





                    }
                    HttpResponseMessage FacultyDetailsResponse = client.GetAsync("api/Faculties/" + loginid).Result;
                    if (FacultyDetailsResponse.IsSuccessStatusCode)
                    {
                        obj = FacultyDetailsResponse.Content.ReadAsAsync<Faculty>().Result;



                    }
                }

                if (obj.FacultyPassword==loginModel.UserPassword)
                {
                    return RedirectToAction("FacultyHome", "Faculty", new { FacultyId = loginid });
                }
                else
                {
                    ViewBag.message = "Login Credentials are Invalid !!!";
                    return View();
                }
            }
            return View();
            //if (ModelState.IsValid)
            //{
            //    string checkStudent = @"SH[0-9]+";
            //    string checkLibrarian = @"LB[0-9]+";
            //    string checkFaculty = @"FC[0-9]+";
            //    string str = loginModel.UserName.ToString();
            //    char[] charArr = loginModel.UserName.ToCharArray();
            //    int loginid = 0;
            //    for (int i = 2; i < charArr.Length; i++)
            //    {
            //        if (Char.IsDigit(charArr[i]))
            //        {
            //            loginid = (loginid * 10) + int.Parse(charArr[i].ToString());
            //        }
            //    }


            //    if (Regex.IsMatch(str, checkStudent))
            //    {
            //        Student obj = new Student();
            //        using (var client = new HttpClient())
            //        {
            //            client.BaseAddress = new Uri("http://localhost:52503/api/");

            //            var responseTask = client.GetAsync("Students/" + loginid);
            //            responseTask.Wait();

            //            var result = responseTask.Result;
            //            if (result.IsSuccessStatusCode)
            //            {
            //                var readTask = result.Content.ReadAsAsync<Student>();
            //                readTask.Wait();

            //                obj = readTask.Result;

            //                //Console.WriteLine(readTask.Result.Name);
            //            }

            //        }
            //        if (obj.StudentPassword == loginModel.UserPassword)
            //        {
            //            return RedirectToAction("StudentHome", "Student", new { StudentId = loginid });
            //        }

            //    }
            //    else if (Regex.IsMatch(str, checkLibrarian))
            //    {
            //        Librarian obj = new Librarian();
            //        using (var client = new HttpClient())
            //        {
            //            client.BaseAddress = new Uri("http://localhost:52503/api/");

            //            var responseTask = client.GetAsync("Librarians/" + loginid);
            //            responseTask.Wait();

            //            var result = responseTask.Result;
            //            if (result.IsSuccessStatusCode)
            //            {
            //                var readTask = result.Content.ReadAsAsync<Librarian>();
            //                readTask.Wait();

            //                obj = readTask.Result;

            //                //Console.WriteLine(readTask.Result.Name);
            //            }

            //        }
            //        if (obj.LibrarianPassword == loginModel.UserPassword)
            //        {
            //            return RedirectToAction("LibrarianHome", "Librarian", new { LibrarianId = loginid });
            //        }
            //    }
            //    else if (Regex.IsMatch(str, checkFaculty))
            //    {
            //        Faculty obj = new Faculty();
            //        using (var client = new HttpClient())
            //        {
            //            client.BaseAddress = new Uri("http://localhost:52503/api/");

            //            var responseTask = client.GetAsync("Faculties/" + loginid);
            //            responseTask.Wait();

            //            var result = responseTask.Result;
            //            if (result.IsSuccessStatusCode)
            //            {
            //                var readTask = result.Content.ReadAsAsync<Faculty>();
            //                readTask.Wait();

            //                obj = readTask.Result;

            //                //Console.WriteLine(readTask.Result.Name);
            //            }

            //        }
            //        if (obj.FacultyPassword == loginModel.UserPassword)
            //        {
            //            return RedirectToAction("FacultyHome", "Faculty", new { FacultyId = loginid });
            //        }
            //    }
            //    return View();
            //}
            //else
            //{
            //    return View(loginModel);
            //}
        }

        //[Route("Login")]
        public ActionResult Login()
        {
            return View();
        }


        [NonAction]
        public void SendVerificationLinkEmail(string EmailId, string resetCode, string emailFor = "ResetPassword")
        {
            var verifyUrl = "/Login/" + emailFor;
            var link = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, verifyUrl);

            var fromEmail = new MailAddress("panchalnikunj999@gmail.com", "Nikunj Panchal");
            var toEmail = new MailAddress(EmailId);
            var fromEmailPassword = "Nikunj@29"; // Replace with actual password

            string subject = "";
            string body = "";
            if (emailFor == "VerifyAccount")
            {
                subject = "Your account is successfully created!";
                body = "<br/><br/>We are excited to tell you that your Dotnet Awesome account is" +
                    " successfully created. Please click on the below link to verify your account" +
                    " <br/><br/><a href='" + link + "'>" + link + "</a> ";
            }
            else if (emailFor == "ResetPassword")
            {
                subject = "Reset Password";
                body = "Hi,<br/>We got request to reset your account password. Please click on the below link to Reset." +
                    "<br/>This is your Activation Code: " + resetCode + "<br/>Use this below link to Reset your password<br/><a href=" + link + ">Reset Password link</a>";
            }

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromEmail.Address, fromEmailPassword),
                EnableSsl = true
            };

            using (var message = new MailMessage(fromEmail, toEmail)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            })
                smtp.Send(message);
        }
        [HttpGet]
        public ActionResult ForgotPassword()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ForgotPassword(string UserName)
        {
            string EmailId = "";
            string checkStudent = @"SH[0-9]+";
            string checkLibrarian = @"LB[0-9]+";
            string checkFaculty = @"FC[0-9]+";
            string str = UserName;
            char[] charArr = UserName.ToCharArray();
            int loginid = 0;
            for (int i = 2; i < charArr.Length; i++)
            {
                if (Char.IsDigit(charArr[i]))
                {
                    loginid = (loginid * 10) + int.Parse(charArr[i].ToString());
                }
            }
            if (Regex.IsMatch(UserName, checkStudent))
            {
                Student student = new Student();
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("http://localhost:52503/api/");
                    var responseTask = client.GetAsync("Students/" + loginid);
                    responseTask.Wait();
                    var result = responseTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        var readTask = result.Content.ReadAsAsync<Student>();
                        readTask.Wait();
                        student = readTask.Result;
                    }
                }
                EmailId = student.StudentEmail;




            }
            else if (Regex.IsMatch(UserName, checkLibrarian))
            {
                Librarian librarian = new Librarian();
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("http://localhost:52503/api/");
                    var responseTask = client.GetAsync("Librarians/" + loginid);
                    responseTask.Wait();
                    var result = responseTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        var readTask = result.Content.ReadAsAsync<Librarian>();
                        readTask.Wait();
                        librarian = readTask.Result;
                    }
                    EmailId = librarian.LibrarianEmail;

                }
            }
            else if (Regex.IsMatch(UserName, checkFaculty))
            {
                Faculty faculty = new Faculty();
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("http://localhost:52503/api/");
                    var responseTask = client.GetAsync("Faculties/" + loginid);
                    responseTask.Wait();
                    var result = responseTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        var readTask = result.Content.ReadAsAsync<Faculty>();
                        readTask.Wait();
                        faculty = readTask.Result;
                    }
                    EmailId = faculty.FacultyEmail;

                }
            }
            string resetCode = Guid.NewGuid().ToString();
            SendVerificationLinkEmail(EmailId, resetCode, "ResetPassword");

            using (var client1 = new HttpClient())
            {
                client1.BaseAddress = new Uri("http://localhost:52503/api/");


                var str1 = $"{{\"ResetCode\":\"{resetCode}\",\"UserName\":\"{UserName}\",\"EmailId\":\"{EmailId}\"}}";
                HttpContent content1 = new StringContent(str1, Encoding.UTF8, "application/json");

                var responseTask1 = client1.PostAsync("ResetPasswords", content1);
                responseTask1.Wait();

                var result1 = responseTask1.Result;
                if (result1.IsSuccessStatusCode)
                {
                    string message = "Reset password link has been sent to your email id.";
                    ViewBag.Message = message;
                    return RedirectToAction("ResetPassword", "Login");
                }
                else
                {
                    return View();
                }

            }

            //Session["resetPasswordCode"] = resetCode;
            
        }
        [HttpGet]
        public ActionResult ResetPassword()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPassword(ResetPasswordModel resetPassword)
        {
            if (resetPassword.ConfirmPassword == resetPassword.Password)
            {
                var message = "";
                //string checkstr = Session["resetPasswordCode"].ToString();
                ResetPasswordModel resetPasswordModel = new ResetPasswordModel();
                using (var client2 = new HttpClient())
                {
                    client2.BaseAddress = new Uri("http://localhost:52503/api/");
                    var responseTask2 = client2.GetAsync("ResetPasswords/" + resetPassword.ResetCode);
                    responseTask2.Wait();
                    var result2 = responseTask2.Result;
                    if (result2.IsSuccessStatusCode)
                    {
                        var readTask2 = result2.Content.ReadAsAsync<ResetPasswordModel>();
                        readTask2.Wait();
                        resetPasswordModel = readTask2.Result;

                        string checkStudent = @"SH[0-9]+";
                        string checkLibrarian = @"LB[0-9]+";
                        string checkFaculty = @"FC[0-9]+";
                        string modelUserName = resetPasswordModel.UserName.ToString();
                        char[] charArr = resetPasswordModel.UserName.ToString().ToCharArray();
                        int loginid = 0;
                        for (int i = 2; i < charArr.Length; i++)
                        {
                            if (Char.IsDigit(charArr[i]))
                            {
                                loginid = (loginid * 10) + int.Parse(charArr[i].ToString());
                            }
                        }
                        if (Regex.IsMatch(modelUserName, checkStudent))
                        {
                            StudentModel student = new StudentModel();
                            using (var client1 = new HttpClient())
                            {
                                client1.BaseAddress = new Uri("http://localhost:52503/api/");
                                var responseTask1 = client1.GetAsync("Students/" + loginid);
                                responseTask1.Wait();
                                var result1 = responseTask1.Result;
                                if (result1.IsSuccessStatusCode)
                                {
                                    var readTask1 = result1.Content.ReadAsAsync<StudentModel>();
                                    readTask1.Wait();
                                    student = readTask1.Result;
                                }
                            }

                            using (var client = new HttpClient())
                            {
                                client.BaseAddress = new Uri("http://localhost:52503/api/");
                                student.StudentPassword = resetPassword.Password;
                                //var strStudent = $"{{\"StudentId\": \"{student.StudentId}\",\"StudentName\": \"{student.StudentName}\",\"StudentPassword\": \"{resetPassword.Password}\",\"StudentImage\": \"{student.StudentImage}\",\"StudentContact\": \"{student.StudentContact}\",\"StudentEmail\": \"{student.StudentEmail}\"}}";
                                //HttpContent content = new StringContent(strStudent, Encoding.UTF8, "application/json");
                                var responseTask = client.PutAsJsonAsync("Students/" + loginid, student);
                                responseTask.Wait();

                                var result = responseTask.Result;
                                if (result.IsSuccessStatusCode)
                                {
                                    message = "Password Updated Successfully";
                                    return RedirectToAction("Index", "Login");
                                }
                                else
                                {
                                    message = "Reset Failed to Reset Password";
                                    return View();
                                }
                            }
                        }
                        else if (Regex.IsMatch(modelUserName, checkLibrarian))
                        {
                            Librarian librarian = new Librarian();
                            using (var client = new HttpClient())
                            {
                                client.BaseAddress = new Uri("http://localhost:52503/api/");
                                var responseTask = client.GetAsync("Librarians/" + loginid);
                                responseTask.Wait();
                                var result = responseTask.Result;
                                if (result.IsSuccessStatusCode)
                                {
                                    var readTask = result.Content.ReadAsAsync<Librarian>();
                                    readTask.Wait();
                                    librarian = readTask.Result;
                                }
                            }

                            using (var client = new HttpClient())
                            {
                                client.BaseAddress = new Uri("http://localhost:52503/api/");
                                librarian.LibrarianPassword = resetPassword.Password;
                                //var strLibrarian = $"{{\"LibrarianId\":\"{librarian.LibrarianId}\",\"LibrarianName\":\"{librarian.LibrarianName}\",\"LibrarianPassword\":\"{resetPassword.Password}\",\"LibrarianImage\":\"{librarian.LibrarianImage}\",\"LibrarianContact\":\"{librarian.LibrarianContact}\",\"LibrarianEmail\":\"{librarian.LibrarianEmail}\"}}";
                                //HttpContent content = new StringContent(strLibrarian, Encoding.UTF8, "application/json");
                                var responseTask = client.PutAsJsonAsync("Librarians/" + loginid, librarian);
                                responseTask.Wait();

                                var result = responseTask.Result;
                                if (result.IsSuccessStatusCode)
                                {
                                    message = "Password Updated Successfully";
                                    return RedirectToAction("Index", "Login");
                                }
                                else
                                {
                                    message = "Reset Failed to Reset Password";
                                    return View();
                                }
                            }
                        }
                        else if (Regex.IsMatch(modelUserName, checkFaculty))
                        {
                            Faculty faculty = new Faculty();
                            using (var client = new HttpClient())
                            {
                                client.BaseAddress = new Uri("http://localhost:52503/api/");
                                var responseTask = client.GetAsync("Faculties/" + loginid);
                                responseTask.Wait();
                                var result = responseTask.Result;
                                if (result.IsSuccessStatusCode)
                                {
                                    var readTask = result.Content.ReadAsAsync<Faculty>();
                                    readTask.Wait();
                                    faculty = readTask.Result;
                                }
                            }

                            using (var client = new HttpClient())
                            {
                                client.BaseAddress = new Uri("http://localhost:52503/api/");
                                faculty.FacultyPassword = resetPassword.Password;
                                //var strFaculty = $"{{\"FacultyId\":\"{faculty.FacultyId}\",\"FacultyName\":\"{faculty.FacultyName}\",\"FacultyPassword\":\"{resetPassword.Password}\",\"FacultyImage\":\"{faculty.FacultyImage}\",\"FacultyContact\":\"{faculty.FacultyContact}\",\"FacultyEmail\":\"{faculty.FacultyEmail}\"}}";
                                //HttpContent content = new StringContent(strFaculty, Encoding.UTF8, "application/json");
                                var responseTask = client.PutAsJsonAsync("Faculties/" + loginid, faculty);
                                responseTask.Wait();

                                var result = responseTask.Result;
                                if (result.IsSuccessStatusCode)
                                {
                                    message = "Password Updated Successfully";
                                    return RedirectToAction("Index", "Login");
                                }
                                else
                                {
                                    message = "Reset Failed to Reset Password";
                                    return View();
                                }
                            }
                        }
                    }
                    else
                    {
                        return View();
                    }
                }
                ViewBag.Message = message;
                return View();
            }
            else
            {
                string message = "Password and confirm password should match";
                ViewBag.Message = message;
                return View();
            }
        }


    }
}