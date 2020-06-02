using MMSL.Domain.DataContracts.DeliveryTimelines;
using MMSL.Domain.Entities.DeliveryTimelines;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MMSL.Services.DeliveryTimelines.Contracts {
    public interface IDeliveryTimelineService {
        Task<List<DeliveryTimeline>> GetDeliveryTimelinesAsync(string searchPhrase, bool isDefault);

        Task<DeliveryTimeline> NewDeliveryTimelineAsync(NewDeliveryTimelineDataContract newDeliveryTimelineDataContract);

        Task UpdateDeliveryTimelineAsync(DeliveryTimeline deliveryTimeline);

        Task DeleteDeliveryTimelineAsync(long deliveryId);

        Task<List<DeliveryTimeline>> AssignDeliveryTimelineAsync(AssignDeliveryTimelineDataContract assignDeliveryTimelineDataContract);

        Task<List<DeliveryTimeline>> GetDeliveryTimelinesByProductAsync(long id);
    }
}
