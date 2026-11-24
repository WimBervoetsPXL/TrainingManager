using System;
using System.Collections.Generic;
using System.Text;
using TrainingManager.Domain;
using TrainingManager.Infrastructure;

namespace TrainingManager.Application
{
    public class EmployeeService
    {
        private readonly EmployeeRepository _repository;

        public EmployeeService(EmployeeRepository repository)
        {
            _repository = repository;

            //string connectionString = @"Server=.\SQLEXPRESS;Database=StudentDb;Trusted_Connection=True;Encrypt=True;TrustServerCertificate=True;";
            //DbConnectionFactory factory = new DbConnectionFactory(connectionString);
            //_repository = new EmployeeRepository(factory);
        }

        public async Task<IEnumerable<Employee>> GetAllEmployeesAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<Employee> GetEmployeeByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<Employee> AddEmployeeAsync(string firstName, string lastName)
        {
            Employee emp = new Employee(firstName, lastName);

            await _repository.AddAsync(emp);

            return emp;
        }

        public async Task UpdateEmployeeAsync(int id, string firstName, string lastName)
        {
            Employee emp = await _repository.GetByIdAsync(id);

            if(emp is null)
            {
                throw new ArgumentException($"Werknemer met id {id} bestaat niet.");
            }

            emp.Update(firstName, lastName);
           
            await _repository.UpdateAsync(emp);
        }



    }
}
