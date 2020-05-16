using MMSL.Domain.Entities;

namespace MMSL.Domain.DataContracts {
    public abstract class EntityDataContractBase<TEntity> where TEntity : EntityBaseNamed {
        public long Id { get; set; }
        public abstract TEntity GetEntity();
    }
}
