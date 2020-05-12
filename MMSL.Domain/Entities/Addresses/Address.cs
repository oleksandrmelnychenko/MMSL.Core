using System.ComponentModel;

namespace MMSL.Domain.Entities.Addresses {
    public class Address : EntityBase {

        [Description("Street")]
        public string AddressLine1 { get; set; }

        [Description("Place Details")]
        public string AddressLine2 { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string Country { get; set; }

        public string ZipCode { get; set; }
    }
}
