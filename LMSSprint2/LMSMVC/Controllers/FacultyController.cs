using LMSMVC.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace LMSMVC.Controllers
{
    public class FacultyController : Controller
    {
        public bool SendEmail(string EmailId, string UserName)
        {
            var fromEmail = new MailAddress("panchalnikunj999@gmail.com", "Nikunj Panchal");
            var toEmail = new MailAddress(EmailId);
            var fromEmailPassword = "Nikunj@29"; // Replace with actual password

            string subject = "";
            string body = "";

            subject = "Your account is successfully created!";
            body = "<br/><br/>We are excited to tell you that your LMSTeam 3 account is" +
                    " successfully created. Your UserName is " +
                    " <br/><br/>" + UserName;

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

            return true;
        }
        public ActionResult Index()
        {
            return View();
        }
        //api/Students
        [HttpPost]
        public ActionResult Index([Bind(Exclude = "FacultyImage")] Faculty faculty)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    FacultyModel facultyModel = new FacultyModel();
                    facultyModel.FacultyName = faculty.FacultyName;
                    facultyModel.FacultyPassword = faculty.FacultyPassword;
                    facultyModel.FacultyContact = faculty.FacultyContact;
                    facultyModel.FacultyEmail = faculty.FacultyEmail;


                    byte[] imageData = null;
                    if (Request.Files.Count > 0)
                    {
                        HttpPostedFileBase poImgFile = Request.Files["FacultyImage"];
                        using (var binary = new BinaryReader(poImgFile.InputStream))
                        {
                            imageData = binary.ReadBytes(poImgFile.ContentLength);

                        }
                    }
                    facultyModel.FacultyImage = imageData;

                    using (var client = new HttpClient())
                    {
                        client.BaseAddress = new Uri("http://localhost:52503/api/");
                        int facId = 1;
                        //var str = $"{{\"FacultyId\":\"{facId}\",\"FacultyName\":\"{faculty.FacultyName}\",\"FacultyPassword\":\"{faculty.FacultyPassword}\",\"FacultyImage\":\"{faculty.FacultyImage}\",\"FacultyContact\":\"{faculty.FacultyContact}\",\"FacultyEmail\":\"{faculty.FacultyEmail}\"}}";
                        //HttpContent content = new StringContent(str, Encoding.UTF8, "application/json");

                        //var responseTask = client.PostAsync("Faculties", content);
                        var responseTask = client.PostAsJsonAsync("Faculties", facultyModel);
                        responseTask.Wait();


                        var result = responseTask.Result;
                        if (result.IsSuccessStatusCode)
                        {
                            IEnumerable<FacultyModel> members = null;

                            using (var client1 = new HttpClient())
                            {

                                client1.BaseAddress = new Uri("http://localhost:52503/api/");

                                var responseTask1 = client1.GetAsync("Faculties");
                                responseTask1.Wait();

                                //To store result of web api response.   
                                var result1 = responseTask1.Result;

                                //If success received   
                                if (result1.IsSuccessStatusCode)
                                {
                                    var readTask1 = result1.Content.ReadAsAsync<IList<FacultyModel>>();
                                    readTask1.Wait();
                                    members = readTask1.Result;
                                    string username = "";
                                    foreach (var item in members)
                                    {
                                        if (item.FacultyName == faculty.FacultyName && item.FacultyContact == faculty.FacultyContact && item.FacultyEmail == faculty.FacultyEmail)
                                        {
                                            username = item.UserName;
                                        }
                                    }
                                    StudentController controller = new StudentController();
                                    bool res = controller.SendEmail(faculty.FacultyEmail, username);


                                    return RedirectToAction("Index", "Login", new { ac = "Success" });
                                }
                                else
                                {
                                    return View();
                                }
                            }
                        }
                        else
                        {
                            return View();
                        }
                    }


                }
                catch
                {
                    return View();
                }
            }
            else
            {
                return View(faculty);
            }
        }
        //api/Students

        // GET: Faculty
        public ActionResult FacultyHome(int FacultyId)
        {
            Session["FacultyId"] = FacultyId;

            FacultyModel obj = new FacultyModel();
            using (var client1 = new HttpClient())
            {
                client1.BaseAddress = new Uri("http://localhost:52503/api/");

                var responseTask1 = client1.GetAsync("Faculties/" + FacultyId);
                responseTask1.Wait();

                var result1 = responseTask1.Result;
                if (result1.IsSuccessStatusCode)
                {
                    var readTask1 = result1.Content.ReadAsAsync<FacultyModel>();
                    readTask1.Wait();

                    obj = readTask1.Result;
                }

            }
            Session["Profilepic"] = obj.FacultyImage;
            Session["FacultyName"] = obj.FacultyName;
            IEnumerable<Book> members = null;
            using (var client = new HttpClient())
            {

                client.BaseAddress = new Uri("http://localhost:52503/api/");

                var responseTask = client.GetAsync("Books");
                responseTask.Wait();

                //To store result of web api response.   
                var result = responseTask.Result;

                //If success received   
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<IList<Book>>();
                    readTask.Wait();
                    members = readTask.Result;
                }
                else
                {
                    //Error response received   
                    members = Enumerable.Empty<Book>();
                }

                return View(members);
            }
        }
        [HttpGet]
        public ActionResult FacultyBookRecommendation()
        {
            return View();
        }
        [HttpPost]
        public ActionResult FacultyBookRecommendation(FacultyBookRecommendation facultyBookRecommendation)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    using (var client = new HttpClient())
                    {
                        client.BaseAddress = new Uri("http://localhost:52503/api/");
                        facultyBookRecommendation.FacultyBookRecommnedationId = 1;
                        var str = $"{{\"FacultyBookRecommnedationId\":\"{facultyBookRecommendation.FacultyBookRecommnedationId}\",\"FacultyId\":\"{Session["FacultyId"]}\",\"BookName\":\"{facultyBookRecommendation.BookName}\",\"BookDescription\":\"{facultyBookRecommendation.BookDescription}\",\"BookAuthor\":\"{facultyBookRecommendation.BookAuthor}\",\"BookPublication\":\"{facultyBookRecommendation.BookPublication}\"}}";
                        HttpContent content = new StringContent(str, Encoding.UTF8, "application/json");

                        var responseTask = client.PostAsync("FacultyBookRecommendations", content);
                        responseTask.Wait();

                        var result = responseTask.Result;
                        if (result.IsSuccessStatusCode)
                        {
                            return RedirectToAction("FacultyHome", "Faculty", new { FacultyId = Session["FacultyId"] });
                        }
                        else
                        {
                            return View();
                        }
                    }
                    // TODO: Add insert logic here

                }
                catch
                {
                    return View();
                }
            }
            else
            {
                return View(facultyBookRecommendation);
            }


        }
        public ActionResult Search(string search)
        {
            List<Book> books = new List<Book>();

            var book = books.Where(s => s.BookName.StartsWith(search));

            return PartialView(book);
        }
        public ActionResult Logout()
        {
            if (Request.Cookies["token"] != null)
            {
                HttpCookie myCookie = new HttpCookie("token");
                myCookie.Expires = DateTime.Now.AddDays(-1d);
                Response.Cookies.Add(myCookie);
            }
            Session.Abandon();
            Session.Clear();
            return RedirectToAction("Index", "Login");
        }
        public FileContentResult UserPhotos()
        {
            return new FileContentResult((byte[])Session["Profilepic"], "image/jpeg");
        }
    }
}