using LMSMVC.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace LMSMVC.Controllers
{
    public class LibrarianController : Controller
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
        // GET: Librarian
        public ActionResult Index()
        {
            return View();
        }
        //api/Students
        [HttpPost]
        public ActionResult Index([Bind(Exclude = "LibrarianImage")] Librarian librarian)
        {
            try
            {
                LibrarianModel librarianModel = new LibrarianModel();
                librarianModel.LibrarianName = librarian.LibrarianName;
                librarianModel.LibrarianPassword =  librarian.LibrarianPassword;
                librarianModel.LibrarianContact = librarian.LibrarianContact;
                librarianModel.LibrarianEmail = librarian.LibrarianEmail;

                byte[] imageData = null;
                if (Request.Files.Count > 0)
                {
                    HttpPostedFileBase poImgFile = Request.Files["LibrarianImage"];

                    using (var binary = new BinaryReader(poImgFile.InputStream))
                    {
                        imageData = binary.ReadBytes(poImgFile.ContentLength);

                    }
                }
                librarianModel.LibrarianImage = imageData;

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("http://localhost:52503/api/");
                    int studId = 1;
                    //var str = $"{{\"LibrarianId\":\"{studId}\",\"LibrarianName\":\"{librarian.LibrarianName}\",\"LibrarianPassword\":\"{librarian.LibrarianPassword}\",\"LibrarianImage\":\"{librarian.LibrarianImage}\",\"LibrarianContact\":\"{librarian.LibrarianContact}\",\"LibrarianEmail\":\"{librarian.LibrarianEmail}\"}}";
                    //HttpContent content = new StringContent(str, Encoding.UTF8, "application/json");

                    //var responseTask = client.PostAsync("Librarians", content);
                    var responseTask = client.PostAsJsonAsync("Librarians", librarianModel);
                    responseTask.Wait();

                    var result = responseTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        IEnumerable<LibrarianModel> members = null;

                        using (var client1 = new HttpClient())
                        {

                            client1.BaseAddress = new Uri("http://localhost:52503/api/");

                            var responseTask1 = client1.GetAsync("Librarians");
                            responseTask1.Wait();

                            //To store result of web api response.   
                            var result1 = responseTask1.Result;

                            //If success received   
                            if (result1.IsSuccessStatusCode)
                            {
                                var readTask1 = result1.Content.ReadAsAsync<IList<LibrarianModel>>();
                                readTask1.Wait();
                                members = readTask1.Result;
                                string username = "";
                                foreach (var item in members)
                                {
                                    if (item.LibrarianName == librarian.LibrarianName && item.LibrarianContact == librarian.LibrarianContact && item.LibrarianEmail == librarian.LibrarianEmail)
                                    {
                                        username = item.UserName;
                                    }
                                }
                                StudentController controller = new StudentController();
                                bool res = controller.SendEmail(librarian.LibrarianEmail, username);


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
        public ActionResult LibrarianHome(int LibrarianId)
        {
            Session["LibrarianId"] = LibrarianId;
            LibrarianModel obj = new LibrarianModel();


            IEnumerable<Student> student = null;

            using (var client3 = new HttpClient())
            {

                client3.BaseAddress = new Uri("http://localhost:52503/api/");

                var responseTask3 = client3.GetAsync("Students");
                responseTask3.Wait();

                //To store result of web api response.   
                var result3 = responseTask3.Result;

                //If success received   
                if (result3.IsSuccessStatusCode)
                {
                    var readTask3 = result3.Content.ReadAsAsync<IList<Student>>();
                    readTask3.Wait();
                    student = readTask3.Result;
                }
                else
                {
                    //Error response received   
                    student = Enumerable.Empty<Student>();
                }

                
            }


            IEnumerable<Book> book = null;

            using (var client2 = new HttpClient())
            {

                client2.BaseAddress = new Uri("http://localhost:52503/api/");

                var responseTask2 = client2.GetAsync("Books");
                responseTask2.Wait();

                //To store result of web api response.   
                var result2 = responseTask2.Result;

                //If success received   
                if (result2.IsSuccessStatusCode)
                {
                    var readTask2 = result2.Content.ReadAsAsync<IList<Book>>();
                    readTask2.Wait();
                    book = readTask2.Result;
                }
                else
                {
                    //Error response received   
                    book = Enumerable.Empty<Book>();
                }

            }


                using (var client1 = new HttpClient())
            {
                client1.BaseAddress = new Uri("http://localhost:52503/api/");

                var responseTask1 = client1.GetAsync("Librarians/" + LibrarianId);
                responseTask1.Wait();

                var result1 = responseTask1.Result;
                if (result1.IsSuccessStatusCode)
                {
                    var readTask1 = result1.Content.ReadAsAsync<LibrarianModel>();
                    readTask1.Wait();

                    obj = readTask1.Result;
                }

            }
            Session["Profilepic"] = obj.LibrarianImage;
            Session["LibrarianName"] = obj.LibrarianName;


            IEnumerable<BookRequest> members = null;
            IEnumerable<BookRequest> bookreq = null;

            using (var client = new HttpClient())
            {

                client.BaseAddress = new Uri("http://localhost:52503/api/");

                var responseTask = client.GetAsync("BookRequests");
                responseTask.Wait();

                //To store result of web api response.   
                var result = responseTask.Result;

                //If success received   
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<IList<BookRequest>>();
                    readTask.Wait();
                    members = readTask.Result;
                }
                else
                {
                    //Error response received   
                    members = Enumerable.Empty<BookRequest>();
                }

                bookreq = members.Where(s => s.RequestStatus.Equals("Pending"));
                
            }
            List<ViewBookReq> viewBookReqs = new List<ViewBookReq>();
            foreach(var item in bookreq)
            {
                ViewBookReq obj1 = new ViewBookReq();
                foreach(var item1 in student)
                {
                    if(item.StudentId == item1.StudentId)
                    {
                        obj1.BookRequestId = item.BookRequestId;
                        obj1.BookId = item.BookId;
                        obj1.StudentId = item.StudentId;
                        obj1.StudentName = item1.StudentName;
                        obj1.RequestStatus = item.RequestStatus;
                    }
                }
                foreach(var item1 in book)
                {
                    if(item.BookId == item1.BookId)
                    {
                        obj1.BookName = item1.BookName;
                        obj1.BookStatus = item1.BookStatus;
                    }
                }
                viewBookReqs.Add(obj1);
            }

            return View(viewBookReqs);

        }
        [HttpGet]
        public ActionResult AcceptBookRequest(int id)
        {
            BookRequest obj = new BookRequest();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:52503/api/");

                var responseTask = client.GetAsync("BookRequests/" + id);
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<BookRequest>();
                    readTask.Wait();

                    obj = readTask.Result;

                    //Console.WriteLine(readTask.Result.Name);
                }

            }
            return View(obj);
        }

        [HttpPost]
        public ActionResult AcceptBookRequest(BookRequest bookRequest)
        {
            try
            {
                string status = "Accepted";
                string Bookstatus = "Occupied";

                Book book = new Book();
                using (var client3 = new HttpClient())
                {
                    client3.BaseAddress = new Uri("http://localhost:52503/api/");

                    var responseTask3 = client3.GetAsync("Books/" + bookRequest.BookId);
                    responseTask3.Wait();

                    var result3 = responseTask3.Result;
                    if (result3.IsSuccessStatusCode)
                    {
                        var readTask3 = result3.Content.ReadAsAsync<Book>();
                        readTask3.Wait();

                        book = readTask3.Result;
                    }
                }


                    using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("http://localhost:52503/api/");

                    var str = $"{{\"BookRequestId\":\"{bookRequest.BookRequestId}\",\"RequestStatus\":\"{status}\",\"BookId\":\"{bookRequest.BookId}\",\"StudentId\":\"{bookRequest.StudentId}\"}}";
                    HttpContent content = new StringContent(str, Encoding.UTF8, "application/json");

                    var responseTask = client.PutAsync("BookRequests/" + bookRequest.BookRequestId, content);
                    responseTask.Wait();

                    var result = responseTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        using (var client1 = new HttpClient())
                        {
                            client1.BaseAddress = new Uri("http://localhost:52503/api/");
                            string issuedate = DateTime.Now.Date.ToString("MM/dd/yyyy");
                            string returnDate = DateTime.Now.Date.AddDays(15).ToString("MM/dd/yyyy");
                            int LibId = int.Parse(Session["LibrarianId"].ToString());
                            var bookstr = $"{{\"BookIssueId\":\"{1}\",\"BookRequestId\":\"{bookRequest.BookRequestId}\",\"BookId\":\"{bookRequest.BookId}\",\"StudentId\":\"{bookRequest.StudentId}\",\"LibrarianId\":\"{LibId }\",\"DateOfIssue\":\"{issuedate}\",\"DateOfReturn\":\"{returnDate}\",\"DuePayment\":\"{50}\"}}";
                            //var empstr = "{\"Id\":" +  txtId.Text + ",\"Name\": \"Harsh\",\"Basic\": 22345,\"DeptNo\": 20}";
                            HttpContent content1 = new StringContent(bookstr, Encoding.UTF8, "application/json");

                            //GetAsync to send a GET    
                            //PutAsync to send a PUT   
                            //DeleteAsync to send a DELETE   
                            var responseTask1 = client1.PostAsync("BookIssues", content1);
                            responseTask1.Wait();

                            var result1 = responseTask1.Result;
                            if (result1.IsSuccessStatusCode)
                            {
                                using (var client4 = new HttpClient())
                                {
                                    client4.BaseAddress = new Uri("http://localhost:52503/api/");

                                    var bookstr4 = $"{{\"BookId\": \"{book.BookId}\",\"BookName\": \"{book.BookName}\",\"BookDescription\": \"{book.BookDescription}\",\"BookAuthor\": \"{book.BookAuthor}\",\"BookPublication\": \"{book.BookPublication}\",\"BookPrice\": \"{book.BookPrice}\",\"BookStatus\": \"{Bookstatus}\"}}";
                                    HttpContent content4 = new StringContent(bookstr4, Encoding.UTF8, "application/json");
                                    var responseTask4 = client4.PutAsync("Books/" + book.BookId, content4);
                                    responseTask4.Wait();

                                    var result4 = responseTask4.Result;
                                    if (result4.IsSuccessStatusCode)
                                    {

                                        return RedirectToAction("LibrarianHome", "Librarian", new { LibrarianId = Session["LibrarianId"] });
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
        [HttpGet]
        public ActionResult RejectBookRequest(int id)
        {
            BookRequest obj = new BookRequest();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:52503/api/");

                var responseTask = client.GetAsync("BookRequests/" + id);
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<BookRequest>();
                    readTask.Wait();

                    obj = readTask.Result;

                    //Console.WriteLine(readTask.Result.Name);
                }

            }
            return View(obj);
        }
        [HttpPost]
        public ActionResult RejectBookRequest(BookRequest bookRequest)
        {

            try
            {
                string status = "Rejected";
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("http://localhost:52503/api/");

                    var str = $"{{\"BookRequestId\":\"{bookRequest.BookRequestId}\",\"RequestStatus\":\"{status}\",\"BookId\":\"{bookRequest.BookId}\",\"StudentId\":\"{bookRequest.StudentId}\"}}";
                    HttpContent content = new StringContent(str, Encoding.UTF8, "application/json");

                    var responseTask = client.PutAsync("BookRequests/" + bookRequest.BookRequestId, content);
                    responseTask.Wait();

                    var result = responseTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        return RedirectToAction("LibrarianHome", "Librarian", new { LibrarianId = Session["LibrarianId"] });
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
        public ActionResult BookModification()
        {
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
        public ActionResult BookDetails(int id)
        {
            Book obj = new Book();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:52503/api/");

                var responseTask = client.GetAsync("Books/" + id);
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
        [HttpGet]
        public ActionResult BookAdd()
        {
            return View();
        }
        [HttpPost]
        public ActionResult BookAdd(Book book)
        {
            try
            {
                using (var client = new HttpClient())
                {

                    HttpCookie cookieObj = Request.Cookies["token"];
                    string _websiteValue = cookieObj["tokenkey"];
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", _websiteValue);

                    client.BaseAddress = new Uri("http://localhost:52503/api/");

                    var bookstr = $"{{\"BookId\": \"{book.BookId}\",\"BookName\": \"{book.BookName}\",\"BookDescription\": \"{book.BookDescription}\",\"BookAuthor\": \"{book.BookAuthor}\",\"BookPublication\": \"{book.BookPublication}\",\"BookPrice\": \"{book.BookPrice}\",\"BookStatus\": \"{book.BookStatus}\"}}";
                    HttpContent content = new StringContent(bookstr, Encoding.UTF8, "application/json");

                    //GetAsync to send a GET    
                    //PutAsync to send a PUT   
                    //DeleteAsync to send a DELETE   
                    var responseTask = client.PostAsync("Books", content);
                    responseTask.Wait();

                    var result = responseTask.Result;
                    if (result.IsSuccessStatusCode)
                    {

                        return RedirectToAction("BookModification");
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
        [HttpGet]
        public ActionResult BookDelete(int id)
        {
            Book obj = new Book();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:52503/api/");

                var responseTask = client.GetAsync("Books/" + id);
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
        public ActionResult BookDelete(int id, Book book)
        {
            try
            {
                // TODO: Add delete logic here
                using (var client = new HttpClient())
                {

                    HttpCookie cookieObj = Request.Cookies["token"];
                    string _websiteValue = cookieObj["tokenkey"];
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", _websiteValue);

                    client.BaseAddress = new Uri("http://localhost:52503/api/");

                    var responseTask = client.DeleteAsync("Books/" + id);
                    responseTask.Wait();

                    var result = responseTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        var readTask = result.Content.ReadAsAsync<Book>();
                        readTask.Wait();

                        return RedirectToAction("BookModification");
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
        [HttpGet]
        public ActionResult BookEdit(int id)
        {
            Book obj = new Book();
            using (var client = new HttpClient())
            {

                client.BaseAddress = new Uri("http://localhost:52503/api/");

                var responseTask = client.GetAsync("Books/" + id);
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
        public ActionResult BookEdit(int id, Book book)
        {
            try
            {
                using (var client = new HttpClient())
                {

                    client.BaseAddress = new Uri("http://localhost:52503/api/");

                    var bookstr = $"{{\"BookId\": \"{book.BookId}\",\"BookName\": \"{book.BookName}\",\"BookDescription\": \"{book.BookDescription}\",\"BookAuthor\": \"{book.BookAuthor}\",\"BookPublication\": \"{book.BookPublication}\",\"BookPrice\": \"{book.BookPrice}\",\"BookStatus\": \"{book.BookStatus}\"}}";
                    HttpContent content = new StringContent(bookstr, Encoding.UTF8, "application/json");
                    var responseTask = client.PutAsync("Books/" + book.BookId, content);
                    responseTask.Wait();

                    var result = responseTask.Result;
                    if (result.IsSuccessStatusCode)
                    {

                        return RedirectToAction("BookModification");
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

        public ActionResult UserModification()
        {
            IEnumerable<Student> members = null;

            using (var client = new HttpClient())
            {

                client.BaseAddress = new Uri("http://localhost:52503/api/");

                var responseTask = client.GetAsync("Students");
                responseTask.Wait();

                //To store result of web api response.   
                var result = responseTask.Result;

                //If success received   
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<IList<Student>>();
                    readTask.Wait();
                    members = readTask.Result;
                }
                else
                {
                    //Error response received   
                    members = Enumerable.Empty<Student>();
                }

                return View(members);
            }
        }
        [HttpGet]
        public ActionResult UserEdit(int id)
        {
            Student obj = new Student();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:52503/api/");

                var responseTask = client.GetAsync("Students/" + id);
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<Student>();
                    readTask.Wait();

                    obj = readTask.Result;
                }

            }
            return View(obj);
        }
        [HttpPost]
        public ActionResult UserEdit(int id,Student student)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("http://localhost:52503/api/");

                    var bookstr = $"{{\"StudentId\": \"{student.StudentId}\",\"StudentName\": \"{student.StudentName}\",\"StudentPassword\": \"{student.StudentPassword}\",\"StudentImage\": \"{student.StudentImage}\",\"StudentContact\": \"{student.StudentContact}\",\"StudentEmail\": \"{student.StudentEmail}\"}}";
                    HttpContent content = new StringContent(bookstr, Encoding.UTF8, "application/json");
                    var responseTask = client.PutAsync("Students/" + id, content);
                    responseTask.Wait();

                    var result = responseTask.Result;
                    if (result.IsSuccessStatusCode)
                    {

                        return RedirectToAction("UserModification");
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
        public ActionResult UserDetails(int id)
        {
            Student obj = new Student();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:52503/api/");

                var responseTask = client.GetAsync("Students/" + id);
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<Student>();
                    readTask.Wait();

                    obj = readTask.Result;
                }

            }
            return View(obj);
        }
        [HttpGet]
        public ActionResult UserDelete(int id)
        {
            Student obj = new Student();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:52503/api/");

                var responseTask = client.GetAsync("Students/" + id);
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<Student>();
                    readTask.Wait();

                    obj = readTask.Result;
                }
                return View(obj);
            }
        }
        [HttpPost]
        public ActionResult UserDelete(int id, Student student)
        {
                try
                {
                    // TODO: Add delete logic here
                    using (var client = new HttpClient())
                    {

                    HttpCookie cookieObj = Request.Cookies["token"];
                    string _websiteValue = cookieObj["tokenkey"];
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", _websiteValue);


                    client.BaseAddress = new Uri("http://localhost:52503/api/");

                        var responseTask = client.DeleteAsync("Students/" + id);
                        responseTask.Wait();

                        var result = responseTask.Result;
                        if (result.IsSuccessStatusCode)
                        {
                            var readTask = result.Content.ReadAsAsync<Student>();
                            readTask.Wait();

                            return RedirectToAction("UserModification");
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
        public ActionResult ViewStudRecomend()
        {
            IEnumerable<StudentModel> student = null;

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
                    student = readTask1.Result;
                }
            }

            IEnumerable<BookRecomendation> members = null;

            using (var client = new HttpClient())
            {

                client.BaseAddress = new Uri("http://localhost:52503/api/");

                var responseTask = client.GetAsync("BookRecomendations");
                responseTask.Wait();

                //To store result of web api response.   
                var result = responseTask.Result;

                //If success received   
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<IList<BookRecomendation>>();
                    readTask.Wait();
                    members = readTask.Result;
                }
                else
                {
                    //Error response received   
                    members = Enumerable.Empty<BookRecomendation>();
                }

                List<ViewStudRecomend> bookrecomend = new List<ViewStudRecomend>();
                foreach (var item in members)
                {
                    foreach (var item1 in student)
                    {
                        if (item.StudentId == item1.StudentId)
                        {
                            ViewStudRecomend obj = new ViewStudRecomend();
                            obj.BookRecomendationId = item.BookRecomendationId;
                            obj.StudentId = item.StudentId;
                            obj.StudentName = item1.StudentName;
                            obj.BookName = item.BookName;
                            obj.BookAuthor = item.BookAuthor;
                            obj.BookDescription = item.BookDescription;
                            obj.BookPublication = item.BookPublication;
                            bookrecomend.Add(obj);
                        }
                    }
                }


                return View(bookrecomend);
            }
        }
        [HttpGet]
        public ActionResult DeleteStudRecomend(int id)
        {
            BookRecomendation obj = new BookRecomendation();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:52503/api/");

                var responseTask = client.GetAsync("BookRecomendations/" + id);
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<BookRecomendation>();
                    readTask.Wait();

                    obj = readTask.Result;
                }
                return View(obj);
            }
        }
        [HttpPost]
        public ActionResult DeleteStudRecomend(int id,BookRecomendation bookRecomendation)
        {
            try
            {
                // TODO: Add delete logic here
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("http://localhost:52503/api/");

                    var responseTask = client.DeleteAsync("BookRecomendations/" + id);
                    responseTask.Wait();

                    var result = responseTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        var readTask = result.Content.ReadAsAsync<BookRecomendation>();
                        readTask.Wait();

                        return RedirectToAction("ViewStudRecomend");
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
        public ActionResult ViewFacRecomend()
        {
            IEnumerable<FacultyModel> faculty = null;

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
                    faculty = readTask1.Result;
                }
            }
            IEnumerable<FacultyBookRecommendation> members = null;

            using (var client = new HttpClient())
            {

                client.BaseAddress = new Uri("http://localhost:52503/api/");

                var responseTask = client.GetAsync("FacultyBookRecommendations");
                responseTask.Wait();

                //To store result of web api response.   
                var result = responseTask.Result;

                //If success received   
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<IList<FacultyBookRecommendation>>();
                    readTask.Wait();
                    members = readTask.Result;
                }
                else
                {
                    //Error response received   
                    members = Enumerable.Empty<FacultyBookRecommendation>();
                }

                
            }
            List<ViewFacRecomend> bookrecomend = new List<ViewFacRecomend>();
            foreach(var item in members)
            {
                foreach(var item1 in faculty)
                {
                    if(item.FacultyId==item1.FacultyId)
                    {
                        ViewFacRecomend obj = new ViewFacRecomend();
                        obj.FacultyBookRecommnedationId = item.FacultyBookRecommnedationId;
                        obj.FacultyId = item.FacultyId;
                        obj.FacultyName = item1.FacultyName;
                        obj.BookName = item.BookName;
                        obj.BookAuthor = item.BookAuthor;
                        obj.BookDescription = item.BookDescription;
                        obj.BookPublication = item.BookPublication;
                        bookrecomend.Add(obj);
                    }
                }
            }
            return View(bookrecomend);
        }
        [HttpGet]
        public ActionResult DeleteFacRecomend(int id)
        {
            FacultyBookRecommendation obj = new FacultyBookRecommendation();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:52503/api/");

                var responseTask = client.GetAsync("FacultyBookRecommendations/" + id);
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<FacultyBookRecommendation>();
                    readTask.Wait();

                    obj = readTask.Result;
                }
                return View(obj);
            }
        }
        [HttpPost]
        public ActionResult DeleteFacRecomend(int id, FacultyBookRecommendation facultyBookRecommendation)
        {
            try
            {
                // TODO: Add delete logic here
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("http://localhost:52503/api/");

                    var responseTask = client.DeleteAsync("FacultyBookRecommendations/" + id);
                    responseTask.Wait();

                    var result = responseTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        var readTask = result.Content.ReadAsAsync<FacultyBookRecommendation>();
                        readTask.Wait();

                        return RedirectToAction("ViewFacRecomend");
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