using MMSL.Domain.Entities.Dealer;
using MMSL.Domain.Entities.Measurements;
using MMSL.Domain.Entities.Products;
using System.Collections.Generic;

namespace MMSL.Domain.Entities.StoreCustomers {
    public class CustomerProductProfile : EntityBaseNamed {

        public ProfileTypes ProfileType { get; set; }

        public long DealerAccountId { get; set; }
        public DealerAccount DealerAccount { get; set; }

        public long StoreCustomerId { get; set; }
        public StoreCustomer StoreCustomer { get; set; }

        public long ProductCategoryId { get; set; }
        public ProductCategory ProductCategory { get; set; }

        public long? MeasurementId { get; set; }
        public Measurement Measurement { get; set; }

        public long? FittingTypeId { get; set; }
        public FittingType FittingType { get; set; }

        public long? MeasurementSizeId { get; set; }
        public MeasurementSize MeasurementSize { get; set; }

        public ICollection<CustomerProfileSizeValue> CustomerProfileSizeValues { get; set; } = new HashSet<CustomerProfileSizeValue>();
    }
}
