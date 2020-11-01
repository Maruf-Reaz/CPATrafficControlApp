using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CPATCMSApp.Models.ViewModels
{
    public class UpdatePasswordViewModel
    {
        public string UserId { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Previous Password")]
        [Required(ErrorMessage = "Please Enter the Previous Password")]
        public string PreviousPassword { set; get; }

        [DataType(DataType.Password)]
        [Display(Name = "New Password")]
        [Required(ErrorMessage = "Please Enter a New Password")]
        public string NewPassword { set; get; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm New Password")]
        [Compare("NewPassword", ErrorMessage = "New Password and Confirm New Password Do Not Match")]
        public string ConfirmNewPassword { get; set; }
    }
}
