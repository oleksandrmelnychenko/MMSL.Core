using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace MMSL.Domain.Entities.Options {
    public class OptionUnit : EntityBase {

        public int OrderIndex { get; set; }

        public string Value { get; set; }

        public string ImageUrl { get; set; }

        public string ImageName { get; set; }

        public bool IsMandatory { get; set; }

        public long OptionGroupId { get; set; }

        [NotMapped]
        public bool IsAllow { get; set; }

        public OptionGroup OptionGroup { get; set; }

        public ICollection<UnitValue> UnitValues { get; set; } = new HashSet<UnitValue>();

        public ICollection<OptionPrice> OptionPrices { get; set; } = new HashSet<OptionPrice>();

        [NotMapped]
        public OptionPrice CurrentPrice { get; set; }

        [NotMapped]
        public bool CanDeclareOwnPrice { get; set; }
    }
}
