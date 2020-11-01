using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CPATCMSApp.Models.Yards
{
    public class Bay
    {

        public int Id { get; set; }

        public string Name { get; set; }

        public List<Yard> Yards { get; set; }
    }
}
