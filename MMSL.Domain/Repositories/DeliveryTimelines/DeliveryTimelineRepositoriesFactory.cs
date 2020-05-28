using MMSL.Domain.Repositories.DeliveryTimelines.Contracts;
using System.Data;

namespace MMSL.Domain.Repositories.DeliveryTimelines {
    public class DeliveryTimelineRepositoriesFactory : IDeliveryTimelineRepositoriesFactory {
        public IDeliveryTimelineRepository NewDeliveryTimelineRepository(IDbConnection connection) =>
            new DeliveryTimelineRepository(connection);
    }
}
