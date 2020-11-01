using System.Collections.Generic;

namespace CPATCMSApp.Models.Assignments
{
    public class AssignmentSlot
    {
        public int Id { get; set; }

        public string AssignmentName { get; set; }

        public int StartTime { get; set; }

        public int EndTime { get; set; }

        public List<Assignment> Assignments { get; set; }
    }
}
