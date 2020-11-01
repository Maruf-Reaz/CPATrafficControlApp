using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CPATCMSApp.Models.ViewModels
{
    public class AssignmentViewModel
    {
        public int AssignmenItemtId { get; set; }

        public IFormFile ReleaseOrder { get; set; }
        public IFormFile CartTicket { get; set; }
        public IFormFile BillOfEntity { get; set; }
        public IFormFile AssesmentNotice { get; set; }
        public IFormFile OneStopBill { get; set; }
        public IFormFile ContainerScanCopy { get; set; }

        [NotMapped]
        public string OldReleaseOrder { get; set; }

        [NotMapped]
        public string OldCartTicket { get; set; }
        [NotMapped]
        public string OldBillOfEntity { get; set; }
        [NotMapped]
        public string OldAssesmentNotice { get; set; }
        [NotMapped]
        public string OldOneStopBill { get; set; }
        [NotMapped]
        public string OldContainerScanCopy { get; set; }
        

    }
}
