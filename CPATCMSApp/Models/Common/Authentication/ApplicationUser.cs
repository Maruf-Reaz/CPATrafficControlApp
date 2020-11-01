using CPATCMSApp.Models.CnFs;
using CPATCMSApp.Models.Gates;
using CPATCMSApp.Models.Yards;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CPATCMSApp.Models.Common.Authentication
{
    public class ApplicationUser : IdentityUser
    {
        [Display(Name = "Gate")]
        public int? GateId { get; set; }
        public Gate Gate { get; set; }

        [Display(Name = "Yard")]
        public int? YardId { get; set; }
        public Yard Yard { get; set; }

        [Display(Name = "User Type")]
        public int UserTypeId { get; set; }
        public UserType UserType { get; set; }

        public CnFProfile CnFProfile { get; set; }

        //[NotMapped]
        //public string GateName { get; set; }

        //[NotMapped]
        //public string YardName { get; set; }

        //[NotMapped]
        //public string UserTypeName { get; set; }
    }
}
