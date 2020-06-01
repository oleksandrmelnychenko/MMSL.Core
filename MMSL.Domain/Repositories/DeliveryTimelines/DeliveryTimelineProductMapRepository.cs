using Dapper;
using MMSL.Domain.Entities.DeliveryTimelines;
using MMSL.Domain.Repositories.DeliveryTimelines.Contracts;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace MMSL.Domain.Repositories.DeliveryTimelines {
    public class DeliveryTimelineProductMapRepository : IDeliveryTimelineProductMapRepository {

        private readonly IDbConnection _connection;

        public DeliveryTimelineProductMapRepository(IDbConnection connection) {
            _connection = connection;
        }

        public DeliveryTimelineProductMap GetByIds(long productCategoryId, long deliveryTimelineId) =>
            _connection.QuerySingleOrDefault<DeliveryTimelineProductMap>(
                "SELECT * " +
                "FROM [DeliveryTimelineProductMaps] " +
                "WHERE [DeliveryTimelineId] = @DeliveryTimelineId AND [ProductCategoryId] = @ProductCategoryId", new { ProductCategoryId = productCategoryId, DeliveryTimelineId = deliveryTimelineId });

        public long New(long productCategoryId, long deliveryTimeLineId) =>
            _connection.Query<long>(
                "INSERT INTO [DeliveryTimelineProductMaps] ([IsDeleted],[DeliveryTimelineId],[ProductCategoryId]) " +
                "VALUES (0, @DeliveryTimelineId,@ProductCategoryId) " +
                "SELECT SCOPE_IDENTITY()", new {
                    ProductCategoryId = productCategoryId,
                    DeliveryTimeLineId = deliveryTimeLineId
                }).SingleOrDefault();

        public void Update(DeliveryTimelineProductMap existedDeliveryTimelineProductMap) =>
            _connection.Execute(
                "UPDATE [DeliveryTimelineProductMaps] " +
                "SET IsDeleted=@IsDeleted,[ProductCategoryId]=@ProductCategoryId,[DeliveryTimelineId]=@DeliveryTimelineId,LastModified = getutcdate() " +
                "WHERE [DeliveryTimelineProductMaps].Id = @Id", existedDeliveryTimelineProductMap);
    }
}
