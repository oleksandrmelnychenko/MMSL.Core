using MMSL.Domain.Entities.Fabrics;

namespace MMSL.Domain.DataContracts.Fabrics {
    public class UpdateFabricDataContract : EntityDataContractBase<Fabric> {

        public string FabricCode { get; set; }
        public string Description { get; set; }
        public FabricStatuses Status { get; set; }

        public string ImageUrl { get; set; }

        public string Composition { get; set; }
        public string Pattern { get; set; }
        public float Metres { get; set; }
        public string Weave { get; set; }
        public string Color { get; set; }
        public string Mill { get; set; }
        public string GSM { get; set; }
        public string Count { get; set; }

        public override Fabric GetEntity() {
            return new Fabric() {
                Id = Id,
                FabricCode = FabricCode,
                Description = Description,
                Status = Status,
                Composition = Composition,
                Pattern = Pattern,
                Color = Color,
                Count = Count,
                GSM = GSM,
                Metres = Metres,
                Mill = Mill,
                Weave = Weave,
                ImageUrl = ImageUrl
            };
        }
    }
}
