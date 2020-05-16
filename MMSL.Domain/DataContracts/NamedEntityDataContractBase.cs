using MMSL.Domain.Entities;

namespace MMSL.Domain.DataContracts {
    public abstract class NamedEntityDataContractBase<TEntity> : EntityDataContractBase<TEntity> where TEntity : EntityBaseNamed {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
