using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace MMSL.Domain.DataContracts {
    public class NewProductCategoryDataContract {
        public string Name { get; set; }

        public string Description { get; set; }

        public List<long> OptionGroupIds { get; set; }

        //public IFormFile File { get; set; }

    }
}
