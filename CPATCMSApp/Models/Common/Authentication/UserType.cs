using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CPATCMSApp.Models.Common.Authentication
{
    public class UserType
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public List<ApplicationUser> ApplicationUsers { get; set; }
    }
}
