using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LMSMVC.Models
{
    public class FacultyBookRecommendation
    {
        [Display(Name = "Recommeded Book")]
        public int FacultyBookRecommnedationId { get; set; }

        [Display(Name = "Faculty Id")]
        public Nullable<int> FacultyId { get; set; }


        [Display(Name = "Book Name")]
        [Required(ErrorMessage = "Please enter book Name")]
        public string BookName { get; set; }


        [Display(Name = "Book Description")]
        [Required(ErrorMessage = "Please enter book description")]
        public string BookDescription { get; set; }


        [Display(Name = "Author")]
        [Required(ErrorMessage = "Please enter book Author")]
        public string BookAuthor { get; set; }


        [Display(Name = "Publication")]
        [Required(ErrorMessage = "Please enter book Publication")]
        public string BookPublication { get; set; }
    }
}