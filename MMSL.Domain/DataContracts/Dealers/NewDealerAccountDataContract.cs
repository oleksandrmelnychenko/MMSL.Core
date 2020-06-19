using MMSL.Domain.Entities.Dealer;
using System;
using System.Collections.Generic;
using System.Text;

namespace MMSL.Domain.DataContracts.Dealers {
    public class NewDealerAccountDataContract : EntityDataContractBase<DealerAccount> {

        public string Name { get; set; }

        public string CompanyName { get; set; }

        public string Email { get; set; }

        public string AlternateEmail { get; set; }

        public string PhoneNumber { get; set; }

        public string TaxNumber { get; set; }

        public long CurrencyTypeId { get; set; }

        public long PaymentTypeId { get; set; }

        public bool IsVatApplicable { get; set; }

        public bool IsCreditAllowed { get; set; }

        public string Password { get; set; }

        public override DealerAccount GetEntity() {
            return new DealerAccount {
                Name = Name,
                CompanyName = CompanyName,
                Email = Email,
                AlternateEmail = AlternateEmail,
                PhoneNumber = PhoneNumber,
                TaxNumber = TaxNumber,
                IsCreditAllowed = IsCreditAllowed,
                IsVatApplicable = IsVatApplicable,

                CurrencyTypeId = CurrencyTypeId,
                PaymentTypeId = PaymentTypeId,

                TempPassword = Password
            };
        }
    }
}
