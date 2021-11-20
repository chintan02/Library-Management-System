using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LMSMVC.Models
{
    public class Student
    {
        [Key]
        public int StudentId { get; set; }

        [DisplayName("Name")]
        [Required(ErrorMessage = "Required Name")]
        [StringLength(30, MinimumLength = 2, ErrorMessage = "StudentName Must be Minimum 2 Charaters")]
        public string StudentName { get; set; }

        [DisplayName("Password")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Required Password")]
        [MaxLength(30, ErrorMessage = "Password cannot be Greater than 30 Charaters")]
        [StringLength(31, MinimumLength = 7, ErrorMessage = "Password Must be Minimum 7 Charaters")]
        public string StudentPassword { get; set; }

        [Display(Name = "Confirm Password")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Required Password")]
        [MaxLength(30, ErrorMessage = "Password cannot be Greater than 30 Charaters")]
        [StringLength(31, MinimumLength = 7, ErrorMessage = "Password Must be Minimum 7 Charaters")]
        [Compare("StudentPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string StudentConfirmPassword { get; set; }


        //[Required(ErrorMessage = "Please select file.")]
        [RegularExpression(@"([a-zA-Z0-9\s_\\.\-:])+(.png|.jpg|.gif)$", ErrorMessage = "Only Image files allowed.")]
        [Display(Name = "Image")]
        public byte[] StudentImage { get; set; }


        //[Display(Name = "Profile Image")]
        //[Required, FileExtensions(Extensions = "jpg", ErrorMessage = "Specify a JPG file. (Comma-separated values)")]
        //public string StudentImage { get; set; }
        //[StringLength(maximumLength:10,MinimumLength =10, ErrorMessage = "Contact Must be of 10 digits")]
        //[RegularExpression("^((?!^StudentContact)[0-9])+$", ErrorMessage = "Contact Details is required and must be properly formatted.")]
        
        [Display(Name = "Contact")]
        [Required(ErrorMessage = "Contact is a Required field.")]
        [RegularExpression(@"^([0-9]{10})$", ErrorMessage = "The Contact is not a valid phone number.")]
        public Int64 StudentContact { get; set; }

        [Display(Name = "Email ID")]
        [Required(ErrorMessage = "Required EmailID")]
        [RegularExpression(@"[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,4}", ErrorMessage = "Please enter Valid Email ID")]
        public string StudentEmail { get; set; }
    }
}