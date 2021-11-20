using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LMSMVC.Models
{
    public class BookRequest
    {
        [Display(Name = "Book Request Id")]
        public int BookRequestId { get; set; }

        [Display(Name = "Request Status")]
        public string RequestStatus { get; set; }

        [Display(Name = "Book Id")]
        public Nullable<int> BookId { get; set; }

        [Display(Name = "Student Id")]
        public Nullable<int> StudentId { get; set; }
    }
}