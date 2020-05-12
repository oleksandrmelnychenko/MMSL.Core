using System.ComponentModel.DataAnnotations;

namespace MMSL.Domain.Entities.BankDetails {
    public class BankDetail : EntityBase {
        [Required]
        public string Name { get; set; }

        [Required]
        public int AccountNo { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        public string SwiftBic { get; set; }

        [Required]
        public string Iban { get; set; }

        [Required]
        public int VatNumber { get; set; }
    }
}
