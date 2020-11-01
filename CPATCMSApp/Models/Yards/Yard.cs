using CPATCMSApp.Models.Common.Authentication;
using CPATCMSApp.Models.Gates;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CPATCMSApp.Models.Yards
{
    public class Yard
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Number { get; set; }

        public int BayId { get; set; }
        public Bay Bay { get; set; }
        

        public int? GateId { get; set; }
        public Gate Gate { get; set; }

        public int? OutGateId { get; set; }
        public Gate OutGate { get; set; }

        [NotMapped]
        public string BayName { get; set; }

     
        public List<ApplicationUser> ApplicationUsers { get; set; }
    }
}
