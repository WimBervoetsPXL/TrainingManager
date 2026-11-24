using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using TrainingManager.Domain;

namespace TrainingManager.Infrastructure
{
    internal class EnrollmentRepository
    {
        private readonly DbConnectionFactory _factory;
        private readonly EmployeeRepository _employeeRepository;
        private readonly TrainingRepository _trainingRepository;

        public EnrollmentRepository(
            DbConnectionFactory connectionFactory, 
            EmployeeRepository employeeRepository, 
            TrainingRepository trainingRepository)
        {
            _factory = connectionFactory;
            _employeeRepository = employeeRepository;
            _trainingRepository = trainingRepository;
        }

        public async Task<IEnumerable<Enrollment>> GetAllAsync()
        {
            string sql = """
                SELECT Id, EmployeeId, TrainingId, IsBillable FROM Enrollments
                """;

            IEnumerable<EnrollmentDto> result;
            IEnumerable<dynamic> dynamicResult;

            using (IDbConnection connection = _factory.CreateConnection())
            {
                result = await connection.QueryAsync<EnrollmentDto>(sql);
                dynamicResult = await connection.QueryAsync<dynamic>(sql);
                dynamicResult = await connection.QueryAsync(sql);
            }

            List<Enrollment> enrollments = new List<Enrollment>();

            //foreach (dynamic dto in dynamicResult)
            foreach (EnrollmentDto dto in result)
            { 
                enrollments.Add(await ToEnrollment(dto));
            }

            return enrollments;

            //return result.Select(async r => await ToEnrollment(r)).Cast<Enrollment>();
        }

        public async Task<Enrollment> GetByIdAsync(int id)
        {
            string sql = """
                SELECT Id, EmployeeId, TrainingId, IsBillable FROM Enrollments WHERE Id = @Id
                """;

            EnrollmentDto dto;

            using (IDbConnection connection = _factory.CreateConnection())
            {
                dto = await connection.QueryFirstAsync<EnrollmentDto>(sql);
            }

            return await ToEnrollment(dto);
        }

        private async Task<Enrollment> ToEnrollment(EnrollmentDto dto)
        {
            Enrollment enrollment = new Enrollment
            {
                Id = dto.Id,
                IsBillable = dto.IsBillable,
                Training = await _trainingRepository.GetByIdAsync(dto.TrainingId),
                Employee = await _employeeRepository.GetByIdAsync(dto.EmployeeId)
            };

            return enrollment;
        }

        public async Task<IEnumerable<Enrollment>> GetByEmployeeIdAsync(int employeeId)
        {
            string sql = """
                SELECT Id, EmployeeId, TrainingId, IsBillable 
                FROM Enrollments 
                WHERE EmployeeId = @EmployeeId
                """;

            IEnumerable<EnrollmentDto> result;
            //IEnumerable<dynamic> dynamicResult;

            using (IDbConnection connection = _factory.CreateConnection())
            {
                result = await connection.QueryAsync<EnrollmentDto>(sql, new { EmployeeId = employeeId});
                //dynamicResult = await connection.QueryAsync<dynamic>(sql);
                //dynamicResult = await connection.QueryAsync(sql);
            }

            List<Enrollment> enrollments = new List<Enrollment>();

            //foreach (dynamic dto in dynamicResult)
            foreach (EnrollmentDto dto in result)
            {
                enrollments.Add(await ToEnrollment(dto));
            }

            return enrollments;

            //return result.Select(async r => await ToEnrollment(r)).Cast<Enrollment>();
        }

        public async Task AddAsync(Enrollment enrollment)
        {
            string sql = """
                INSERT INTO Enrollments (EmployeeId, TrainingId, IsBillable) 
                VALUES (@EmployeeId, @TrainingId, @IsBillable); 
                SELECT CAST(SCOPE_IDENTITY() as int)
                """;

            using (IDbConnection connection = _factory.CreateConnection())
            {
                EnrollmentDto dto = new EnrollmentDto
                {
                    TrainingId = enrollment.Training.Id,
                    EmployeeId = enrollment.Employee.Id,
                    IsBillable = enrollment.IsBillable
                };
                enrollment.Id = await connection.ExecuteScalarAsync<int>(sql, dto);
            }
        }

        public async Task UpdateAsync(Enrollment enrollment)
        {
            string sql = """
                UPDATE Enrollments 
                SET EmployeeId = @EmployeeId, TrainingId = @TrainingId, IsBillable = @IsBillable 
                WHERE Id = @Id
                """;

            using (IDbConnection connection = _factory.CreateConnection())
            {
                EnrollmentDto dto = new EnrollmentDto
                {
                    Id = enrollment.Id,
                    TrainingId = enrollment.Training.Id,
                    EmployeeId = enrollment.Employee.Id,
                    IsBillable = enrollment.IsBillable
                };
                await connection.ExecuteAsync(sql, dto);
            }
        }

        public async Task DeleteAsync(int id)
        {
            string sql = """
                DELETE FROM Enrollments 
                WHERE Id = @Id
                """;

            using (IDbConnection connection = _factory.CreateConnection())
            {
                await connection.ExecuteAsync(sql, new { Id = id });
            }
        }
    }
}
