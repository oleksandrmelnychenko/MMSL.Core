using MMSL.Domain.DataContracts.Products;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MMSL.Services.ProductCategories.Contracts {
    public interface IDealerProductAvailabilityService {
        Task SaveProductAvailabilitySettings(IEnumerable<ProductAvailabilitiesDataContract> settings);
    }
}
