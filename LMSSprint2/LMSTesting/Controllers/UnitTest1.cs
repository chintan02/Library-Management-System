using Microsoft.VisualStudio.TestTools.UnitTesting;
using LMSAPI.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using LMSAPI.Models;

namespace LMSAPI.Controllers.Tests
{
    [TestClass()]
    public class UnitTest1
    {
        [TestMethod()]
        public void GetBooksTest()
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
            }
            Assert.IsNotNull(members);
        }

        [TestMethod()]
        public void GetFacultyBookRecommendationTest()
        {
            FacultyBookRecommendation obj = new FacultyBookRecommendation();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:52503/api/");

                var responseTask = client.GetAsync("FacultyBookRecommendations/" + 20);
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<FacultyBookRecommendation>();
                    readTask.Wait();

                    obj = readTask.Result;
                }
            }
            Assert.IsNotNull(obj);
        }

    }
}