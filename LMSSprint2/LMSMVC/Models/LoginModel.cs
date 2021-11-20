using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LMSMVC.Models
{

    public class LoginModel
    {
        [DisplayName("User Name")]
        [Required(ErrorMessage = "Required User Name")]
        public string UserName { get; set; }


        [DisplayName("Password")]
        [Required(ErrorMessage = "Required User Password")]
        [DataType(DataType.Password)]
        public string UserPassword { get; set; }
    }
}