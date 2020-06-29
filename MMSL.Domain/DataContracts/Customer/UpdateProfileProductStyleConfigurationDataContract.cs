namespace MMSL.Domain.DataContracts.Customer {
    public class UpdateProfileProductStyleConfigurationDataContract {
        public long Id { get; set; }
        public bool IsDeleted { get; set; }
        public string SelectedStyleValue { get; set; }
        public long OptionUnitId { get; set; }
    }
}
