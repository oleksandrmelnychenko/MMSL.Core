using Dapper;
using MMSL.Domain.Entities.Options;
using MMSL.Domain.Repositories.Options.Contracts;
using System.Data;

namespace MMSL.Domain.Repositories.Options {
    class UnitValuesRepository : IUnitValuesRepository {

        private readonly IDbConnection _connection;

        public UnitValuesRepository(IDbConnection connection) {
            _connection = connection;
        }

        public long AddUnitValue(UnitValue value) =>
            _connection.QuerySingleOrDefault<long>(
@"INSERT INTO [UnitValues] ([IsDeleted],[Created],[Value],[OptionUnitId]) 
VALUES (0,GETUTCDATE(),@Value,@OptionUnitId)",
                value);

        public UnitValue GetUnitValue(long unitValueId) {
            throw new System.NotImplementedException();
        }

        public UnitValue GetUnitValuesByUnitId(long optionUnitId) {
            throw new System.NotImplementedException();
        }

        public UnitValue UpdateUnitValue(UnitValue value) =>
            _connection.QuerySingleOrDefault<UnitValue>(
@"UPDATE [UnitValues] 
SET [LastModified]=GETUTCDATE(), [Value]=@Value, [IsDeleted]=@IsDeleted 
WHERE [Id] = @Id;
SELECT TOP(1) * FROM [UnitValues]
WHERE [Id] = @Id",
                value);
    }
}
