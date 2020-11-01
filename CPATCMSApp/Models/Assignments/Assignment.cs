using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace CPATCMSApp.Models.Assignments
{
    public class Assignment
    {
        public int Id { get; set; }

        public DateTime Date { get; set; }

        public int AssignmentSlotId { get; set; }
        public AssignmentSlot AssignmentSlot { get; set; }

        public int Status { get; set; }         //1=Added, 2=Submitted

        public DateTime DeliveryDate { get; set; }

        public int DeliveryStart { get; set; }

        public int DeliveryEnd { get; set; }

        public int NumberOfAssignmentItems { get; set; }

        public List<AssignmentItem> AssignmentItems { get; set; }

        [NotMapped]
        public string AssignmentSlotAssignmentName { get; set; }
    }
}
