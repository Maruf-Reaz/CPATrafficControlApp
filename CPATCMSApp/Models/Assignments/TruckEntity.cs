using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CPATCMSApp.Models.Assignments
{
    public class TruckEntity
    {
        public int Id { get; set; }

        public int AssignmentItemId { get; set; }
        public AssignmentItem AssignmentItem { get; set; }

        [Required]
        public string TruckNumer { get; set; }

        //[Display(Name = "ID Card Number of Driver")]
        public string IdCardNumberOfDriver { get; set; }

        //[Display(Name = "ID Card Number of Assistant/Helper")]
        public string IdCardNumberOfAssistant { get; set; }

        public DateTime EntryDate { get; set; }
        public int EntryTime { get; set; }
        public int ExitTime { get; set; }
        public int LoadingTime { get; set; }
        public int Status { get; set; }

        [NotMapped]
        public string AssignmentItemCnFProfileName { get; set; }

        [NotMapped]
        public string AssignmentItemContainerNumber { get; set; }

        [NotMapped]
        public string AssignmentItemVerifyNumber { get; set; }

        [NotMapped]
        public string AssignmentItemExitNumber { get; set; }
    }
}
