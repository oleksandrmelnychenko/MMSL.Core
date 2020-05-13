using MMSL.Domain.Entities;
using System.Collections.Generic;

namespace MMSL.Domain.EntityHelpers {
    public class PaginatingResult<TEntity> where TEntity : EntityBase {
        public ICollection<TEntity> Entities { get; set; }
        public PaginationInfo PaginationInfo { get; set; }
    }
}
