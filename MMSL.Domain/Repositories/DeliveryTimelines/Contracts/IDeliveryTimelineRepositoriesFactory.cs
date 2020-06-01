using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace MMSL.Domain.Repositories.DeliveryTimelines.Contracts {
    public interface IDeliveryTimelineRepositoriesFactory {
        IDeliveryTimelineRepository NewDeliveryTimelineRepository(IDbConnection connection);

        IDeliveryTimelineProductMapRepository NewDeliveryTimelineProductMapRepository(IDbConnection connection);
    }
}
