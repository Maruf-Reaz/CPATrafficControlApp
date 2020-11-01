using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CPATCMSApp.Models.Common.ViewModels
{
    public class RoleViewModel
    {
        public string Id { get; set; }

        [Required(ErrorMessage = "Please Enter Name of Role")]
        [Display(Name = "Role Name")]
        public string RoleName { get; set; }

        public string Description { get; set; }

        [Display(Name = "Created On")]
        public DateTime? CreatedOn { get; set; }

        public List<string> UserNames { get; set; }

        public RoleViewModel()
        {
            UserNames = new List<string>();
        }
    }
}
