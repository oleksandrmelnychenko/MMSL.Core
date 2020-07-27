using MMSL.Domain.Entities.Fabrics;

namespace MMSL.Domain.DataContracts.Fabrics {
    public class NewFabricDataContract {
        public string FabricCode { get; set; }
        public string Description { get; set; }
        public FabricStatuses Status { get; set; }

        public string Composition { get; set; }
        public string Pattern { get; set; }
        public float Metres { get; set; }
        public string Weave { get; set; }
        public string Color { get; set; }
        public string Mill { get; set; }
        public string GSM { get; set; }
        public string Count { get; set; }

        public Fabric GetEntity() {
            return new Fabric() {
                FabricCode = FabricCode,
                Description = Description,
                Status = Status,
                Composition = Composition,
                Pattern = Pattern,
                Color = Color,
                Count = Count,
                GSM = GSM,
                Metres = Metres.ToString(),
                Mill = Mill,
                Weave = Weave
            };
        }
    }
}
