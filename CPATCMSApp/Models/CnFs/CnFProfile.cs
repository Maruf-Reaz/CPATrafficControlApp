using CPATCMSApp.Models.Common.Authentication;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CPATCMSApp.Models.CnFs
{
    public class CnFProfile
    {
        public int Id { get; set; }

        [Required]
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }

        public string VerificationNumber { get; set; }

        public double Balance { get; set; }

        [Display(Name = "Phone Number")]
        [RegularExpression(@"(^(\01)?(01){1}[23456789]{1}(\d){8})$", ErrorMessage = "Please provide a valid mobile number")]
        public string PhoneNumber { get; set; }

        [NotMapped]
        public string UserName { get; set; }
    }
}
