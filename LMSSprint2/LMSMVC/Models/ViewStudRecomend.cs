using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LMSMVC.Models
{
    public class ViewStudRecomend
    {
        [Display(Name = "BookRecommnedation Id")]
        public int BookRecomendationId { get; set; }
        [Display(Name = "Student Id")]
        public Nullable<int> StudentId { get; set; }
        [Display(Name = "Student Name")]
        public string StudentName { get; set; }
        [Display(Name = "Book Name")]
        [Required(ErrorMessage = "Please enter book Name")]
        public string BookName { get; set; }
        [Display(Name = "Book Description")]
        [Required(ErrorMessage = "Please enter book description")]
        public string BookDescription { get; set; }
        [Display(Name = "Book Author")]
        [Required(ErrorMessage = "Please enter book Author")]
        public string BookAuthor { get; set; }
        [Display(Name = "Book Publication")]
        [Required(ErrorMessage = "Please enter book Publication")]
        public string BookPublication { get; set; }
    }
}