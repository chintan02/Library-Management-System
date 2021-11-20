using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LMSMVC.Models
{
    public class ViewBookReq
    {
        [Display(Name = "Book Request Id")]
        public int BookRequestId { get; set; }
        [Display(Name = "Request Status")]
        public string RequestStatus { get; set; }
        [Display(Name = "Book Id")]
        public Nullable<int> BookId { get; set; }
        [Display(Name = "Student Id")]
        public Nullable<int> StudentId { get; set; }
        [Display(Name = "Student Name")]
        public string StudentName { get; set; }
        [Display(Name = "Book Name")]
        public string BookName { get; set; }
        [Display(Name = "Book Status")]
        public string BookStatus { get; set; }

    }
}