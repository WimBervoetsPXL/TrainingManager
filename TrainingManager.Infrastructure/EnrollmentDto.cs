using System;
using System.Collections.Generic;
using System.Text;

namespace TrainingManager.Infrastructure
{
    internal class EnrollmentDto
    {
        public int Id { get; set; }
        public string TrainingId { get; set; }
        public int EmployeeId { get; set; }
        public bool IsBillable { get; set; }
    }
}
