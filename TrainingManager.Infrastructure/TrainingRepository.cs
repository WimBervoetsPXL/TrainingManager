using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using TrainingManager.Domain;

namespace TrainingManager.Infrastructure
{
    public class TrainingRepository
    {
        private readonly DbConnectionFactory _factory;

        public TrainingRepository(DbConnectionFactory factory)
        {
            _factory = factory;
        }

        public async Task<IEnumerable<Training>> GetAllAsync()
        {
            using (IDbConnection connection = _factory.CreateConnection())
            {
                return await connection.QueryAsync<Training>("SELECT Id, Title, TrainerName, DurationInHours, TrainingType, Technology, RiskLevel FROM Trainings");
            }
        }

        public async Task<Training> GetByIdAsync(string id)
        {
            using (IDbConnection connection = _factory.CreateConnection())
            {
                string sql = $"""
                    SELECT Id, Title, TrainerName, DurationInHours, TrainingType, Technology, RiskLevel 
                    FROM Trainings 
                    WHERE Id = @Id
                    """;
                return await connection.QueryFirstOrDefaultAsync<Training>(sql, new { Id = id });
            }
        }

        public async Task AddAsync(Training training)
        {
            using (IDbConnection connection = _factory.CreateConnection())
            {
                string sql = """
                    INSERT INTO Trainings(Id, Title, TrainerName, DurationInHours) 
                    VALUES(@Id, @Title, @TrainerName, @DurationInHours)
                    """;

                //string technical = "INSERT INTO Trainings(Id, Title, TrainerName, DurationInHours, TrainingType, Technology) VALUES(@Id, @Title, @TrainerName, @DurationInHours, 'Technical', @Technology)";
                //string safety = "INSERT INTO Trainings(Id, Title, TrainerName, DurationInHours, TrainingType, RiskLevel) VALUES(@Id, @Title, @TrainerName, @DurationInHours, 'Safety', @RiskLevel)";

                int newId = await connection.ExecuteScalarAsync<int>(sql, training);
                training.Id = newId;
            }
        }

        public async Task UpdateAsync(Training training)
        {
            using (IDbConnection connection = _factory.CreateConnection())
            {
                string sql = """
                    UPDATE Trainings 
                    SET Title = @Title, TrainerName = @TrainerName, DurationInHours = @DurationInHours
                    WHERE Id = @Id
                """;

                await connection.ExecuteAsync(sql, training);
            }
        }

        public async Task DeleteAsync(string id)
        {
            using (IDbConnection connection = _factory.CreateConnection())
            {
                string sql = """
                    DELETE FROM Trainings 
                    WHERE Id = @Id
                """;

                await connection.ExecuteAsync(sql, new { Id = id });
            }
        }
    }
}
