using Dapper;
using MMSL.Domain.DataContracts.DeliveryTimelines;
using MMSL.Domain.Entities.DeliveryTimelines;
using MMSL.Domain.Repositories.DeliveryTimelines.Contracts;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMSL.Domain.Repositories.DeliveryTimelines {
    public class DeliveryTimelineRepository : IDeliveryTimelineRepository {

        private readonly IDbConnection _connection;

        public DeliveryTimelineRepository(IDbConnection connection) {
            _connection = connection;
        }

        public List<DeliveryTimeline> GetAll(string searchPhrase, bool isDefault) =>
             _connection.Query<DeliveryTimeline>(
                "SELECT * " +
                "FROM [DeliveryTimelines] " +
                "WHERE [DeliveryTimelines].[IsDeleted] = 0 AND IsDefault = @IsDefault AND PATINDEX('%' + @SearchTerm + '%', [DeliveryTimelines].Name) > 0 ",
                new {
                    SearchTerm = string.IsNullOrEmpty(searchPhrase) ? string.Empty : searchPhrase,
                    IsDefault = isDefault
                }).ToList();

        public List<DeliveryTimeline> GetAllByProductId(long id) =>
            _connection.Query<DeliveryTimeline>(
                "SELECT * " +
                "FROM [DeliveryTimelines] AS d " +
                "LEFT JOIN [DeliveryTimelineProductMaps] AS dpm ON dpm.DeliveryTimelineId = d.Id	AND dpm.IsDeleted = 0 " +
                "WHERE d.IsDeleted = 0 AND dpm.ProductCategoryId = @Id",
                new { Id = id }).ToList();

        public DeliveryTimeline GetById(long id) =>
             _connection.QuerySingleOrDefault<DeliveryTimeline>(
                 "SELECT * " +
                 "FROM [DeliveryTimelines] " +
                 "WHERE Id = @Id",
                 new { Id = id });

        public DeliveryTimeline New(DeliveryTimeline deliveryTimeline) =>
            _connection.QuerySingleOrDefault<DeliveryTimeline>(
                "INSERT INTO [DeliveryTimelines]([IsDeleted],[Name],[Ivory],[Silver],[Black],[Gold],[IsDefault]) " +
                "VALUES(0,@Name,@Ivory,@Silver,@Black,@Gold,@IsDefault) " +
                "SELECT * " +
                "FROM [DeliveryTimelines] " +
                "WHERE [DeliveryTimelines].Id = SCOPE_IDENTITY()", deliveryTimeline);

        public int Update(DeliveryTimeline deliveryTimeline) =>
            _connection.Execute(
                "UPDATE [DeliveryTimelines] " +
                "SET IsDeleted=@IsDeleted,[Name]=@Name,[Ivory]=@Ivory,[Silver]=@Silver,[Black]=@Black, " +
                "[Gold]=@Gold,LastModified = getutcdate() " +
                "WHERE [DeliveryTimelines].Id = @Id;", deliveryTimeline);
    }
}
