using System;
using System.Collections.Generic;
using System.Text;

namespace TrainingManager.Domain
{
    public class Enrollment
    {
        public int Id { get; set; }
        public Training Training { get; set; }
        public Employee Employee { get; set; }
        public bool IsBillable { get; set; }

        public Enrollment()
        {
            
        }

        public Enrollment(int id, Training training, Employee employee)
        {
            Id = id;
            Training = training;
            Employee = employee;
        }
    }
}
