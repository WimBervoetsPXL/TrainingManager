using System;
using System.Collections.Generic;
using System.Text;

namespace TrainingManager.Domain
{
    public class Employee
    {
        public int Id { get; set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }

        //public Employee()
        //{
            
        //}

        public Employee(string firstName, string lastName)
        {
            if(string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName))
            {
                throw new ArgumentException("Voor- en achternaam zijn verplicht.");
            }

            FirstName = firstName;
            LastName = lastName;
        }

        public Employee(int id, string firstName, string lastName) : this(firstName, lastName)
        {
            Id = id;
        }

        public void Update(string firstName, string lastName)
        {
            if (string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName))
            {
                throw new ArgumentException("Voor- en achternaam zijn verplicht.");
            }

            FirstName = firstName;
            LastName = lastName;
        }
    }
}
