using Dapper;
using MMSL.Domain.Entities.Fabrics;
using MMSL.Domain.Repositories.Fabrics.Contracts;
using System.Data;

namespace MMSL.Domain.Repositories.Fabrics {
    class FabricRepository : IFabricRepository {
        private readonly IDbConnection _connection;

        public FabricRepository(IDbConnection connection) {
            this._connection = connection;
        }

        public Fabric GetById(long fabricId) =>
            _connection.QuerySingleOrDefault<Fabric>(
@"SELECT [Fabrics].*
FROM[Fabrics]
WHERE Id = @Id",
                new { Id = fabricId }
                );

        //TODO: hide properties if need
        public Fabric GetByIdForDealer(long fabricId) =>
            _connection.QuerySingleOrDefault<Fabric>(
@"SELECT [Fabrics].*
FROM[Fabrics]
WHERE Id = @Id",
                new { Id = fabricId }
                );

        public Fabric AddFabric(Fabric fabricEntity) {
            long id = _connection.QuerySingleOrDefault<long>(
@"INSERT INTO [Fabrics]([IsDeleted],[IsMetresVisible],[IsMillVisible],[IsColorVisible],[IsCompositionVisible],[IsGSMVisible],[IsCountVisible],[IsWeaveVisible],[IsPatternVisible]
,[FabricCode],[Description],[ImageUrl],[Status],[Metres],[Mill],[Color],[Composition],[GSM],[Count],[Weave],[Pattern])
VALUES (0,1,1,1,1,1,1,1
,@FabricCode,@Description,@ImageUrl,@Status,@Metres,@Mill,@Color,@Composition,@GSM,@Count,@Weave,@Pattern);
SELECT SCOPE_IDENTITY()",
                fabricEntity
                );

            return GetById(id);
        }

        public Fabric UpdateFabric(Fabric fabricEntity) {
            _connection.Execute(
@"UPDATE [Fabrics] SET [IsDeleted]= @IsDeleted
,[IsMetresVisible] = @IsMetresVisible
,[IsMillVisible] = @IsMillVisible
,[IsColorVisible]= @IsColorVisible
,[IsCompositionVisible]= @IsCompositionVisible
,[IsGSMVisible]= @IsGSMVisible
,[IsCountVisible]= @IsCountVisible
,[IsWeaveVisible]= @IsWeaveVisible
,[IsPatternVisible]= @IsPatternVisible
,[FabricCode]= @FabricCode
,[Description]= @Description
,[ImageUrl]= @ImageUrl
,[Status]= @Status
,[Metres]= @Metres
,[Mill]= @Mill
,[Color]= @Color
,[Composition]= @Composition
,[GSM]= @GSM
,[Count]= @Count
,[Weave]= @Weave
,[Pattern]= @Pattern
WHERE [Id] = @Id",
                    fabricEntity
                    );

            return GetById(fabricEntity.Id);
        }
    }
}
