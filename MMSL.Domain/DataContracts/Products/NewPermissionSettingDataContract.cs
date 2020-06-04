namespace MMSL.Domain.DataContracts.Products {
    public class NewPermissionSettingDataContract {
        public bool IsAllow { get; set; }
        public long OptionGroupId { get; set; }
        public long? OptionUnitId { get; set; }
    }
}
