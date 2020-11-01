using CPATCMSApp.Models.Common.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CPATCMSApp.Models.Gates
{
    public class Gate
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public List<ApplicationUser> ApplicationUsers { get; set; }
    }
}
