using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LMSMVC.Models
{
    public class Librarian
    {
        [Key]
        public int LibrarianId { get; set; }

        [DisplayName("Name")]
        [Required(ErrorMessage = "Required Name")]
        [StringLength(30, MinimumLength = 2, ErrorMessage = "Name Must be Minimum 2 Charaters")]
        public string LibrarianName { get; set; }


        [DisplayName("Password")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Required Password")]
        [MaxLength(30, ErrorMessage = "Password cannot be Greater than 30 Charaters")]
        [StringLength(31, MinimumLength = 7, ErrorMessage = "Password Must be Minimum 7 Charaters")]
        public string LibrarianPassword { get; set; }

        [DisplayName("Confirm Password")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Required Confirm Password")]
        [MaxLength(30, ErrorMessage = "Password cannot be Greater than 30 Charaters")]
        [StringLength(31, MinimumLength = 7, ErrorMessage = "Password Must be Minimum 7 Charaters")]
        [Compare("LibrarianPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string LibrarianConfirmPassword { get; set; }

        //[Display(Name = "Profile Image")]
        //[Required, FileExtensions(Extensions = "jpg", ErrorMessage = "Specify a JPG file. (Comma-separated values)")]
        //[Required(ErrorMessage = "Please select file.")]
        ////[Required(ErrorMessage = "Please select file.")]
        [RegularExpression(@"([a-zA-Z0-9\s_\\.\-:])+(.png|.jpg|.gif)$", ErrorMessage = "Only Image files allowed.")]
        [Display(Name = "Image")]
        public byte[] LibrarianImage { get; set; }


        [Display(Name = "Contact")]
        [Required(ErrorMessage = "Contact is a Required field.")]
        [RegularExpression(@"^([0-9]{10})$", ErrorMessage = "The Contact is not a valid phone number.")]
        public Int64 LibrarianContact { get; set; }

        [Display(Name = "Email ID")]
        [Required(ErrorMessage = "Required EmailID")]
        [RegularExpression(@"[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,4}", ErrorMessage = "Please enter Valid Email ID")]
        public string LibrarianEmail { get; set; }


       
    }
}