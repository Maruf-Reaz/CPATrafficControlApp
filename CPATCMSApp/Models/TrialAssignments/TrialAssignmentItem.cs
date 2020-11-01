using System;

namespace CPATCMSApp.Models.TrialAssignments
{
    public class TrialAssignmentItem
    {
        public int Id { get; set; }

        public int YardId { get; set; }
        public string BlockName { get; set; }

        public string CnFName { get; set; }

        public string CnFCode { get; set; }

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

        //public DateTime KeepDownTime { get; set; }

        //public int EstimatedTruckQty { get; set; }
        //public int Status { get; set; }

        //public List<TruckEntity> TruckEntities { get; set; }
    }
}
