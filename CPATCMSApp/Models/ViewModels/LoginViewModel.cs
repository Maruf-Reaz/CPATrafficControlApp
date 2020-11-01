using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CPATCMSApp.Models.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Please Enter User Name to Login")]
        [Display(Name = "User Name")]
        public string UserName { set; get; }

        [Required(ErrorMessage = "Please Enter Your Password")]
        [DataType(DataType.Password)]
        public string Password { set; get; }

        [Display(Name = "Remember Me")]
        public bool RememberMe { get; set; }
    }
}
