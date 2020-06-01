using Dapper;
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

        public long New(long productCategoryId, long deliveryTimeLineId) =>
            _connection.Query<long>(
                "INSERT INTO [DeliveryTimelineProductMaps] ([IsDeleted],[DeliveryTimelineId],[ProductCategoryId]) " +
                "VALUES (0, @DeliveryTimelineId,@ProductCategoryId) " +
                "SELECT SCOPE_IDENTITY()", new {
                    ProductCategoryId = productCategoryId,
                    DeliveryTimeLineId = deliveryTimeLineId
                }).SingleOrDefault();
    }
}
