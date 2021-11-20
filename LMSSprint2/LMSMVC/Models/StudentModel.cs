using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace LMSMVC.Models
{
    public class StudentModel
    {
        [DisplayName("Student Id")]
        public int StudentId { get; set; }

        [DisplayName("User Name")]
        public string UserName { get; set; }

        [DisplayName("Name")]
        public string StudentName { get; set; }

        [DisplayName("Password")]
        public string StudentPassword { get; set; }

        [DisplayName("Image")]
        public byte[] StudentImage { get; set; }

        [DisplayName("Contact Details")]
        public Int64 StudentContact { get; set; }

        [DisplayName("Email Id")]
        public string StudentEmail { get; set; }
    }
}