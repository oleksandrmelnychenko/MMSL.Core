using Dapper;
using MMSL.Domain.Entities.Fabrics;
using MMSL.Domain.EntityHelpers;
using MMSL.Domain.Repositories.Fabrics.Contracts;
using System.Data;
using System.Linq;

namespace MMSL.Domain.Repositories.Fabrics {
    class FabricRepository : IFabricRepository {
        private readonly IDbConnection _connection;

        public FabricRepository(IDbConnection connection) {
            this._connection = connection;
        }

        public PaginatingResult<Fabric> GetPagination(int pageNumber, int limit, string searchPhrase) {
            PaginatingResult<Fabric> result = new PaginatingResult<Fabric>();

            PagerParams pager = new PagerParams(pageNumber, limit, searchPhrase);

            string paginatingDetailQuery = 
@"SELECT TOP(1) 
[PageSize]= @Limit,
[PageNumber] = (@Offset / @Limit) + 1, 
[TotalItems] = COUNT(DISTINCT [Fabrics].Id), 
[PagesCount] = CEILING(CONVERT(float, COUNT(DISTINCT[Fabrics].Id)) / @Limit) 
FROM [Fabrics] 
WHERE [Fabrics].[IsDeleted] = 0";

            string query =
@";WITH [Paginated_Fabrics_CTE] AS ( 
SELECT [Fabrics].Id, ROW_NUMBER() OVER(ORDER BY [Fabrics].[FabricCode]) AS [RowNumber] 
FROM [Fabrics] 
WHERE [Fabrics].IsDeleted = 0 
)
SELECT [Paginated_Fabrics_CTE].RowNumber, [Fabrics].*
FROM [Fabrics]  
LEFT JOIN [Paginated_Fabrics_CTE] ON [Paginated_Fabrics_CTE].Id = [Fabrics].Id  
WHERE [Fabrics].IsDeleted = 0  
AND [Paginated_Fabrics_CTE].RowNumber > @Offset  
AND [Paginated_Fabrics_CTE].RowNumber <= @Offset + @Limit  
ORDER BY [Paginated_Fabrics_CTE].RowNumber";

            if (!string.IsNullOrEmpty(searchPhrase)) {
                string searchPart = 
@"AND (
PATINDEX('%' + @SearchTerm + '%', [DealerAccount].CompanyName) > 0 
OR PATINDEX('%' + @SearchTerm + '%', [DealerAccount].Email) > 0 
OR PATINDEX('%' + @SearchTerm + '%', [DealerAccount].PhoneNumber) > 0 
)";

                paginatingDetailQuery += searchPart;
                query += searchPart;
            }

            result.Entities = _connection.Query<Fabric>(query,
                new {
                    pager.Offset,
                    pager.Limit,
                    pager.SearchTerm,
                })
                .ToList();

            result.PaginationInfo = _connection.QuerySingle<PaginationInfo>(paginatingDetailQuery,
                new {
                    pager.Offset,
                    pager.Limit,
                    pager.SearchTerm,
                });

            return result;
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
