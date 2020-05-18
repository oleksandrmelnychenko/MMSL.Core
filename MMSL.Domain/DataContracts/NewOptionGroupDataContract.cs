using System.ComponentModel.DataAnnotations;

namespace MMSL.Domain.DataContracts {
    public class NewOptionGroupDataContract {        
        public string Name { get; set; }

        public bool IsMandatory { get; set; }
    }
}
