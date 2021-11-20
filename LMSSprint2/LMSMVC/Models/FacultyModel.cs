using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LMSMVC.Models
{
    public class FacultyModel
    {
        [Display(Name = "Faculty Id")]
        public int FacultyId { get; set; }

        [Display(Name = "User Name")]
        public string UserName { get; set; }

        [Display(Name = "Name")]
        public string FacultyName { get; set; }

        [Display(Name = "Password")]
        public string FacultyPassword { get; set; }

        [Display(Name = "Image")]
        public byte[] FacultyImage { get; set; }

        [Display(Name = "Contact Number")]
        public Int64 FacultyContact { get; set; }

        [Display(Name = "Email Id")]
        public string FacultyEmail { get; set; }
    }
}