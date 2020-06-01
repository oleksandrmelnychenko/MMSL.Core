using MMSL.Domain.Entities.DeliveryTimelines;
using System;
using System.Collections.Generic;
using System.Text;

namespace MMSL.Domain.Repositories.DeliveryTimelines.Contracts {
    public interface IDeliveryTimelineProductMapRepository {
        long New(long productCategoryId, long deliveryTimeLineId);

        DeliveryTimelineProductMap GetByIds(long productCategoryId, long deliveryTimelineId);

        void Update(DeliveryTimelineProductMap existedDeliveryTimelineProductMap);
    }
}
