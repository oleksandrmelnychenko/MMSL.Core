using System.Collections.Generic;

namespace MMSL.Domain.DataContracts.Filters {
    public class FilterItem {
        public string Name { get; private set; }

        public float Min { get; set; }
        public float Max { get; set; }

        public bool IsRange { get; private set; }

        public List<FabricFilterValue> Values { get; set; } = new List<FabricFilterValue>();

        public FilterItem(string name, bool isRange = false) {
            Name = name;
            IsRange = isRange;
        }
    }
}
