using MMSL.Domain.Entities.Products;

namespace MMSL.Domain.DataContracts.Products {
    public class ProductAvailabilitiesDataContract : EntityDataContractBase<DealerProductAvailability> {
        public long ProductCategoryId { get; set; }
        public long DealerAccountId { get; set; }
        public bool IsDisabled { get; set; }

        public override DealerProductAvailability GetEntity() {
            return new DealerProductAvailability() {
                Id = Id,
                DealerAccountId = DealerAccountId,
                ProductCategoryId = ProductCategoryId,
                IsDisabled = IsDisabled
            };
        }
    }
}
