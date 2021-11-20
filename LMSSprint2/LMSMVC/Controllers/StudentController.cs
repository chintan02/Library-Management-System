using LMSMVC.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
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
    public class StudentController : Controller
    {
        public bool SendEmail(string EmailId,string UserName)
        {
            var fromEmail = new MailAddress("panchalnikunj999@gmail.com", "Nikunj Panchal");
            var toEmail = new MailAddress(EmailId);
            var fromEmailPassword = "Nikunj@29"; // Replace with actual password

            string subject = "";
            string body = "";
            
            subject = "Your account is successfully created!";
            body = "<br/><br/>We are excited to tell you that your LMSTeam 3 account is" +
                    " successfully created. Your UserName is " +
                    " <br/><br/>"+UserName;
           
           

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
        public ActionResult Index([Bind(Exclude = "StudentImage")]Student student)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    StudentModel studentModel = new StudentModel();
                    studentModel.StudentName = student.StudentName;
                    studentModel.StudentPassword =student.StudentPassword;
                    studentModel.StudentContact = student.StudentContact;
                    studentModel.StudentEmail = student.StudentEmail;
                    
                    byte[] imageData = null;
                    if (Request.Files.Count > 0)
                    {
                        HttpPostedFileBase poImgFile = Request.Files["StudentImage"];

                        using (var binary = new BinaryReader(poImgFile.InputStream))
                        {
                            imageData = binary.ReadBytes(poImgFile.ContentLength);
                        }
                    }
                    studentModel.StudentImage = imageData;

                    using (var client = new HttpClient())
                    {
                        client.BaseAddress = new Uri("http://localhost:52503/api/");
                        //var str = $"{{\"StudentId\":\"{studId}\",\"StudentName\":\"{student.StudentName}\",\"StudentPassword\":\"{student.StudentPassword}\",\"StudentImage\":\"{rawbyte}\",\"StudentContact\":\"{student.StudentContact}\",\"StudentEmail\":\"{student.StudentEmail}\"}}";
                        //HttpContent content = new StringContent(str, Encoding.UTF8, "application/json");

                        //var responseTask = client.PostAsync("Students", content);
                        var responseTask = client.PostAsJsonAsync("Students", studentModel);
                        responseTask.Wait();

                        var result = responseTask.Result;
                        if (result.IsSuccessStatusCode)
                        {

                            IEnumerable<StudentModel> members = null;

                            using (var client1 = new HttpClient())
                            {

                                client1.BaseAddress = new Uri("http://localhost:52503/api/");

                                var responseTask1 = client1.GetAsync("Students");
                                responseTask1.Wait();

                                //To store result of web api response.   
                                var result1 = responseTask1.Result;

                                //If success received   
                                if (result1.IsSuccessStatusCode)
                                {
                                    var readTask1 = result1.Content.ReadAsAsync<IList<StudentModel>>();
                                    readTask1.Wait();
                                    members = readTask1.Result;
                                    string username = "";
                                    foreach(var item in members)
                                    {
                                        if(item.StudentName==student.StudentName && item.StudentContact==student.StudentContact && item.StudentEmail==student.StudentEmail)
                                        {
                                            username = item.UserName;
                                        }
                                    }
                                    StudentController controller = new StudentController();
                                    bool res = controller.SendEmail(student.StudentEmail, username);

                                    return RedirectToAction("Index", "Login", new { ac = "Success" });
                                }
                                else
                                {
                                    //Error response received   
                                    members = Enumerable.Empty<StudentModel>();
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
                return View(student);
            }

        }
        // GET: Student
        public ActionResult StudentHome(int StudentId)
        {
            Session["StudentId"] = StudentId;
            StudentModel obj = new StudentModel();
            using (var client1 = new HttpClient())
            {
                client1.BaseAddress = new Uri("http://localhost:52503/api/");

                var responseTask1 = client1.GetAsync("Students/" + StudentId);
                responseTask1.Wait();

                var result1 = responseTask1.Result;
                if (result1.IsSuccessStatusCode)
                {
                    var readTask1 = result1.Content.ReadAsAsync<StudentModel>();
                    readTask1.Wait();

                    obj = readTask1.Result;
                }

            }
            Session["Profilepic"] = obj.StudentImage;
            Session["StudentName"] = obj.StudentName;
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
        public ActionResult StudentBookRecommendation()
        {

            return View();
        }
        [HttpPost]
        public ActionResult StudentBookRecommendation(BookRecomendation bookRecomendation)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    using (var client = new HttpClient())
                    {
                        client.BaseAddress = new Uri("http://localhost:52503/api/");
                        int studentId = int.Parse(Session["StudentId"].ToString());
                        var str = $"{{\"BookRecomendationId\":\"{1}\",\"StudentId\":\"{studentId}\",\"BookName\":\"{bookRecomendation.BookName}\",\"BookDescription\":\"{bookRecomendation.BookDescription}\",\"BookAuthor\":\"{bookRecomendation.BookAuthor}\",\"BookPublication\":\"{bookRecomendation.BookPublication}\"}}";
                        HttpContent content = new StringContent(str, Encoding.UTF8, "application/json");

                        var responseTask = client.PostAsync("BookRecomendations", content);
                        responseTask.Wait();

                        var result = responseTask.Result;
                        if (result.IsSuccessStatusCode)
                        {
                            return RedirectToAction("StudentHome", "Student", new { StudentId = studentId });
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
                return View(bookRecomendation);
            }
        }
        [HttpGet]
        public ActionResult BookRequest(int BookId)
        {
            Book obj = new Book();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:52503/api/");

                var responseTask = client.GetAsync("Books/" + BookId);
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<Book>();
                    readTask.Wait();

                    obj = readTask.Result;
                }

            }
            return View(obj);
        }
        [HttpPost]
        public ActionResult BookRequest(Book book)
        {
            try
            {
                string status = "Pending";
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("http://localhost:52503/api/");
                    int studentId = int.Parse(Session["StudentId"].ToString());

                    var str = $"{{\"BookRequestId\":\"{1}\",\"RequestStatus\":\"{status}\",\"BookId\":\"{book.BookId}\",\"StudentId\":\"{studentId}\"}}";
                    HttpContent content = new StringContent(str, Encoding.UTF8, "application/json");

                    var responseTask = client.PostAsync("BookRequests", content);
                    responseTask.Wait();

                    var result = responseTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        return RedirectToAction("StudentHome", "Student", new { StudentId = studentId });
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
        public ActionResult ShowBookIssue()
        {

            IEnumerable<Book> book = null;

            using (var client1 = new HttpClient())
            {

                client1.BaseAddress = new Uri("http://localhost:52503/api/");

                var responseTask1 = client1.GetAsync("Books");
                responseTask1.Wait();

                //To store result of web api response.   
                var result1 = responseTask1.Result;

                //If success received   
                if (result1.IsSuccessStatusCode)
                {
                    var readTask1 = result1.Content.ReadAsAsync<IList<Book>>();
                    readTask1.Wait();
                    book = readTask1.Result;
                }
                else
                {
                    //Error response received   
                    book = Enumerable.Empty<Book>();
                }

            }

            IEnumerable<BookIssue> members = null;
            IEnumerable<BookIssue> bookissue = null;

            using (var client = new HttpClient())
            {

                client.BaseAddress = new Uri("http://localhost:52503/api/");

                var responseTask = client.GetAsync("BookIssues");
                responseTask.Wait();

                //To store result of web api response.   
                var result = responseTask.Result;

                //If success received   
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<IList<BookIssue>>();
                    readTask.Wait();
                    members = readTask.Result;
                }
                else
                {
                    //Error response received   
                    members = Enumerable.Empty<BookIssue>();
                }

                bookissue = members.Where(s => s.StudentId.ToString().Equals(Session["StudentId"].ToString()));          
            }
            List<ViewBookIssue> viewBookIssues = new List<ViewBookIssue>();
            foreach(var item in bookissue)
            {
                foreach(var item1 in book)
                {
                    if(item.BookId == item1.BookId)
                    {
                        ViewBookIssue obj1 = new ViewBookIssue();
                        obj1.BookIssueId = item.BookIssueId;
                        obj1.BookId = item.BookId;
                        obj1.LibrarianId = item.LibrarianId;
                        obj1.StudentId = item.StudentId;
                        obj1.DateOfIssue = item.DateOfIssue;
                        obj1.DateOfReturn = item.DateOfReturn;
                        obj1.DuePayment = item.DuePayment;
                        obj1.BookName = item1.BookName;
                        viewBookIssues.Add(obj1);
                    }
                }
            }
            return View(viewBookIssues);
        }
        [HttpGet]
        public ActionResult ReturnBook(int id)
        {
            BookIssue obj = new BookIssue();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:52503/api/");

                var responseTask = client.GetAsync("BookIssues/" + id);
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<BookIssue>();
                    readTask.Wait();

                    obj = readTask.Result;
                }

            }
            return View(obj);
        }

        [HttpPost]
        public ActionResult ReturnBook(int id,BookIssue bookIssue)
        {
            try
            {

                BookIssue obj = new BookIssue();
                using (var client3 = new HttpClient())
                {
                    client3.BaseAddress = new Uri("http://localhost:52503/api/");

                    var responseTask3 = client3.GetAsync("BookIssues/" + id);
                    responseTask3.Wait();

                    var result3 = responseTask3.Result;
                    if (result3.IsSuccessStatusCode)
                    {
                        var readTask3 = result3.Content.ReadAsAsync<BookIssue>();
                        readTask3.Wait();

                        obj = readTask3.Result;
                    }

                }



                Book book = new Book();
                using (var client2 = new HttpClient())
                {

                    client2.BaseAddress = new Uri("http://localhost:52503/api/");

                    var responseTask2 = client2.GetAsync("Books/" + obj.BookId);
                    responseTask2.Wait();

                    var result2 = responseTask2.Result;
                    if (result2.IsSuccessStatusCode)
                    {
                        var readTask2 = result2.Content.ReadAsAsync<Book>();
                        readTask2.Wait();

                        book = readTask2.Result;
                    }

                }


                // TODO: Add delete logic here
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("http://localhost:52503/api/");

                    var responseTask = client.DeleteAsync("BookIssues/" + id);
                    responseTask.Wait();

                    var result = responseTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        var readTask = result.Content.ReadAsAsync<BookIssue>();
                        readTask.Wait();

                        using (var client1 = new HttpClient())
                        {

                            client1.BaseAddress = new Uri("http://localhost:52503/api/");
                            string status = "available";
                            var bookstr1 = $"{{\"BookId\": \"{book.BookId}\",\"BookName\": \"{book.BookName}\",\"BookDescription\": \"{book.BookDescription}\",\"BookAuthor\": \"{book.BookAuthor}\",\"BookPublication\": \"{book.BookPublication}\",\"BookPrice\": \"{book.BookPrice}\",\"BookStatus\": \"{status}\"}}";
                            HttpContent content1 = new StringContent(bookstr1, Encoding.UTF8, "application/json");
                            var responseTask1 = client.PutAsync("Books/" + book.BookId, content1);
                            responseTask1.Wait();

                            var result1 = responseTask1.Result;
                            if (result1.IsSuccessStatusCode)
                            {

                                return RedirectToAction("ShowBookIssue");
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