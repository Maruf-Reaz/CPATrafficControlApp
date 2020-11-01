using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CPATCMSApp.Models.ViewModels
{
    public class AssignmentApiViewModel
    {
        
        public string CNF_Code { get; set; }
        public string CNF_Agent { get; set; }

        //public int AssignmentId { get; set; }
        //public Assignment Assignment { get; set; }

        public string Vessel_Name { get; set; }

        public string Import_Rotation { get; set; }

        public string MLO { get; set; }
        public string Delivery { get; set; }

        public string Container_No { get; set; }

        public double Cont_Size { get; set; }
        public double Cont_Height { get; set; }

        public string line_no { get; set; }
        public string Delivery_st { get; set; }
        public string remarks { get; set; }
        public string Verify_No { get; set; }
        public string Exit_No { get; set; }
        public string Assignment_Type { get; set; }
        public string Bay { get; set; }
        public string Yard_No { get; set; }
        public string Assignment_Dt { get; set; }
        public string Block_No { get; set; }
        
        
    }
}
