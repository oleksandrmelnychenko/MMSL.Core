using MMSL.Domain.Entities.Products;

namespace MMSL.Domain.Repositories.ProductRepositories.Contracts {
    public interface IDealerProductAvailabilityRepository {
        long AddAvailability(DealerProductAvailability dealerProductAvailability);
        void UpdateAvailability(DealerProductAvailability dealerProductAvailability);
        void DeleteAvailability(DealerProductAvailability dealerProductAvailability);
    }
}
