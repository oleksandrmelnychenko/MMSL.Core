using Dapper;
using MMSL.Common.Helpers;
using MMSL.Domain.DataContracts.Fabrics;
using MMSL.Domain.DataContracts.Filters;
using MMSL.Domain.Entities.Fabrics;
using MMSL.Domain.EntityHelpers;
using MMSL.Domain.Repositories.Fabrics.Contracts;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace MMSL.Domain.Repositories.Fabrics {
    class FabricRepository : IFabricRepository {
        private readonly IDbConnection _connection;

        public FabricRepository(IDbConnection connection) {
            this._connection = connection;
        }

        public PaginatingResult<Fabric> GetPagination(int pageNumber, int limit, string searchPhrase, FilterItem[] filters) {
            PaginatingResult<Fabric> result = new PaginatingResult<Fabric>();

            PagerParams pager = new PagerParams(pageNumber, limit, searchPhrase);


            string searchPart =
@"AND (
PATINDEX('%' + @SearchTerm + '%', [Fabrics].[FabricCode]) > 0 
OR PATINDEX('%' + @SearchTerm + '%', [Fabrics].[Description]) > 0 
)";

            string paginatingDetailQuery =
$@"SELECT TOP(1) 
[PageSize]= @Limit,
[PageNumber] = (@Offset / @Limit) + 1, 
[TotalItems] = COUNT(DISTINCT [Fabrics].Id), 
[PagesCount] = CEILING(CONVERT(float, COUNT(DISTINCT[Fabrics].Id)) / @Limit) 
FROM [Fabrics] 
WHERE [Fabrics].[IsDeleted] = 0 
{(!string.IsNullOrEmpty(searchPhrase) ? searchPart : string.Empty)}";

            string query =
$@";WITH [Paginated_Fabrics_CTE] AS ( 
SELECT [Fabrics].Id, ROW_NUMBER() OVER(ORDER BY [Fabrics].[FabricCode]) AS [RowNumber] 
FROM [Fabrics] 
WHERE [Fabrics].IsDeleted = 0 
{(!string.IsNullOrEmpty(searchPhrase) ? searchPart : string.Empty)}
)
SELECT [Paginated_Fabrics_CTE].RowNumber
,[Fabrics].[Id]
,[IsDeleted]
,[Created]
,[LastModified]
,[FabricCode]
,[Description]
,[ImageUrl]
,[Status]
,IIF([IsMetresVisible]=1,[Metres],NULL) AS [Metres]
,[IsMetresVisible]
,IIF([IsMillVisible]=1,[Mill],NULL) AS [Mill]
,[IsMillVisible]
,IIF([IsColorVisible]=1,[Color],NULL) AS [Color]
,[IsColorVisible]
,IIF([IsCompositionVisible]=1,[Composition],NULL) AS [Composition]
,[IsCompositionVisible]
,IIF([IsGSMVisible]=1,[GSM],NULL) AS [GSM]
,[IsGSMVisible]
,IIF([IsCountVisible]=1,[Count],NULL) AS [Count]
,[IsCountVisible]
,IIF([IsWeaveVisible]=1,[Weave],NULL) AS [Weave]
,[IsWeaveVisible]
,IIF([IsPatternVisible]=1,[Pattern],NULL) AS [Pattern]
,[IsPatternVisible]
FROM [Fabrics]  
LEFT JOIN [Paginated_Fabrics_CTE] ON [Paginated_Fabrics_CTE].Id = [Fabrics].Id  
WHERE [Fabrics].IsDeleted = 0  
AND [Paginated_Fabrics_CTE].RowNumber > @Offset  
AND [Paginated_Fabrics_CTE].RowNumber <= @Offset + @Limit";

            if (filters != null) {
                foreach (FilterItem filter in filters) {
                    if (filter.IsRange) {
                        //string

                        //NumericParsingHelper.TryParseFloat()
                        string filterRangeQueryPart = $" AND CONVERT(float, {filter.Name}) <= CONVERT(float, '{filter.Max.ToString(System.Globalization.CultureInfo.InvariantCulture)}') AND CONVERT(float, {filter.Name}) >= CONVERT(float, '{filter.Min.ToString(System.Globalization.CultureInfo.InvariantCulture)}') ";

                        paginatingDetailQuery += filterRangeQueryPart;
                        query += filterRangeQueryPart;
                    } else if (filter.Values != null && filter.Values.Count > 0) {
                        foreach (FabricFilterValue filterValue in filter.Values.Where(x => x.Applied)) {
                            string filterValueTemplate = $" AND {filter.Name} = '{filterValue.Value}' ";

                            paginatingDetailQuery += filterValueTemplate;
                            query += filterValueTemplate;
                        }
                    }
                }
            }

            query += " ORDER BY [Paginated_Fabrics_CTE].RowNumber ";

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
,[FabricCode],[Description],[ImageUrl],[Status],[Metres],[Mill],[Color],[Composition],[GSM],[Count],[Weave],[Pattern],[UserIdentityId])
VALUES (0,1,1,1,1,1,1,1,1
,@FabricCode,@Description,@ImageUrl,@Status,@Metres,@Mill,@Color,@Composition,@GSM,@Count,@Weave,@Pattern,@UserIdentityId);
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

        public List<FilterItem> GetFilters() {
            List<FilterItem> filters = new List<FilterItem> {
                new FilterItem("Color"),
                new FilterItem("Mill"),
                new FilterItem("Composition"),
                new FilterItem("GSM"),
                new FilterItem("Weave"),
                new FilterItem("Pattern"),
                new FilterItem("Metres", true),
                new FilterItem("Count", true)
            };

            Type[] columnQueryTypes = new Type[] { typeof(FilterQueryResult) };
            Func<object[], FilterQueryResult> columnQueryMapper = (objects) => {
                FilterQueryResult result = (FilterQueryResult)objects[0];

                FilterItem filter = filters.FirstOrDefault(x => x.Name == result.Name);

                if (filter != null) {
                    filter.Values.Add(new FabricFilterValue(result.Value));
                }

                return result;
            };

            Type[] rangeQueryTypes = new Type[] { typeof(FilterQueryResult) };
            Func<object[], FilterQueryResult> rangeQueryMapper = (objects) => {
                FilterQueryResult result = (FilterQueryResult)objects[0];

                FilterItem filter = filters.FirstOrDefault(x => x.Name == result.Name);

                if (filter != null) {
                    filter.Max = result.Max;
                    filter.Min = result.Min;
                }

                return result;
            };

            string columnReplacer = "{COLUMN_NAME}";
            string rangeQuery = "SELECT TOP(1) '{COLUMN_NAME}' AS [Name], MAX({COLUMN_NAME}) AS [Max], MIN({COLUMN_NAME}) AS [Min] FROM [Fabrics] ";
            string columnQuery = "SELECT '{COLUMN_NAME}' AS [Name], {COLUMN_NAME} AS [Value] FROM [Fabrics] GROUP BY {COLUMN_NAME} ";

            StringBuilder builder = new StringBuilder();

            foreach (FilterItem filter in filters) {
                if (filter.IsRange) {
                    builder.Append(rangeQuery.Replace(columnReplacer, filter.Name));
                } else {
                    builder.Append(columnQuery.Replace(columnReplacer, filter.Name));
                }
            }

            using (SqlMapper.GridReader reader = _connection.QueryMultiple(builder.ToString())) {
                reader.Read(columnQueryTypes, columnQueryMapper);
                reader.Read(columnQueryTypes, columnQueryMapper);
                reader.Read(columnQueryTypes, columnQueryMapper);
                reader.Read(columnQueryTypes, columnQueryMapper);
                reader.Read(columnQueryTypes, columnQueryMapper);
                reader.Read(columnQueryTypes, columnQueryMapper);
                reader.Read(rangeQueryTypes, rangeQueryMapper);
                reader.Read(rangeQueryTypes, rangeQueryMapper);
            }

            return filters;
        }

        public void UpdateFabricVisibilities(UpdateFabricVisibilitiesDataContract fabric, long userIdentityId) {
            string query = @"UPDATE [Fabrics] SET 
[IsMetresVisible] = @IsMetresVisible
,[IsMillVisible] = @IsMillVisible
,[IsColorVisible]= @IsColorVisible
,[IsCompositionVisible]= @IsCompositionVisible
,[IsGSMVisible]= @IsGSMVisible
,[IsCountVisible]= @IsCountVisible
,[IsWeaveVisible]= @IsWeaveVisible
,[IsPatternVisible]= @IsPatternVisible
WHERE [UserIdentityId] = @UserIdentityId
AND [IsDeleted]= 0";

            _connection.Execute(query,
                new {
                    IsMetresVisible = fabric.IsColorVisible,
                    IsMillVisible = fabric.IsColorVisible,
                    IsColorVisible = fabric.IsColorVisible,
                    IsCompositionVisible = fabric.IsColorVisible,
                    IsGSMVisible = fabric.IsColorVisible,
                    IsCountVisible = fabric.IsColorVisible,
                    IsWeaveVisible = fabric.IsColorVisible,
                    IsPatternVisible = fabric.IsColorVisible,
                    UserIdentityId = userIdentityId
                });
        }
    }
}
