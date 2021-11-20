using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LMSMVC.Models
{
    public class LibrarianModel
    {
        [Display(Name = "Librarian Id")]
        public int LibrarianId { get; set; }

        [Display(Name = "User Name")]
        public string UserName { get; set; }

        [Display(Name = "Name")]
        public string LibrarianName { get; set; }

        [Display(Name = "Password")]
        public string LibrarianPassword { get; set; }

        [Display(Name = "Image")]
        public byte[] LibrarianImage { get; set; }

        [Display(Name = "Contact Details")]
        public Int64 LibrarianContact { get; set; }

        [Display(Name = "Email Id")]
        public string LibrarianEmail { get; set; }
    }
}