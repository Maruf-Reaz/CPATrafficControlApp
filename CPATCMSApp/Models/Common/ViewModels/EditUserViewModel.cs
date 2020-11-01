using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CPATCMSApp.Models.Common.ViewModels
{
    public class EditUserViewModel
    {
        public string Id { get; set; }

        [Required(ErrorMessage = "Please Enter a User Name")]
        [Display(Name = "User Name")]
        public string UserName { set; get; }

        [EmailAddress(ErrorMessage = "Please Enter a Valid Email")]
        public string Email { get; set; }

        public IList<string> Roles { get; set; }

        public EditUserViewModel()
        {
            Roles = new List<string>();
        }
    }
}
