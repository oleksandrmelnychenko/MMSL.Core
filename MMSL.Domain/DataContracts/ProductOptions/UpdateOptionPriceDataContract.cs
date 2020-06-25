using System;
using System.Collections.Generic;
using System.Text;

namespace MMSL.Domain.DataContracts.ProductOptions {
    public class UpdateOptionPriceDataContract : NewOptionPriceDataContract {
        public long Id { get; set; }
        public bool IsDeleted { get; set; }
    }
}
