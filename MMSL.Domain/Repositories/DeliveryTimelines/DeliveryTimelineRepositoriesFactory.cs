using MMSL.Domain.Repositories.DeliveryTimelines.Contracts;
using System.Data;

namespace MMSL.Domain.Repositories.DeliveryTimelines {
    public class DeliveryTimelineRepositoriesFactory : IDeliveryTimelineRepositoriesFactory {
        public IDeliveryTimelineProductMapRepository NewDeliveryTimelineProductMapRepository(IDbConnection connection) =>
            new DeliveryTimelineProductMapRepository(connection);

        public IDeliveryTimelineRepository NewDeliveryTimelineRepository(IDbConnection connection) =>
            new DeliveryTimelineRepository(connection);
    }
}
