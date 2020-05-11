namespace MMSL.Domain.Entities {
    public abstract class EntityBaseNamed : EntityBase {

        public string Name { get; set; }

        public string Description { get; set; }
    }
}
