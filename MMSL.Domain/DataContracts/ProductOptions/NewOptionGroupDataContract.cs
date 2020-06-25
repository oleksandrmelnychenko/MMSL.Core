using System.ComponentModel.DataAnnotations;

namespace MMSL.Domain.DataContracts.ProductOptions {
    public class NewOptionGroupDataContract {
        public long ProductId { get; set; }

        public string Name { get; set; }

        public bool IsMandatory { get; set; }

        public decimal Price { get; set; }

        public long CurrencyTypeId { get; set; }
    }
}
