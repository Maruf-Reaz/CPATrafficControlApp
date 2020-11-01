using System;
using CPATCMSApp.Models.CnFs;
using CPATCMSApp.Models.Yards;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace CPATCMSApp.Models.Assignments
{
    public class AssignmentItem
    {
        public int Id { get; set; }

        public int YardId { get; set; }
        public Yard Yard { get; set; }

        public int CnFProfileId { get; set; }
        public CnFProfile CnFProfile { get; set; }

        public int AssignmentId { get; set; }
        public Assignment Assignment { get; set; }

        public string Vessel { get; set; }

        public string ImpReg { get; set; }

        public string MLO { get; set; }

        public string ContainerNumber { get; set; }

        public double Size { get; set; }
        public double Height { get; set; }

        public string LineNumber { get; set; }
        public string Dst { get; set; }
        public string Remarks { get; set; }
        public string VerifyNumber { get; set; }
        public string ExitNumber { get; set; }
        public string AssignmentType { get; set; }

        public DateTime CreationTime { get; set; }

        public DateTime KeepDownTime { get; set; }

        public int EstimatedTruckQty { get; set; }
        public int Status { get; set; }

        public List<TruckEntity> TruckEntities { get; set; }

        public string ReleaseOrder { get; set; }
        public string CartTicket { get; set; }
        public string BillOfEntity { get; set; }
        public string AssesmentNotice { get; set; }
        public string OneStopBill { get; set; }
        public string ContainerScanCopy { get; set; }

        [NotMapped]
        public DateTime AssignmentDate { get; set; }

        [NotMapped]
        public string AssignmentAssignmentSlotAssignmentName { get; set; }

        [NotMapped]
        public string YardName { get; set; }

        [NotMapped]
        public string BayName { get; set; }

        [NotMapped]
        public string CnFProfileName { get; set; }

        [NotMapped]
        public string CnFProfileVerificationNumber { get; set; }
    }
}
