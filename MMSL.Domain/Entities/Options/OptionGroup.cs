using System.Collections;
using System.Collections.Generic;

namespace MMSL.Domain.Entities.Options {
    public class OptionGroup : EntityBase {

        public OptionGroup() {
            OptionUnits = new HashSet<OptionUnit>();
        }

        public string Name { get; set; }

        public ICollection<OptionUnit> OptionUnits { get; set; }
    }
}
