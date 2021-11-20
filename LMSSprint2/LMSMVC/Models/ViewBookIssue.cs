using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LMSMVC.Models
{
    public class ViewBookIssue
    {
        [Display(Name = "Book Issue Id")]
        public int BookIssueId { get; set; }
        [Display(Name = "Book Request Id")]
        public Nullable<int> BookRequestId { get; set; }
        [Display(Name = "Book Id")]
        public Nullable<int> BookId { get; set; }
        [Display(Name = "Student Id")]
        public Nullable<int> StudentId { get; set; }
        [Display(Name = "Librarian Id")]
        public Nullable<int> LibrarianId { get; set; }
        [Display(Name = "Date of Issue")]
        public Nullable<System.DateTime> DateOfIssue { get; set; }
        [Display(Name = "Date of Return")]
        public Nullable<System.DateTime> DateOfReturn { get; set; }
        [Display(Name = "Due Payment")]
        public Nullable<int> DuePayment { get; set; }
        [Display(Name = "Book Name")]
        public string BookName { get; set; }
    }
}