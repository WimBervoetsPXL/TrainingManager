using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using TrainingManager.Domain;

namespace TrainingManager.Infrastructure
{
    public class EmployeeRepository
    {
        private readonly DbConnectionFactory _factory;

        public EmployeeRepository(DbConnectionFactory factory)
        {
            _factory = factory;
        }

        public async Task<IEnumerable<Employee>> GetAllAsync()
        {
            using(IDbConnection connection = _factory.CreateConnection())
            {
                return await connection.QueryAsync<Employee>("SELECT Id, FirstName, LastName FROM Employees");
            }
        }

        public async Task<Employee> GetByIdAsync(int id)
        {
            using (IDbConnection connection = _factory.CreateConnection())
            {
                string sql = $"""
                    SELECT Id, FirstName, LastName 
                    FROM Employees 
                    WHERE Id = @Id
                    """;
                return await connection.QueryFirstOrDefaultAsync<Employee>(sql, new { Id = id });
            }
        }

        public async Task AddAsync(Employee employee)
        {
            using(IDbConnection connection = _factory.CreateConnection())
            {
                string sql = """
                    INSERT INTO Employees (FirstName, LastName) 
                    VALUES (@FirstName, @LastName); 
                    SELECT CAST(SCOPE_IDENTITY() as int)"
                    """;

                int newId = await connection.ExecuteScalarAsync<int>(sql, employee);
                employee.Id = newId;
            }
        }

        public async Task UpdateAsync(Employee employee)
        {
            using (IDbConnection connection = _factory.CreateConnection())
            {
                string sql = """
                    UPDATE Employees 
                    SET FirstName = @FirstName, LastName = @LastName 
                    WHERE Id = @Id
                """;

                await connection.ExecuteAsync(sql, employee);
            }
        }

        public async Task DeleteAsync(int id)
        {
            using (IDbConnection connection = _factory.CreateConnection())
            {
                string sql = """
                    DELETE FROM Employees 
                    WHERE Id = @Id
                """;

                await connection.ExecuteAsync(sql, new { Id = id });
            }
        }
    }
}
