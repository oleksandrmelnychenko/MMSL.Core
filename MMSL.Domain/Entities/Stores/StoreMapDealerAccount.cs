using MMSL.Domain.Entities.Dealer;

namespace MMSL.Domain.Entities.Stores {
    public class StoreMapDealerAccount : EntityBase {

        public long DealerAccountId { get; set; }

        public long StoreId { get; set; }

        public DealerAccount DealerAccount { get; set; }

        public Store Store { get; set; }
    }
}
