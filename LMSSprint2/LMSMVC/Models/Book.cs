using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LMSMVC.Models
{
    public class Book
    {
        [Display(Name = "Book ID")]
        public int BookId { get; set; }

        [Display(Name = "Book Name")]
        public string BookName { get; set; }

        [Display(Name = "Book Description")]
        public string BookDescription { get; set; }

        [Display(Name = "Book Author")]
        public string BookAuthor { get; set; }

        [Display(Name = "Book Publication")]
        public string BookPublication { get; set; }

        [Display(Name = "Book Price")]
        public Nullable<double> BookPrice { get; set; }

        [Display(Name = "Book Status")]
        public string BookStatus { get; set; }
    }
}