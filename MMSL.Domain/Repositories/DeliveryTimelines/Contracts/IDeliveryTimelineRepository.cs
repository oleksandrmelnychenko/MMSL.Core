using MMSL.Domain.DataContracts.DeliveryTimelines;
using MMSL.Domain.Entities.DeliveryTimelines;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MMSL.Domain.Repositories.DeliveryTimelines.Contracts {
    public interface IDeliveryTimelineRepository {
        List<DeliveryTimeline> GetAll(string searchPhrase, bool isDefault);

        DeliveryTimeline New(DeliveryTimeline deliveryTimeline);

        DeliveryTimeline GetById(long id);

        int Update(DeliveryTimeline deliveryTimeline);
    }
}
